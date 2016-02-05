/**
* DecryptionV2.cs
* Implements SIF EN, TW, KR, and CN decrypter
**/

#define DEFENCRYPTSETUP(Type) \
            static public Type EncryptSetup(String Filename,Stream HeaderOut) \
            { \
                if(String.IsNullOrEmpty(Filename)) throw new ArgumentNullException("Filename"); \
                if(HeaderOut==null) throw new ArgumentNullException("HeaderOut"); \
                Type dctx=new Type(); \
                DecrypterContext.__SetupEncryptV2(dctx,Type.Prefix,Filename,HeaderOut); \
                return dctx; \
            }

namespace DEP
{
    namespace HonokaMiku
    {
        using System;
        using System.IO;
        using DEP.HonokaMiku.Core;

        public class EN_Dctx:V2_Dctx
        {
            static readonly String Prefix="BFd3EnkcKa";
            protected EN_Dctx():base() {}
            public EN_Dctx(Byte[] FileHeader,String Filename):base(EN_Dctx.Prefix,FileHeader,Filename) {}
            DEFENCRYPTSETUP(EN_Dctx)
        }

        public class TW_Dctx:V2_Dctx
        {
            static readonly String Prefix="M2o2B7i3M6o6N88";
            protected TW_Dctx():base() {}
            public TW_Dctx(Byte[] FileHeader,String Filename):base(TW_Dctx.Prefix,FileHeader,Filename) {}
            DEFENCRYPTSETUP(TW_Dctx)
        };

        public class KR_Dctx:V2_Dctx
        {
            static readonly String Prefix="Hello";
            protected KR_Dctx():base() {}
            public KR_Dctx(Byte[] FileHeader,String Filename):base(KR_Dctx.Prefix,FileHeader,Filename) {}
            DEFENCRYPTSETUP(KR_Dctx)
        };

        public class CN_Dctx:V2_Dctx
        {
            static readonly String Prefix="iLbs0LpvJrXm3zjdhAr4";
            protected CN_Dctx():base() {}
            public CN_Dctx(Byte[] FileHeader,String Filename):base(CN_Dctx.Prefix,FileHeader,Filename) {}
            DEFENCRYPTSETUP(CN_Dctx)
        };
    }
}