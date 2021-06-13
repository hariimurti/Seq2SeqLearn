
namespace SampleApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bInsert = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAnswerInput = new System.Windows.Forms.TextBox();
            this.tbQuestionInput = new System.Windows.Forms.TextBox();
            this.labelTotalConv = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelLastTry = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.labelTrained = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bStop = new System.Windows.Forms.Button();
            this.bTrain = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.bPredict = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbAnswerOutput = new System.Windows.Forms.TextBox();
            this.tbQuestionOutput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelProgress = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelRuntime = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelTotalConv);
            this.groupBox1.Controls.Add(this.bInsert);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbAnswerInput);
            this.groupBox1.Controls.Add(this.tbQuestionInput);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 117);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Conversation";
            // 
            // bInsert
            // 
            this.bInsert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bInsert.Location = new System.Drawing.Point(363, 43);
            this.bInsert.Name = "bInsert";
            this.bInsert.Size = new System.Drawing.Size(111, 59);
            this.bInsert.TabIndex = 4;
            this.bInsert.Text = "INSERT";
            this.bInsert.UseVisualStyleBackColor = true;
            this.bInsert.Click += new System.EventHandler(this.bInsert_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Answer :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Question :";
            // 
            // tbAnswerInput
            // 
            this.tbAnswerInput.Location = new System.Drawing.Point(9, 82);
            this.tbAnswerInput.Name = "tbAnswerInput";
            this.tbAnswerInput.Size = new System.Drawing.Size(348, 20);
            this.tbAnswerInput.TabIndex = 1;
            // 
            // tbQuestionInput
            // 
            this.tbQuestionInput.Location = new System.Drawing.Point(9, 43);
            this.tbQuestionInput.Name = "tbQuestionInput";
            this.tbQuestionInput.Size = new System.Drawing.Size(348, 20);
            this.tbQuestionInput.TabIndex = 0;
            // 
            // labelTotalConv
            // 
            this.labelTotalConv.AutoEllipsis = true;
            this.labelTotalConv.Location = new System.Drawing.Point(396, 27);
            this.labelTotalConv.Name = "labelTotalConv";
            this.labelTotalConv.Size = new System.Drawing.Size(80, 13);
            this.labelTotalConv.TabIndex = 2;
            this.labelTotalConv.Text = "0000";
            this.labelTotalConv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelRuntime);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.labelProgress);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.labelLastTry);
            this.groupBox3.Controls.Add(this.labelMessage);
            this.groupBox3.Controls.Add(this.labelTrained);
            this.groupBox3.Controls.Add(this.labelStatus);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.bStop);
            this.groupBox3.Controls.Add(this.bTrain);
            this.groupBox3.Location = new System.Drawing.Point(12, 135);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(480, 117);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Seq2Seq";
            // 
            // labelLastTry
            // 
            this.labelLastTry.AutoEllipsis = true;
            this.labelLastTry.Location = new System.Drawing.Point(68, 71);
            this.labelLastTry.Name = "labelLastTry";
            this.labelLastTry.Size = new System.Drawing.Size(110, 13);
            this.labelLastTry.TabIndex = 9;
            this.labelLastTry.Text = ": ....";
            // 
            // labelMessage
            // 
            this.labelMessage.AutoEllipsis = true;
            this.labelMessage.Location = new System.Drawing.Point(68, 93);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(289, 13);
            this.labelMessage.TabIndex = 8;
            this.labelMessage.Text = ": ....";
            // 
            // labelTrained
            // 
            this.labelTrained.AutoEllipsis = true;
            this.labelTrained.Location = new System.Drawing.Point(68, 49);
            this.labelTrained.Name = "labelTrained";
            this.labelTrained.Size = new System.Drawing.Size(110, 13);
            this.labelTrained.TabIndex = 7;
            this.labelTrained.Text = ": ....";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoEllipsis = true;
            this.labelStatus.Location = new System.Drawing.Point(68, 27);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(289, 13);
            this.labelStatus.TabIndex = 6;
            this.labelStatus.Text = ": ....";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Last Try";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Message";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Trained";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Status";
            // 
            // bStop
            // 
            this.bStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bStop.Enabled = false;
            this.bStop.Location = new System.Drawing.Point(363, 83);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(111, 23);
            this.bStop.TabIndex = 1;
            this.bStop.Text = "STOP";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bTrain
            // 
            this.bTrain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bTrain.Location = new System.Drawing.Point(363, 22);
            this.bTrain.Name = "bTrain";
            this.bTrain.Size = new System.Drawing.Size(111, 55);
            this.bTrain.TabIndex = 0;
            this.bTrain.Text = "TRAIN";
            this.bTrain.UseVisualStyleBackColor = true;
            this.bTrain.Click += new System.EventHandler(this.bTrain_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.bPredict);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.tbAnswerOutput);
            this.groupBox4.Controls.Add(this.tbQuestionOutput);
            this.groupBox4.Location = new System.Drawing.Point(12, 258);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(480, 119);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Talk with Robot";
            // 
            // bPredict
            // 
            this.bPredict.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bPredict.Enabled = false;
            this.bPredict.Location = new System.Drawing.Point(363, 43);
            this.bPredict.Name = "bPredict";
            this.bPredict.Size = new System.Drawing.Size(111, 59);
            this.bPredict.TabIndex = 9;
            this.bPredict.Text = "PREDICT";
            this.bPredict.UseVisualStyleBackColor = true;
            this.bPredict.Click += new System.EventHandler(this.bPredict_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Answer :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Question :";
            // 
            // tbAnswerOutput
            // 
            this.tbAnswerOutput.Location = new System.Drawing.Point(9, 82);
            this.tbAnswerOutput.Name = "tbAnswerOutput";
            this.tbAnswerOutput.ReadOnly = true;
            this.tbAnswerOutput.Size = new System.Drawing.Size(348, 20);
            this.tbAnswerOutput.TabIndex = 6;
            // 
            // tbQuestionOutput
            // 
            this.tbQuestionOutput.Location = new System.Drawing.Point(9, 43);
            this.tbQuestionOutput.Name = "tbQuestionOutput";
            this.tbQuestionOutput.Size = new System.Drawing.Size(348, 20);
            this.tbQuestionOutput.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(362, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Total :";
            // 
            // labelProgress
            // 
            this.labelProgress.AutoEllipsis = true;
            this.labelProgress.Location = new System.Drawing.Point(247, 49);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(110, 13);
            this.labelProgress.TabIndex = 11;
            this.labelProgress.Text = ": ....";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(185, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Progress";
            // 
            // labelRuntime
            // 
            this.labelRuntime.AutoEllipsis = true;
            this.labelRuntime.Location = new System.Drawing.Point(247, 71);
            this.labelRuntime.Name = "labelRuntime";
            this.labelRuntime.Size = new System.Drawing.Size(110, 13);
            this.labelRuntime.TabIndex = 13;
            this.labelRuntime.Text = ": ....";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(185, 71);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(46, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Runtime";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 387);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Seq2Seq - Sample Application";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bInsert;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAnswerInput;
        private System.Windows.Forms.TextBox tbQuestionInput;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Button bTrain;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button bPredict;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbAnswerOutput;
        private System.Windows.Forms.TextBox tbQuestionOutput;
        private System.Windows.Forms.Label labelTotalConv;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelLastTry;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Label labelTrained;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelRuntime;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label label11;
    }
}

