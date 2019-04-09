namespace Pdf_Converter
{
    partial class Main_Window
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
            this.Select_Box = new System.Windows.Forms.GroupBox();
            this.Open_File_Btn = new System.Windows.Forms.Button();
            this.Threshold_RBtn = new System.Windows.Forms.RadioButton();
            this.Nutraceutical_RBtn = new System.Windows.Forms.RadioButton();
            this.Now_RBtn = new System.Windows.Forms.RadioButton();
            this.Select_Box.SuspendLayout();
            this.SuspendLayout();
            // 
            // Select_Box
            // 
            this.Select_Box.Controls.Add(this.Open_File_Btn);
            this.Select_Box.Controls.Add(this.Threshold_RBtn);
            this.Select_Box.Controls.Add(this.Nutraceutical_RBtn);
            this.Select_Box.Controls.Add(this.Now_RBtn);
            this.Select_Box.Location = new System.Drawing.Point(79, 36);
            this.Select_Box.Name = "Select_Box";
            this.Select_Box.Size = new System.Drawing.Size(108, 138);
            this.Select_Box.TabIndex = 0;
            this.Select_Box.TabStop = false;
            this.Select_Box.Text = "Select One:";
            // 
            // Open_File_Btn
            // 
            this.Open_File_Btn.Location = new System.Drawing.Point(7, 95);
            this.Open_File_Btn.Name = "Open_File_Btn";
            this.Open_File_Btn.Size = new System.Drawing.Size(75, 23);
            this.Open_File_Btn.TabIndex = 3;
            this.Open_File_Btn.Text = "Open File";
            this.Open_File_Btn.UseVisualStyleBackColor = true;
            this.Open_File_Btn.Click += new System.EventHandler(this.Open_File_Btn_Click);
            // 
            // Threshold_RBtn
            // 
            this.Threshold_RBtn.AutoSize = true;
            this.Threshold_RBtn.Enabled = false;
            this.Threshold_RBtn.Location = new System.Drawing.Point(7, 70);
            this.Threshold_RBtn.Name = "Threshold_RBtn";
            this.Threshold_RBtn.Size = new System.Drawing.Size(80, 19);
            this.Threshold_RBtn.TabIndex = 2;
            this.Threshold_RBtn.TabStop = true;
            this.Threshold_RBtn.Text = "Threshold";
            this.Threshold_RBtn.UseVisualStyleBackColor = true;
            // 
            // Nutraceutical_RBtn
            // 
            this.Nutraceutical_RBtn.AutoSize = true;
            this.Nutraceutical_RBtn.Location = new System.Drawing.Point(7, 45);
            this.Nutraceutical_RBtn.Name = "Nutraceutical_RBtn";
            this.Nutraceutical_RBtn.Size = new System.Drawing.Size(97, 19);
            this.Nutraceutical_RBtn.TabIndex = 1;
            this.Nutraceutical_RBtn.TabStop = true;
            this.Nutraceutical_RBtn.Text = "Nutraceutical";
            this.Nutraceutical_RBtn.UseVisualStyleBackColor = true;
            // 
            // Now_RBtn
            // 
            this.Now_RBtn.AutoSize = true;
            this.Now_RBtn.Location = new System.Drawing.Point(7, 20);
            this.Now_RBtn.Name = "Now_RBtn";
            this.Now_RBtn.Size = new System.Drawing.Size(50, 19);
            this.Now_RBtn.TabIndex = 0;
            this.Now_RBtn.TabStop = true;
            this.Now_RBtn.Text = "Now";
            this.Now_RBtn.UseVisualStyleBackColor = true;
            // 
            // Main_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 233);
            this.Controls.Add(this.Select_Box);
            this.Name = "Main_Window";
            this.Text = "PDFConverter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Select_Box.ResumeLayout(false);
            this.Select_Box.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Select_Box;
        private System.Windows.Forms.RadioButton Threshold_RBtn;
        private System.Windows.Forms.RadioButton Nutraceutical_RBtn;
        private System.Windows.Forms.RadioButton Now_RBtn;
        private System.Windows.Forms.Button Open_File_Btn;
    }
}

