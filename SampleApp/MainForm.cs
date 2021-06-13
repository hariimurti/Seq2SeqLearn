using Seq2SeqLearn;
using Seq2SeqLearn.Events;
using Seq2SeqLearn.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SampleApp
{
    public partial class MainForm : Form
    {
        private readonly string filename = "conversation.txt";
        private List<string> input_raw = new List<string>();
        private Seq2Seq ss;
        private DateTime dt;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            ss = new Seq2Seq();
            ss.OnComplete += Ss_OnComplete;
            ss.OnProgress += Ss_OnProgress;
            ss.OnResume += Ss_OnResume;
            ss.OnStop += Ss_OnStop;

            if (File.Exists(filename))
                input_raw = File.ReadAllLines(filename).ToList();
            ReloadConversation();
        }

        private void Ss_OnResume(ResumeEventArgs obj)
        {
            Invoke((MethodInvoker)delegate {
                SetLabel(labelStatus, "Running (Resume)");
                SetLabel(labelTrained, obj.TrainedData.ToString());
                SetLabel(labelProgress, obj.InPercent().ToString());
                SetLabel(labelLastTry, obj.LastTime.ToString("dd/MM/yyyy - HH:mm"));
                SetLabel(labelRuntime, "00:00:00");
                SetLabel(labelMessage, string.Format("From {0}/{1} ({2}%)", obj.TrainedData, obj.TotalData, obj.InPercent()));
            });
        }

        private void Ss_OnStop(StopEventArgs obj)
        {
            Invoke((MethodInvoker)delegate {
                SetLabel(labelStatus, "Stopped");
                SetLabel(labelTrained, obj.TrainedData.ToString());
                SetLabel(labelProgress, obj.InPercent().ToString());
                SetLabel(labelLastTry, Runtime().ToString());
                SetLabel(labelMessage, obj.Message);

                bInsert.Enabled = true;
                bTrain.Enabled = true;
                bStop.Enabled = false;
                bPredict.Enabled = true;
            });
        }

        private void Ss_OnProgress(ProgressEventArgs obj)
        {
            Invoke((MethodInvoker)delegate {
                if (!labelStatus.Text.Contains("Resume")) SetLabel(labelStatus, "Running");
                SetLabel(labelTrained, string.Format("{0}/{1}", obj.TrainedData, obj.TotalData));
                SetLabel(labelProgress, obj.InPercent().ToString() + "%");
                //SetLabel(labelLastTry, obj.LastTime.ToString("dd/MM/yyyy - HH:mm"));
                SetLabel(labelRuntime, Runtime().ToString());
                SetLabel(labelMessage, string.Format("EP: {0} > Cost: {1}", obj.Epoch, obj.Cost));
            });
        }

        private void Ss_OnComplete(CompleteEventArgs obj)
        {
            Invoke((MethodInvoker)delegate {
                SetLabel(labelStatus, "Complete");
                SetLabel(labelTrained, obj.TrainedData.ToString());
                SetLabel(labelProgress, "100%");
                SetLabel(labelLastTry, obj.LastTime.ToString("dd/MM/yyyy - HH:mm"));
                SetLabel(labelRuntime, Runtime().ToString());
                SetLabel(labelMessage, obj.StartOver ? "I'm ready to play with you..." : "No need to train me again...");

                bInsert.Enabled = true;
                bTrain.Enabled = true;
                bStop.Enabled = false;
                bPredict.Enabled = true;
            });
        }

        private void bInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbQuestionInput.Text))
            {
                tbQuestionInput.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbAnswerInput.Text))
            {
                tbAnswerInput.Focus();
                return;
            }
            input_raw.Add(tbQuestionInput.Text.Trim());
            input_raw.Add(tbAnswerInput.Text.Trim());
            try
            {
                File.WriteAllLines(filename, input_raw.ToArray());
                tbQuestionInput.Text = "";
                tbAnswerInput.Text = "";
                ReloadConversation();
            }
            catch (Exception) { }
        }

        private void bTrain_Click(object sender, EventArgs e)
        {
            dt = DateTime.Now;
            ss.StartTraining(300);
            bInsert.Enabled = false;
            bTrain.Enabled = false;
            bStop.Enabled = true;
            bPredict.Enabled = false;
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            ss.StopTraining();
        }

        private void bPredict_Click(object sender, EventArgs e)
        {
            try
            {
                var result = ss.Predict(tbQuestionOutput.Text.SplitWordToList());
                tbAnswerOutput.Text = result.JoinToSentence();
            }
            catch (Exception ex)
            {
                tbAnswerOutput.Text = ex.Message;
            }
        }

        private void SetLabel(Label lb, string text)
        {
            lb.Text = ": " + (string.IsNullOrWhiteSpace(text) ? "...." : text);
        }

        private TimeSpan Runtime()
        {
            var runtime = DateTime.Now - dt;
            var tfs = TimeSpan.FromSeconds(runtime.TotalSeconds);
            return new TimeSpan(tfs.Days, tfs.Hours, tfs.Minutes, tfs.Seconds);
        }

        private void ReloadConversation()
        {
            if (input_raw.Count > 1)
            {
                var input_raw = this.input_raw.ToArray();

                var input = new List<List<string>>();
                var output = new List<List<string>>();

                for (int i = 0; i < input_raw.Length; i++)
                {
                    if (input_raw.Length % 2 == 1 && input_raw.Length - 1 == i) break;
                    var dt = input_raw[i].ToLower().SplitWordToList();

                    if (i % 2 == 0) input.Add(dt);
                    else output.Add(dt);
                }

                ss.SetData(32, 16, 1, input, output, false);
                var info = ss.GetTrainingInfo();

                bTrain.Enabled = true;
                bPredict.Enabled = info.TrainedData == info.TotalData && info.TotalData > 0;

                var trained = info.TrainedData.ToString();
                var progress = bPredict.Enabled ? "100%" : "0%";
                if (info.TrainedData != info.TotalData && info.TotalData > 0)
                {
                    trained = string.Format("{0}/{1}", info.TrainedData, info.TotalData);
                    progress = string.Format("{0}%", (int)((double)info.TrainedData / info.TotalData * 100));
                }

                labelTotalConv.Text = (input_raw.Length / 2).ToString();
                SetLabel(labelStatus, bPredict.Enabled ? "Complete" : "Ready");
                SetLabel(labelTrained, trained);
                SetLabel(labelProgress, progress);
                SetLabel(labelLastTry, info.LastTime.ToString("dd/MM/yyyy - HH:mm"));
                SetLabel(labelRuntime, "00:00:00");
                SetLabel(labelMessage, bPredict.Enabled ? "I'm ready to play with you..." : "Train me first!");
            }
            else
            {
                bTrain.Enabled = false;
                bPredict.Enabled = false;

                labelTotalConv.Text = "0";
                SetLabel(labelStatus, "Standby");
                SetLabel(labelTrained, "0");
                SetLabel(labelProgress, "0");
                SetLabel(labelLastTry, "00/00/00 - 00:00");
                SetLabel(labelRuntime, "00:00:00");
                SetLabel(labelMessage, "");
            }
        }
    }
}
