namespace AnAddin
{
    partial class TaskpaneUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Message = new System.Windows.Forms.Label();
            this.CommitButton = new System.Windows.Forms.Button();
            this.PastCommits = new System.Windows.Forms.Label();
            this.NewCommits = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(25, 278);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(376, 148);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Write a short description of your commit.";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.BackColor = System.Drawing.SystemColors.Control;
            this.Message.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Message.Location = new System.Drawing.Point(21, 255);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(77, 20);
            this.Message.TabIndex = 1;
            this.Message.Text = "Message";
            this.Message.Click += new System.EventHandler(this.label1_Click);
            // 
            // CommitButton
            // 
            this.CommitButton.Location = new System.Drawing.Point(260, 452);
            this.CommitButton.Name = "CommitButton";
            this.CommitButton.Size = new System.Drawing.Size(141, 24);
            this.CommitButton.TabIndex = 2;
            this.CommitButton.Text = "Commit";
            this.CommitButton.UseVisualStyleBackColor = true;
            this.CommitButton.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // PastCommits
            // 
            this.PastCommits.AutoSize = true;
            this.PastCommits.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.PastCommits.Location = new System.Drawing.Point(19, 18);
            this.PastCommits.Name = "PastCommits";
            this.PastCommits.Size = new System.Drawing.Size(161, 29);
            this.PastCommits.TabIndex = 3;
            this.PastCommits.Text = "Past Commits";
            // 
            // NewCommits
            // 
            this.NewCommits.AutoSize = true;
            this.NewCommits.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.NewCommits.Location = new System.Drawing.Point(19, 210);
            this.NewCommits.Name = "NewCommits";
            this.NewCommits.Size = new System.Drawing.Size(152, 29);
            this.NewCommits.TabIndex = 4;
            this.NewCommits.Text = "New Commit";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(25, 61);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(376, 121);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // TaskpaneUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.NewCommits);
            this.Controls.Add(this.PastCommits);
            this.Controls.Add(this.CommitButton);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TaskpaneUI";
            this.Size = new System.Drawing.Size(445, 557);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.Button CommitButton;
        private System.Windows.Forms.Label PastCommits;
        private System.Windows.Forms.Label NewCommits;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}
