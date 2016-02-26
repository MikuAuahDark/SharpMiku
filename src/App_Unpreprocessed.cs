/**
* App.cs
* Equivalent of HonokaMiku.cc in HonokaMiku C++ Implementation
* Meant to be preprocessed by GCC/MSVC first!
**/

#define DEPCASEISTRCMP(x,y,z) String.Equals(x,y,StringComparison.OrdinalIgnoreCase)

namespace DEP
{
    namespace HonokaMiku
    {
        namespace App
        {
            using System;
            using System.IO;
            using DEP.HonokaMiku;
            using Dctx=DEP.HonokaMiku.Core.DecrypterContext;

            class SharpMiku
            {
                static Byte g_DecryptGame=0;
                static Boolean g_Encrypt=false;
                static Dctx g_Dctx=null;
                static String g_Basename=null;
                static Int32 g_InPos=0;
                static Int32 g_OutPos=0;
                static Byte g_XEncryptGame=0;
                static Boolean g_TestMode=false;
                static Byte get_gametype(String a)
                {
	                if(DEPCASEISTRCMP(a,"w",2) || DEPCASEISTRCMP(a,"sif-ww",7) || DEPCASEISTRCMP(a,"sif-en",10))
		                return 1;
	                else if(DEPCASEISTRCMP(a,"j",2) || DEPCASEISTRCMP(a,"sif-jp",7))
		                return 2;
	                else if(DEPCASEISTRCMP(a,"t",2) || DEPCASEISTRCMP(a,"sif-tw",7))
		                return 3;
	                else if(DEPCASEISTRCMP(a,"k",2) || DEPCASEISTRCMP(a,"sif-kr",7))
		                return 4;
	                else if(DEPCASEISTRCMP(a,"c",2) || DEPCASEISTRCMP(a,"sif-cn",7))
		                return 5;
	                return 0;
                }
                static void Main(String[] argv)
                {
                    Console.Error.WriteLine("HonokaMiku. Universal LL!SIF game files decrypter");
                    
                }
            }
        }
    }
}