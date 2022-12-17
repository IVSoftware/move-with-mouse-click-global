namespace move_with_mouse_click
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.checkBoxEnableCTM = new System.Windows.Forms.CheckBox();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // checkBoxEnableCTM
            // 
            this.checkBoxEnableCTM.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEnableCTM.AutoSize = true;
            this.checkBoxEnableCTM.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.checkBoxEnableCTM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEnableCTM.Location = new System.Drawing.Point(12, 26);
            this.checkBoxEnableCTM.Name = "checkBoxEnableCTM";
            this.checkBoxEnableCTM.Size = new System.Drawing.Size(187, 35);
            this.checkBoxEnableCTM.TabIndex = 0;
            this.checkBoxEnableCTM.Text = "Enable Click to Move";
            this.checkBoxEnableCTM.UseVisualStyleBackColor = true;
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.Location = new System.Drawing.Point(228, 26);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(335, 211);
            this.richTextBox.TabIndex = 1;
            this.richTextBox.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 244);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.checkBoxEnableCTM);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Click to Move";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox checkBoxEnableCTM;
        private RichTextBox richTextBox;
    }
}