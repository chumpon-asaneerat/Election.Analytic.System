namespace GIS
{
    partial class Form1
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
            this.label3 = new System.Windows.Forms.Label();
            this.nudTolerance = new System.Windows.Forms.NumericUpDown();
            this.lblSimplified = new System.Windows.Forms.Label();
            this.lblOriginal = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudTolerance)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(89, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Tolerance";
            // 
            // nudTolerance
            // 
            this.nudTolerance.DecimalPlaces = 2;
            this.nudTolerance.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudTolerance.Location = new System.Drawing.Point(153, 47);
            this.nudTolerance.Name = "nudTolerance";
            this.nudTolerance.Size = new System.Drawing.Size(62, 20);
            this.nudTolerance.TabIndex = 10;
            this.nudTolerance.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblSimplified
            // 
            this.lblSimplified.AutoSize = true;
            this.lblSimplified.Location = new System.Drawing.Point(152, 31);
            this.lblSimplified.Name = "lblSimplified";
            this.lblSimplified.Size = new System.Drawing.Size(0, 13);
            this.lblSimplified.TabIndex = 9;
            // 
            // lblOriginal
            // 
            this.lblOriginal.AutoSize = true;
            this.lblOriginal.Location = new System.Drawing.Point(150, 9);
            this.lblOriginal.Name = "lblOriginal";
            this.lblOriginal.Size = new System.Drawing.Size(0, 13);
            this.lblOriginal.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Number of points Simplified";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Number of points Originally";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 619);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudTolerance);
            this.Controls.Add(this.lblSimplified);
            this.Controls.Add(this.lblOriginal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.nudTolerance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudTolerance;
        private System.Windows.Forms.Label lblSimplified;
        private System.Windows.Forms.Label lblOriginal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

