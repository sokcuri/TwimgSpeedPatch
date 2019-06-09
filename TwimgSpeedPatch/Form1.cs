using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TwimgSpeedPatch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            HostFileManager.PingProgressChanged += this.HostFileManager_PingProgressChanged;
        }

        private void HostFileManager_PingProgressChanged(int percent)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int>(this.HostFileManager_PingProgressChanged), percent);
            else
            {
                this.progress.Visible = true;
                this.progress.Value = percent;
            }
        }

        private void PatchButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("최적의 패치를 위하여 시간이 소요될 수 있습니다.\n계속할까요?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            this.patchButton.Enabled = false;
            this.unpatchButton.Enabled = false;

            HostFileManager
                .PatchAsync()
                .ContinueWith(task =>
                {
                    string errMsg;
                    if (task.IsCompleted)
                        errMsg = task.Result ?? "패치되었습니다";
                    else
                        errMsg = "알 수 없는 오류가 발생하였습니다.";

                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show(errMsg, "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.patchButton.Enabled = true;
                        this.unpatchButton.Enabled = true;
                        this.progress.Visible = false;
                    }));
                });
        }

        private void UnpatchButton_Click(object sender, EventArgs e)
        {
            this.patchButton.Enabled = false;
            this.unpatchButton.Enabled = false;

            HostFileManager
                .UnpatchAsync()
                .ContinueWith(task =>
                {
                    string errMsg;
                    if (task.IsCompleted)
                        errMsg = task.Result ?? "패치를 제거했습니다";
                    else
                        errMsg = "알 수 없는 오류가 발생하였습니다.";

                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show(errMsg, "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.patchButton.Enabled = true;
                        this.unpatchButton.Enabled = true;
                    }));
                });
        }

        private void OpenHostFile_Click(object sender, EventArgs e)
        {
            HostFileManager.OpenHostFile();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/sokcuri/TwimgSpeedPatch");
        }
    }
}
