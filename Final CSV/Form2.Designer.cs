using System.Windows.Forms;

namespace GiulioSmedile_CSV
{
    partial class GraphVisualizer
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.xComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.yComboBox = new System.Windows.Forms.ComboBox();
            this.radioAbsolute = new System.Windows.Forms.RadioButton();
            this.radioRelative = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(10, 71);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(2415, 1316);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(2288, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "Generate Charts";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // xComboBox
            // 
            this.xComboBox.AllowDrop = true;
            this.xComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.xComboBox.FormattingEnabled = true;
            this.xComboBox.Location = new System.Drawing.Point(185, 29);
            this.xComboBox.Name = "xComboBox";
            this.xComboBox.Size = new System.Drawing.Size(121, 28);
            this.xComboBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Value for X coordinate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(315, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Value for Y coordinate";
            // 
            // yComboBox
            // 
            this.yComboBox.AllowDrop = true;
            this.yComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.yComboBox.FormattingEnabled = true;
            this.yComboBox.Location = new System.Drawing.Point(488, 29);
            this.yComboBox.Name = "yComboBox";
            this.yComboBox.Size = new System.Drawing.Size(121, 28);
            this.yComboBox.TabIndex = 4;
            // 
            // radioAbsolute
            // 
            this.radioAbsolute.AutoSize = true;
            this.radioAbsolute.Checked = true;
            this.radioAbsolute.Location = new System.Drawing.Point(804, 12);
            this.radioAbsolute.Name = "radioAbsolute";
            this.radioAbsolute.Size = new System.Drawing.Size(189, 24);
            this.radioAbsolute.TabIndex = 6;
            this.radioAbsolute.TabStop = true;
            this.radioAbsolute.Text = "Absolute Frequencies";
            this.radioAbsolute.UseVisualStyleBackColor = true;
            // 
            // radioRelative
            // 
            this.radioRelative.AutoSize = true;
            this.radioRelative.Location = new System.Drawing.Point(804, 41);
            this.radioRelative.Name = "radioRelative";
            this.radioRelative.Size = new System.Drawing.Size(183, 24);
            this.radioRelative.TabIndex = 7;
            this.radioRelative.Text = "Relative Frequencies";
            this.radioRelative.UseVisualStyleBackColor = true;
            // 
            // GraphVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(2437, 1399);
            this.Controls.Add(this.radioRelative);
            this.Controls.Add(this.radioAbsolute);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.yComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.xComboBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "GraphVisualizer";
            this.Text = "Graph Visualizer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox xComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox yComboBox;
        private System.Windows.Forms.RadioButton radioAbsolute;
        private System.Windows.Forms.RadioButton radioRelative;
    }
}