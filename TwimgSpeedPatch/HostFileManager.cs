using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TwimgSpeedPatch
{
    public static class HostFileManager
    {
        private static readonly string HostFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts");
        private static readonly string[] TwImgNodeIPs = new[] {"23.1.106.237", "184.31.10.237", "151.101.52.159"};
        private static readonly string HostName = "pbs.twimg.com";

        public static void Patch()
        {
            if (IsPatched())
            {
                return;
            }
            bool endLF = IsHostFileLFOfEnd();
            using (var w = File.AppendText(HostFileName))
            {
                if (!endLF)
                {
                    w.WriteLine("");
                }
                w.WriteLine($"{TwImgNodeIPs[new Random().Next(3)]} {HostName}");
            }
            MessageBox.Show("패치되었습니다", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void UnPatch()
        {
            if (!IsPatched())
            {
                return;
            }

            string contents = String.Empty;
            using (var sr = new StreamReader(HostFileName))
            {
                string s;
                while (!sr.EndOfStream)
                {
                    s = sr.ReadLine();
                    if (!IsPatchedLine(s))
                    {
                        contents += s + "\r\n";
                    }
                }
            }
            File.WriteAllText(HostFileName, contents);

            MessageBox.Show("패치를 제거했습니다", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void OpenHostFile()
        {
            Process.Start("notepad.exe", HostFileName);
        }

        private static bool IsPatched()
        {
            using (var sr = new StreamReader(HostFileName))
            {
                string s;
                while (!sr.EndOfStream)
                {
                    s = sr.ReadLine();
                    if (IsPatchedLine(s))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsPatchedLine(string line)
        {
            var pattern = $@"[^\s\t].+[\s\t]+{HostName}\t?";
            var m = Regex.Match(line, pattern,
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return m.Success;
        }

        private static bool IsHostFileLFOfEnd()
        {
            using (var sr = new StreamReader(HostFileName))
            {
                while (!sr.EndOfStream)
                {
                    sr.ReadLine();
                }

                sr.BaseStream.Seek(-2, SeekOrigin.End);

                sr.Read();
                var s1 = sr.Read();
                return s1 == 0x0A;
            }
        }
    }
}
