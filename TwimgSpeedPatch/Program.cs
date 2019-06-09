using System;
using System.Diagnostics;
using System.Net;
using System.Security.Principal;
using System.Windows.Forms;

namespace TwimgSpeedPatch
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool administrativeMode = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!administrativeMode)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.Verb = "runas";
                startInfo.FileName = Application.ExecutablePath;
                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    MessageBox.Show("Hosts 파일 수정을 위해 관리자 권한이 필요합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                return;
            }

            WebRequest.DefaultWebProxy = null;
            WebRequest.DefaultCachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            ServicePointManager.MaxServicePoints = 100;
            
            Application.Run(new Form1());
        }
    }
}
