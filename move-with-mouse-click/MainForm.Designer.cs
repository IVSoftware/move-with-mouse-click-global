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
            this.buttonClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxEnableDragging = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxEnableCTM
            // 
            this.checkBoxEnableCTM.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEnableCTM.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.checkBoxEnableCTM.FlatAppearance.BorderSize = 2;
            this.checkBoxEnableCTM.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.checkBoxEnableCTM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEnableCTM.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.checkBoxEnableCTM.Location = new System.Drawing.Point(458, 10);
            this.checkBoxEnableCTM.Name = "checkBoxEnableCTM";
            this.checkBoxEnableCTM.Size = new System.Drawing.Size(50, 50);
            this.checkBoxEnableCTM.TabIndex = 0;
            this.checkBoxEnableCTM.UseVisualStyleBackColor = true;
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.Location = new System.Drawing.Point(12, 67);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(554, 186);
            this.richTextBox.TabIndex = 1;
            this.richTextBox.Text = "";
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.buttonClose.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonClose.Location = new System.Drawing.Point(514, 10);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(50, 50);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enable Click to Move ->";
            // 
            // checkBoxEnableDragging
            // 
            this.checkBoxEnableDragging.AutoSize = true;
            this.checkBoxEnableDragging.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.checkBoxEnableDragging.Location = new System.Drawing.Point(12, 259);
            this.checkBoxEnableDragging.Name = "checkBoxEnableDragging";
            this.checkBoxEnableDragging.Size = new System.Drawing.Size(121, 29);
            this.checkBoxEnableDragging.TabIndex = 4;
            this.checkBoxEnableDragging.Text = "Draggable";
            this.checkBoxEnableDragging.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 300);
            this.Controls.Add(this.checkBoxEnableDragging);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.checkBoxEnableCTM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Click to Move";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox checkBoxEnableCTM;
        private RichTextBox richTextBox;
        private Button buttonClose;
        private Label label1;
        private CheckBox checkBoxEnableDragging;
    }
}