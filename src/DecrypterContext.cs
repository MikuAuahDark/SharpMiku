/**
* DecrypterContext.cs
* Definition of HonokaMiku decrypter base class
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
            using System.Runtime.InteropServices;
            using System.Security.Cryptography;
            using System.Text;

            /// <summary>
            ///   Exception thrown on <c>DecrypterContext</c> class
            /// </summary>
            public class DecrypterException:Exception
            {
                public DecrypterException(String msg):base(msg) {}
                public DecrypterException(String msg,Exception inn):base(msg,inn) {}
            };

            /// <summary>
            ///   Base class of all decryption function
            /// </summary>
            abstract public class DecrypterContext
            {
                // Current decrypt position
                protected UInt64 Position;
                // Key to used for jump
                protected UInt32 InitKey;
                // Key to used for next decryption cycle
                protected UInt32 UpdateKey;
                // Value used to XOR bytes
                protected UInt16 XorKey;
                // Protected constructor
                protected DecrypterContext() {
                    Position=0;
                    InitKey=0;
                    UpdateKey=0;
                    XorKey=0;
                }
                /// <summary>
                ///   Decrypt bytes in <c>Buffer</c> and modify it's contents
                /// </summary>
                /// <param name="Buffer">Bytes that want to be decrypted</param>
                abstract public void DecryptBlock(Byte[] Buffer);
                /// <summary>
                ///   Jumps to specificed offset
                /// </summary>
                /// <param name="Offset">Where?</param>
                /// <param name="Relative">Jumps to relative location?</param>
                abstract public void JumpOffset(Int64 Offset,Boolean Relative=false);
                // Used internally
                abstract protected void Update();

                /// <summary>
                ///   Decrypt bytes in <c>Buffer</c> with option to return the new decrypted bytes
                /// </summary>
                /// <param name="Buffer">Bytes that want to be decrypted</param>
                /// <param name="NewBuffer">Allocate new buffer for decrypted bytes?</param>
                public Byte[] DecryptBlock(Byte[] Buffer,Boolean NewBuffer)
                {
                    if(NewBuffer==false)
                    {
                        DecryptBlock(Buffer);
                        return Buffer;
                    }
                    Byte[] buf=new Byte[Buffer.Length];
                    DecryptBlock(buf);
                    return buf;
                }
                /// <summary>
                ///   Decrypts bytes pointed by <c>Buffer</c> with specificed size
                /// </summary>
                /// <param name="Buffer">Pointer to bytes that want to be decrypted</param>
                /// <param name="Size">Size of <c>Buffer</c>, in bytes</param>
                unsafe public void DecryptBlock(Byte* Buffer,UInt32 Size)
                {
                    Byte[] buf=new Byte[Size];
                    Marshal.Copy((IntPtr)Buffer,buf,0,(Int32)Size);
                    DecryptBlock(buf);
                    Marshal.Copy(buf,0,(IntPtr)Buffer,(Int32)Size);
                }
                /// <summary>
                ///   Jumps to absolute offset
                /// </summary>
                /// <param name="Offset">Where?</param>
                public void JumpOffset(UInt32 Offset)
                {
                    JumpOffset((Int64)Offset,false);
                }
                
                static protected void __SetupEncryptV2(DecrypterContext dctx,String Prefix,String Filename,Stream HeaderOut)
                {
                    MD5 mctx=MD5.Create();
                    Byte[] md5_out;
                    Byte[] temp;

                    temp=Encoding.UTF8.GetBytes(Prefix);
                    mctx.TransformBlock(temp,0,temp.Length,temp,0);
                    temp=Encoding.UTF8.GetBytes(Path.GetFileName(Filename));
                    mctx.TransformFinalBlock(temp,0,temp.Length);
                    md5_out=mctx.Hash;

                    HeaderOut.Write(md5_out,4,4);
                    dctx.InitKey=(UInt32)((md5_out[0]<<24)|(md5_out[1]<<16)|(md5_out[2]<<8)|md5_out[3])&2147483647;
                    dctx.UpdateKey=dctx.InitKey;
                    dctx.XorKey=(UInt16)((dctx.InitKey>>23)&255|(dctx.InitKey>>7)&65280);
                    dctx.Position=0;
                }
                static protected Byte[] __SetupEncryptV2(DecrypterContext dctx,String Prefix,String Filename)
                {
                    MemoryStream mem=new MemoryStream(4);
                    __SetupEncryptV2(dctx,Prefix,Filename,mem);
                    return mem.ToArray();
                }
            };
        }
    }
}
