/**
* DecryptV2.cs
* Base class of Version 2 Decrypter
**/

namespace DEP
{
    namespace HonokaMiku
    {
        namespace Core
        { 
            using System;
            using System.Net;
            using System.IO;
            using System.Security.Cryptography;
            using System.Text;

            public class V2_Dctx:DecrypterContext
            {
                protected V2_Dctx():base() {}
                public V2_Dctx(String Prefix,Byte[] FileHeader,String Filename)
                {
                    // No need to check for Prefix
                    if(FileHeader==null) throw new ArgumentNullException("FileHeader");
                    if(String.IsNullOrEmpty(Filename)) throw new ArgumentNullException("Filename");

                    MD5 mctx=MD5.Create();
                    Byte[] temp;
                    Byte[] md5_out;

                    temp=Encoding.UTF8.GetBytes(Prefix);
                    mctx.TransformBlock(temp,0,temp.Length,temp,0);
                    temp=Encoding.UTF8.GetBytes(Filename);
                    mctx.TransformFinalBlock(temp,0,temp.Length);
                    md5_out=mctx.Hash;

                    if(FileHeader[0]==md5_out[4] && FileHeader[1]==md5_out[5] && FileHeader[2]==md5_out[6] && FileHeader[3]==md5_out[7])
                    {
                        UpdateKey=InitKey=(UInt32)(md5_out[0]<<24|md5_out[1]<<16|md5_out[2]<<8|md5_out[3])&2147483647;
                        XorKey=(UInt16)((UpdateKey>>23)&255|(UpdateKey>>7)&65280);
                        return;
                    }
                    else
                        throw new DecrypterException("Invalid header. Possibily file is renamed or incompatible game file");
                }
                public override void DecryptBlock(Byte[] Buffer)
                {
                    if(Buffer==null) throw new ArgumentNullException("Buffer");
                    if(Buffer.Length==0) return;

                    UInt32 buflen=(UInt32)Buffer.Length;
                    UInt64 oldpos=Position;

                    Position+=buflen;
                    for(UInt64 i=oldpos;i<Position;i++)
	                {
		                if(i%2==0)
			                Buffer[i-oldpos]^=(Byte)(XorKey&255);
		                else
		                {
			                Buffer[i-oldpos]^=(Byte)(XorKey>>8);
			                Update();
		                }
	                }
                }
                public override void JumpOffset(Int64 Offset,Boolean Relative=false)
                {
                    if(Relative)
                    {
                        if(Offset==0) return;
                        else if(Offset>0)
                        {
                            for(UInt32 i=0;i<Offset/2;i++)
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
                    
                    UpdateKey=InitKey;
                    XorKey=(UInt16)((UpdateKey>>23)&255|(UpdateKey>>7)&65280);
                    for(UInt32 i=0;i<Position/2;i++)
                        Update();
                }
                protected override void Update()
                {
	                UInt32 a,b,c,d;

	                a=UpdateKey>>16;
	                b=((a*1101463552)&2147483647)+(UpdateKey&65535)*16807;
	                c=(a*16807)>>15;
	                d=c+b-2147483647;
	                b=b>2147483646?d:b+c;
	                UpdateKey=b;
	                XorKey=(UInt16)((b>>23)&255|(b>>7)&65280);;
                }
            };
        }
    }
}