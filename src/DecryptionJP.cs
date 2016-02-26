/**
* DecryptionJP.cs
* SIF JP Decryption routines
**/

namespace DEP
{
    namespace HonokaMiku
    {
        using System;
        using System.Net;
        using System.IO;
        using System.Security.Cryptography;
        using System.Text;
        using DEP.HonokaMiku.Core;

        public class JP_Dctx:DecrypterContext
        {
            private static readonly UInt32[] keyTables={
                1210253353	,1736710334	,1030507233	,1924017366,
                1603299666	,1844516425	,1102797553	,32188137,
                782633907	,356258523	,957120135	,10030910,
                811467044	,1226589197	,1303858438	,1423840583,
                756169139	,1304954701	,1723556931	,648430219,
                1560506399	,1987934810	,305677577	,505363237,
                450129501	,1811702731	,2146795414	,842747461,
                638394899	,51014537	,198914076	,120739502,
                1973027104	,586031952	,1484278592	,1560111926,
                441007634	,1006001970	,2038250142	,232546121,
                827280557	,1307729428	,775964996	,483398502,
                1724135019	,2125939248	,742088754	,1411519905,
                136462070	,1084053905	,2039157473	,1943671327,
                650795184	,151139993	,1467120569	,1883837341,
                1249929516	,382015614	,1020618905	,1082135529,
                870997426	,1221338057	,1623152467	,1020681319
            };
            protected JP_Dctx():base() {}
            public JP_Dctx(Byte[] FileHeader,String Filename):base()
            {
                if(FileHeader==null) throw new ArgumentNullException("FileHeader");
                if(String.IsNullOrEmpty(Filename)) throw new ArgumentNullException("Filename");
                if(FileHeader.Length<16) throw new DecrypterException("Invalid 'FileHeader' size. Expected size is 16 bytes");

                MD5 mctx=MD5.Create();
                Byte[] temp;
                Byte[] filename;
                Byte[] md5_out;
                UInt16 name_sum=500;

                temp=Encoding.UTF8.GetBytes("Hello");
                mctx.TransformBlock(temp,0,5,temp,0);
                filename=Encoding.UTF8.GetBytes(Path.GetFileName(Filename));
                mctx.TransformFinalBlock(filename,0,filename.Length);
                md5_out=mctx.Hash;

                if(FileHeader[0]==(~md5_out[4]) && FileHeader[1]==(~md5_out[5]) && FileHeader[2]==(~md5_out[6]))
                {
                    foreach(Byte b in filename)
                        name_sum+=b;
                    if(name_sum==IPAddress.NetworkToHostOrder((UInt16)(FileHeader[10]*256+FileHeader[11])))
                    {
                        InitKey=UpdateKey=keyTables[FileHeader[11]&63];
                        XorKey=(UInt16)(UpdateKey>>24);
                        Position=0;
                        return;
                    }
                    else
                        throw new DecrypterException("Invalid filename. Possibily file is renamed or wrong 'Filename' is passed");
                }
                else
                    throw new DecrypterException("Header file doesn't match");
            }
            public override void DecryptBlock(Byte[] Buffer)
            {
                if(Buffer==null) throw new ArgumentNullException("Buffer");
                if(Buffer.Length==0) return;

                for(UInt32 i=0;i<Buffer.Length;i++)
                {
                    Buffer[i]^=(Byte)XorKey;
                    Update();
                }
            }
            public override void JumpOffset(Int64 Offset,Boolean Relative=false)
            {
                if(Relative)
                {
                    if(Offset==0) return;
                    else if(Offset>0)
                    {
                        for(UInt32 i=0;i<Offset;i++)
                            Update();
                        Position+=(UInt64)Offset;
                        return;
                    }
                    else if(Offset<0)
                        Position-=(UInt64)(-Offset);
                }
                else
                    if(Offset<0)
                        throw new ArgumentOutOfRangeException("Offset");
                    else
                        Position=(UInt64)Offset;

                XorKey=(UInt16)((UpdateKey=InitKey)>>24);
                for(UInt32 i=0;i<Position;i++)
                    Update();
            }
            protected override void Update()
            {
                XorKey=(UInt16)((UpdateKey=UpdateKey*214013+2531011)>>24);
            }
            static public JP_Dctx EncryptSetup(String Filename,Stream HeaderOut)
            {
                if(String.IsNullOrEmpty(Filename)) throw new ArgumentNullException("Filename");
                if(HeaderOut==null) throw new ArgumentNullException("HeaderOut");

                BinaryWriter binwrite=new BinaryWriter(HeaderOut);
                JP_Dctx dctx=new JP_Dctx();
                MD5 mctx=MD5.Create();
                UInt16 namesum=500;
                Byte[] md5_out;
                Byte[] temp;
                Byte[] filename;

                temp=Encoding.UTF8.GetBytes("Hello");
                mctx.TransformBlock(temp,0,5,temp,0);
                filename=Encoding.UTF8.GetBytes(Path.GetFileName(Filename));
                mctx.TransformFinalBlock(filename,0,filename.Length);
                md5_out=mctx.Hash;

                temp=new Byte[16]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
                for(UInt32 i=0;i<3;i++)
                    temp[i]=(Byte)(~md5_out[i]);
                binwrite.Write(temp);
                binwrite.Seek(-6,SeekOrigin.Current);

                foreach(Byte b in filename)
                    namesum+=b;
                binwrite.Write(IPAddress.HostToNetworkOrder(namesum));
                dctx.XorKey=(UInt16)((dctx.InitKey=dctx.UpdateKey=keyTables[namesum&64])>>24);

                return dctx;
            }
            static public JP_Dctx EncryptSetup(String Filename,Byte[] HeaderOut)
            {
                if(String.IsNullOrEmpty(Filename)) throw new ArgumentNullException("Filename");
                if(HeaderOut==null) throw new ArgumentNullException("HeaderOut");
                if(HeaderOut.Length<16) throw new ArgumentException("HeaderOut is less than 16 bytes in size","HeaderOut");

                MemoryStream mem=new MemoryStream(16);
                JP_Dctx dctx=JP_Dctx.EncryptSetup(Filename,mem);
                Buffer.BlockCopy(mem.ToArray(),0,HeaderOut,0,16);

                return dctx;
            }
        };
    }
}
