namespace RabbitMQ.Client.Queue
{
    partial class Dashboard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel2 = new Panel();
            label2 = new Label();
            pnl2ClientReply_btn = new Button();
            pnl2WhichClient_txt = new TextBox();
            listBox2 = new ListBox();
            panel1 = new Panel();
            listBox1 = new ListBox();
            label1 = new Label();
            pnl1SendRequest_btn = new Button();
            pnl1CountClient_txt = new TextBox();
            AllDeleteQueue_btn = new Button();
            listBox3 = new ListBox();
            allClientReadQueue_btn = new Button();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Control;
            panel2.Controls.Add(label2);
            panel2.Controls.Add(pnl2ClientReply_btn);
            panel2.Controls.Add(pnl2WhichClient_txt);
            panel2.Controls.Add(listBox2);
            panel2.Location = new Point(494, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(471, 426);
            panel2.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(96, 12);
            label2.Name = "label2";
            label2.Size = new Size(122, 15);
            label2.TabIndex = 7;
            label2.Text = "Cevap Alınacak Client";
            // 
            // pnl2ClientReply_btn
            // 
            pnl2ClientReply_btn.Location = new Point(304, 30);
            pnl2ClientReply_btn.Name = "pnl2ClientReply_btn";
            pnl2ClientReply_btn.Size = new Size(141, 23);
            pnl2ClientReply_btn.TabIndex = 6;
            pnl2ClientReply_btn.Text = "Mesajı AL";
            pnl2ClientReply_btn.UseVisualStyleBackColor = true;
            pnl2ClientReply_btn.Click += pnl2ClientReply_btn_Click;
            // 
            // pnl2WhichClient_txt
            // 
            pnl2WhichClient_txt.Location = new Point(95, 30);
            pnl2WhichClient_txt.Name = "pnl2WhichClient_txt";
            pnl2WhichClient_txt.Size = new Size(190, 23);
            pnl2WhichClient_txt.TabIndex = 5;
            // 
            // listBox2
            // 
            listBox2.BackColor = Color.Black;
            listBox2.ForeColor = Color.FromArgb(128, 255, 255);
            listBox2.FormattingEnabled = true;
            listBox2.HorizontalScrollbar = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new Point(3, 74);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(465, 349);
            listBox2.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.Controls.Add(listBox1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pnl1SendRequest_btn);
            panel1.Controls.Add(pnl1CountClient_txt);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(471, 426);
            panel1.TabIndex = 2;
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.Black;
            listBox1.ForeColor = Color.FromArgb(128, 255, 255);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(3, 74);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(465, 349);
            listBox1.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(73, 11);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 2;
            label1.Text = "Kaç Pos";
            // 
            // pnl1SendRequest_btn
            // 
            pnl1SendRequest_btn.Location = new Point(239, 29);
            pnl1SendRequest_btn.Name = "pnl1SendRequest_btn";
            pnl1SendRequest_btn.Size = new Size(141, 23);
            pnl1SendRequest_btn.TabIndex = 1;
            pnl1SendRequest_btn.Text = "İstek Gönder";
            pnl1SendRequest_btn.UseVisualStyleBackColor = true;
            pnl1SendRequest_btn.Click += pnl1SendRequest_btn_Click;
            // 
            // pnl1CountClient_txt
            // 
            pnl1CountClient_txt.Location = new Point(72, 29);
            pnl1CountClient_txt.Name = "pnl1CountClient_txt";
            pnl1CountClient_txt.Size = new Size(148, 23);
            pnl1CountClient_txt.TabIndex = 0;
            // 
            // AllDeleteQueue_btn
            // 
            AllDeleteQueue_btn.BackColor = Color.FromArgb(192, 0, 0);
            AllDeleteQueue_btn.FlatStyle = FlatStyle.Popup;
            AllDeleteQueue_btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            AllDeleteQueue_btn.ForeColor = Color.White;
            AllDeleteQueue_btn.Location = new Point(434, 16);
            AllDeleteQueue_btn.Name = "AllDeleteQueue_btn";
            AllDeleteQueue_btn.Size = new Size(115, 67);
            AllDeleteQueue_btn.TabIndex = 4;
            AllDeleteQueue_btn.Text = "Tüm Kuyrukları Sil";
            AllDeleteQueue_btn.UseVisualStyleBackColor = false;
            AllDeleteQueue_btn.Click += AllDeleteQueue_btn_Click;
            // 
            // listBox3
            // 
            listBox3.BackColor = Color.Black;
            listBox3.ForeColor = Color.FromArgb(128, 255, 255);
            listBox3.FormattingEnabled = true;
            listBox3.HorizontalScrollbar = true;
            listBox3.ItemHeight = 15;
            listBox3.Location = new Point(12, 497);
            listBox3.Name = "listBox3";
            listBox3.Size = new Size(950, 274);
            listBox3.TabIndex = 4;
            // 
            // allClientReadQueue_btn
            // 
            allClientReadQueue_btn.BackColor = Color.DarkGreen;
            allClientReadQueue_btn.FlatStyle = FlatStyle.Popup;
            allClientReadQueue_btn.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            allClientReadQueue_btn.ForeColor = Color.White;
            allClientReadQueue_btn.Location = new Point(12, 457);
            allClientReadQueue_btn.Name = "allClientReadQueue_btn";
            allClientReadQueue_btn.Size = new Size(465, 34);
            allClientReadQueue_btn.TabIndex = 5;
            allClientReadQueue_btn.Text = "Tüm Client Kuyruğunu Oku";
            allClientReadQueue_btn.UseVisualStyleBackColor = false;
            allClientReadQueue_btn.Click += AllClientReadQueue_btn_Click;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(977, 783);
            Controls.Add(allClientReadQueue_btn);
            Controls.Add(listBox3);
            Controls.Add(AllDeleteQueue_btn);
            Controls.Add(panel1);
            Controls.Add(panel2);
            MaximumSize = new Size(993, 822);
            MinimumSize = new Size(993, 822);
            Name = "Dashboard";
            Opacity = 0.9D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Client Queue";
            Load += Dashboard_Load;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel2;
        private Label label2;
        private Button pnl2ClientReply_btn;
        private TextBox pnl2WhichClient_txt;
        private ListBox listBox2;
        private Panel panel1;
        private ListBox listBox1;
        private Label label1;
        private Button pnl1SendRequest_btn;
        private TextBox pnl1CountClient_txt;
        private Button AllDeleteQueue_btn;
        private ListBox listBox3;
        private Button allClientReadQueue_btn;
    }
}