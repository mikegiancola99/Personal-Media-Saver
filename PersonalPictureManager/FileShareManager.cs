using System;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using ActiveDs;
namespace PersonalPictureManager
{
    class FileShareManager
    {
        public enum NetError
        {
            NERR_Success = 0,
            NERR_BASE = 2100,
            NERR_UnknownDevDir = (NERR_BASE + 16),
            NERR_DuplicateShare = (NERR_BASE + 18),
            NERR_BufTooSmall = (NERR_BASE + 23),
        }
        public enum SHARE_TYPE : ulong
        {
            STYPE_DISKTREE = 0,
            STYPE_PRINTQ = 1,
            STYPE_DEVICE = 2,
            STYPE_IPC = 3,
            STYPE_SPECIAL = 0x80000000,
        }

        [ StructLayout( LayoutKind.Sequential )]
        public struct SHARE_INFO_502
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi502_netname;
            public uint shi502_type;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi502_remark;
            public Int32 shi502_permissions;
            public Int32 shi502_max_uses;
            public Int32 shi502_current_uses;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi502_path;
            public IntPtr shi502_passwd;
            public Int32 shi502_reserved;
            public IntPtr shi502_security_descriptor;
        }

        [DllImport("Netapi32.dll")]
        public static extern int NetShareAdd([MarshalAs(UnmanagedType.LPWStr)]
                                              string strServer, 
                                              Int32 dwLevel, 
                                              IntPtr buf, 
                                              IntPtr parm_err);

        public FileShareManager.NetError CreateShare(string strServer,
                                              string strPath,
                                              string strShareName,
                                              string strShareDesc,
                                              bool bAdmin)
        {
            FileShareManager.SHARE_INFO_502 shInfo =
                  new FileShareManager.SHARE_INFO_502();
            shInfo.shi502_netname = strShareName;
            shInfo.shi502_type =
                (uint)FileShareManager.SHARE_TYPE.STYPE_DISKTREE;
            if (bAdmin)
            {
                shInfo.shi502_type =
                    (uint)FileShareManager.SHARE_TYPE.STYPE_SPECIAL;
                shInfo.shi502_netname += "$";
            }
            shInfo.shi502_permissions = 0;
            shInfo.shi502_path = strPath;
            shInfo.shi502_passwd = IntPtr.Zero;
            shInfo.shi502_remark = strShareDesc;
            shInfo.shi502_max_uses = -1;
            shInfo.shi502_security_descriptor = IntPtr.Zero;

            string strTargetServer = strServer;
            if (strServer.Length != 0)
            {
                strTargetServer = strServer;
                if (strServer[0] != '\\')
                {
                    strTargetServer = "\\\\" + strServer;
                }
            }
            int nRetValue = 0;
            // Call Net API to add the share..
            int nStSize = Marshal.SizeOf(shInfo);
            IntPtr buffer = Marshal.AllocCoTaskMem(nStSize);
            Marshal.StructureToPtr(shInfo, buffer, false);
            nRetValue = FileShareManager.NetShareAdd(strTargetServer, 502,
                    buffer, IntPtr.Zero);
            Marshal.FreeCoTaskMem(buffer);

            return (FileShareManager.NetError)nRetValue;
        }



    }
    
    class AD_ShareUtil
    {
        /*
        static void Main(string[] args)
        {
            string strServer = @"HellRaiser";
            string strShareFolder = @"G:\Mp3folder";
            string strShareName = @"MyMP3Share";
            string strShareDesc = @"Share to store MP3 files";
            FileShareManager.NetError nRetVal = 0;
            AD_ShareUtil shUtil = new AD_ShareUtil();
            nRetVal = shUtil.CreateShare(strServer, 
              strShareFolder, strShareName, strShareDesc, false);
            if (nRetVal == FileShareManager.NetError.NERR_Success)
            {
                Console.WriteLine("Share {0} created", strShareName);
            }
            else if (nRetVal == FileShareManager.NetError.NERR_DuplicateShare)
            {
                Console.WriteLine("Share {0} already exists", 
                          strShareName);
            }
        }
        */

    }
}  