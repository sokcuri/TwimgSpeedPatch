using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SimpleJSON;

namespace TwimgSpeedPatch
{
    public static class HostFileManager
    {
        private static readonly string HostFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts");
        private static readonly string HostName = "pbs.twimg.com";

        private static JObject TwImgNodeIPs = null;

        public static void Init()
        {
            try
            {
                WebRequest request =
                    WebRequest.Create(
                        "https://raw.githubusercontent.com/sokcuri/TwimgSpeedPatch/master/data/server_ip.json");
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        String responseString = reader.ReadToEnd();

                        try
                        {
                            TwImgNodeIPs = JSONDecoder.Decode(responseString);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("JSON Decode를 실패했습니다: " + e.Message, "오류", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            Environment.Exit(1);
                        }

                        for (int i = 0; i < TwImgNodeIPs.Count; i++)
                        {
                            if (TwImgNodeIPs[i].StringValue == "")
                            {
                                MessageBox.Show($"정상적인 ip가 아닙니다. i: {i}, value: {TwImgNodeIPs[i].StringValue}", "오류", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                Environment.Exit(3);
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show("트위터 노드 서버 정보를 가져올 수 없습니다. 인터넷 연결을 확인한 후 다시 시도해주세요: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(2);
            }
            
        }

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
                w.WriteLine($"{TwImgNodeIPs[new Random().Next(TwImgNodeIPs.Count)].StringValue} {HostName}");
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
