namespace qr_code_to_href
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_selectFile = new System.Windows.Forms.Button();
            this.textBox_selectedFile = new System.Windows.Forms.TextBox();
            this.button_proceed = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_selectFile
            // 
            this.button_selectFile.Location = new System.Drawing.Point(472, 12);
            this.button_selectFile.Name = "button_selectFile";
            this.button_selectFile.Size = new System.Drawing.Size(75, 23);
            this.button_selectFile.TabIndex = 0;
            this.button_selectFile.Text = "Select File";
            this.button_selectFile.UseVisualStyleBackColor = true;
            this.button_selectFile.Click += new System.EventHandler(this.button_selectFile_Click);
            // 
            // textBox_selectedFile
            // 
            this.textBox_selectedFile.Location = new System.Drawing.Point(12, 14);
            this.textBox_selectedFile.Name = "textBox_selectedFile";
            this.textBox_selectedFile.Size = new System.Drawing.Size(454, 20);
            this.textBox_selectedFile.TabIndex = 1;
            // 
            // button_proceed
            // 
            this.button_proceed.Location = new System.Drawing.Point(472, 64);
            this.button_proceed.Name = "button_proceed";
            this.button_proceed.Size = new System.Drawing.Size(75, 23);
            this.button_proceed.TabIndex = 2;
            this.button_proceed.Text = "Proceed";
            this.button_proceed.UseVisualStyleBackColor = true;
            this.button_proceed.Click += new System.EventHandler(this.button_proceed_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 99);
            this.Controls.Add(this.button_proceed);
            this.Controls.Add(this.textBox_selectedFile);
            this.Controls.Add(this.button_selectFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_selectFile;
        private System.Windows.Forms.TextBox textBox_selectedFile;
        private System.Windows.Forms.Button button_proceed;
    }
}

