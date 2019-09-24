namespace Annex.forms
{
    partial class ServerSelectorForm
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
            this.serverDropdown = new System.Windows.Forms.ComboBox();
            this.startBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serverDropdown
            // 
            this.serverDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverDropdown.FormattingEnabled = true;
            this.serverDropdown.Items.AddRange(new object[] {
            "Offline",
            "Bancho",
            "Ripple"});
            this.serverDropdown.Location = new System.Drawing.Point(12, 12);
            this.serverDropdown.Name = "serverDropdown";
            this.serverDropdown.Size = new System.Drawing.Size(242, 21);
            this.serverDropdown.TabIndex = 0;
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(12, 39);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(242, 24);
            this.startBtn.TabIndex = 1;
            this.startBtn.Text = "Start Game";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Lucida Console", 6F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 8);
            this.label1.TabIndex = 2;
            this.label1.Text = "*This will hide your session token";
            // 
            // ServerSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 81);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.serverDropdown);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "ServerSelectorForm";
            this.ShowIcon = false;
            this.Text = "Server Selector";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ServerSelectorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox serverDropdown;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Label label1;
    }
}

