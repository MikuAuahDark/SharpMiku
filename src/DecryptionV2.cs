// This file is preprocessed by GCC

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
            static public EN_Dctx EncryptSetup(String Filename,Stream HeaderOut) { if(String.IsNullOrEmpty(Filename)) throw new ArgumentNullException("Filename"); if(HeaderOut==null) throw new ArgumentNullException("HeaderOut"); EN_Dctx dctx=new EN_Dctx(); DecrypterContext.__SetupEncryptV2(dctx,EN_Dctx.Prefix,Filename,HeaderOut); return dctx; }
        }

        public class TW_Dctx:V2_Dctx
        {
            static readonly String Prefix="M2o2B7i3M6o6N88";
            protected TW_Dctx():base() {}
            public TW_Dctx(Byte[] FileHeader,String Filename):base(TW_Dctx.Prefix,FileHeader,Filename) {}
            static public TW_Dctx EncryptSetup(String Filename,Stream HeaderOut) { if(String.IsNullOrEmpty(Filename)) throw new ArgumentNullException("Filename"); if(HeaderOut==null) throw new ArgumentNullException("HeaderOut"); TW_Dctx dctx=new TW_Dctx(); DecrypterContext.__SetupEncryptV2(dctx,TW_Dctx.Prefix,Filename,HeaderOut); return dctx; }
        };

        public class KR_Dctx:V2_Dctx
        {
            static readonly String Prefix="Hello";
            protected KR_Dctx():base() {}
            public KR_Dctx(Byte[] FileHeader,String Filename):base(KR_Dctx.Prefix,FileHeader,Filename) {}
            static public KR_Dctx EncryptSetup(String Filename,Stream HeaderOut) { if(String.IsNullOrEmpty(Filename)) throw new ArgumentNullException("Filename"); if(HeaderOut==null) throw new ArgumentNullException("HeaderOut"); KR_Dctx dctx=new KR_Dctx(); DecrypterContext.__SetupEncryptV2(dctx,KR_Dctx.Prefix,Filename,HeaderOut); return dctx; }
        };

        public class CN_Dctx:V2_Dctx
        {
            static readonly String Prefix="iLbs0LpvJrXm3zjdhAr4";
            protected CN_Dctx():base() {}
            public CN_Dctx(Byte[] FileHeader,String Filename):base(CN_Dctx.Prefix,FileHeader,Filename) {}
            static public CN_Dctx EncryptSetup(String Filename,Stream HeaderOut) { if(String.IsNullOrEmpty(Filename)) throw new ArgumentNullException("Filename"); if(HeaderOut==null) throw new ArgumentNullException("HeaderOut"); CN_Dctx dctx=new CN_Dctx(); DecrypterContext.__SetupEncryptV2(dctx,CN_Dctx.Prefix,Filename,HeaderOut); return dctx; }
        };
    }
}
