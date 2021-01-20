using System.Windows.Forms;

namespace Final_CSV
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.selectFileButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.changeVariableButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.variableListBox = new System.Windows.Forms.ComboBox();
            this.variableNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.filePathTextBox = new System.Windows.Forms.RichTextBox();
            this.exportClassButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.exportCSVButton = new System.Windows.Forms.Button();
            this.newFormButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(337, 797);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // selectFileButton
            // 
            this.selectFileButton.Location = new System.Drawing.Point(355, 12);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new System.Drawing.Size(123, 33);
            this.selectFileButton.TabIndex = 2;
            this.selectFileButton.Text = "Select File";
            this.selectFileButton.UseVisualStyleBackColor = true;
            this.selectFileButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.changeVariableButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.variableListBox);
            this.groupBox1.Controls.Add(this.variableNameTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(355, 642);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(444, 167);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modify Variable";
            // 
            // changeVariableButton
            // 
            this.changeVariableButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.changeVariableButton.Location = new System.Drawing.Point(139, 128);
            this.changeVariableButton.Name = "changeVariableButton";
            this.changeVariableButton.Size = new System.Drawing.Size(83, 33);
            this.changeVariableButton.TabIndex = 4;
            this.changeVariableButton.Text = "Apply";
            this.changeVariableButton.UseVisualStyleBackColor = true;
            this.changeVariableButton.Click += new System.EventHandler(this.changeVariableButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Variable Type:";
            // 
            // variableListBox
            // 
            this.variableListBox.FormattingEnabled = true;
            this.variableListBox.Items.AddRange(new object[] {
            "Boolean",
            "Int32",
            "Int64",
            "Double",
            "DateTime",
            "String"});
            this.variableListBox.Location = new System.Drawing.Point(12, 127);
            this.variableListBox.Name = "variableListBox";
            this.variableListBox.Size = new System.Drawing.Size(121, 28);
            this.variableListBox.TabIndex = 2;
            this.variableListBox.DropDownStyle = ComboBoxStyle.DropDownList;
            // 
            // variableNameTextBox
            // 
            this.variableNameTextBox.Location = new System.Drawing.Point(11, 49);
            this.variableNameTextBox.Name = "variableNameTextBox";
            this.variableNameTextBox.Size = new System.Drawing.Size(304, 26);
            this.variableNameTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Variable name:";
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePathTextBox.Enabled = false;
            this.filePathTextBox.Location = new System.Drawing.Point(484, 12);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(849, 33);
            this.filePathTextBox.TabIndex = 4;
            this.filePathTextBox.Text = "";
            // 
            // exportClassButton
            // 
            this.exportClassButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportClassButton.Location = new System.Drawing.Point(1171, 767);
            this.exportClassButton.Name = "exportClassButton";
            this.exportClassButton.Size = new System.Drawing.Size(162, 32);
            this.exportClassButton.TabIndex = 6;
            this.exportClassButton.Text = "Export Class";
            this.exportClassButton.UseVisualStyleBackColor = true;
            this.exportClassButton.Click += new System.EventHandler(this.exportClassButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(355, 51);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(978, 585);
            this.dataGridView1.TabIndex = 9;
            // 
            // exportCSVButton
            // 
            this.exportCSVButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportCSVButton.Location = new System.Drawing.Point(1003, 767);
            this.exportCSVButton.Name = "exportCSVButton";
            this.exportCSVButton.Size = new System.Drawing.Size(162, 32);
            this.exportCSVButton.TabIndex = 10;
            this.exportCSVButton.Text = "Export CSV";
            this.exportCSVButton.UseVisualStyleBackColor = true;
            this.exportCSVButton.Click += new System.EventHandler(this.exportCSVButton_Click);
            // 
            // newFormButton
            // 
            this.newFormButton.Location = new System.Drawing.Point(809, 765);
            this.newFormButton.Name = "newFormButton";
            this.newFormButton.Size = new System.Drawing.Size(124, 33);
            this.newFormButton.TabIndex = 11;
            this.newFormButton.Text = "Visualize Data";
            this.newFormButton.UseVisualStyleBackColor = true;
            this.newFormButton.Click += new System.EventHandler(this.newFormButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1345, 821);
            this.Controls.Add(this.newFormButton);
            this.Controls.Add(this.exportCSVButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.exportClassButton);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.selectFileButton);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.Text = "CSV Visualizer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button selectFileButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox variableListBox;
        private System.Windows.Forms.TextBox variableNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button changeVariableButton;
        private System.Windows.Forms.RichTextBox filePathTextBox;
        private System.Windows.Forms.Button exportClassButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button exportCSVButton;
        private System.Windows.Forms.Button newFormButton;
    }
}

