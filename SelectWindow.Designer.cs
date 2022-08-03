namespace Client_Management_2._1
{
    partial class SelectWindow
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
            this.checkBoxName = new System.Windows.Forms.CheckBox();
            this.checkBoxPhone = new System.Windows.Forms.CheckBox();
            this.checkBoxVin = new System.Windows.Forms.CheckBox();
            this.checkBoxModel = new System.Windows.Forms.CheckBox();
            this.checkBoxEngine = new System.Windows.Forms.CheckBox();
            this.checkBoxPlate = new System.Windows.Forms.CheckBox();
            this.BtnExtract = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // checkBoxName
            // 
            this.checkBoxName.AutoSize = true;
            this.checkBoxName.Location = new System.Drawing.Point(12, 22);
            this.checkBoxName.Name = "checkBoxName";
            this.checkBoxName.Size = new System.Drawing.Size(54, 17);
            this.checkBoxName.TabIndex = 0;
            this.checkBoxName.Text = "Name";
            this.checkBoxName.UseVisualStyleBackColor = true;
            this.checkBoxName.Click += new System.EventHandler(this.checkBoxName_Click);
            // 
            // checkBoxPhone
            // 
            this.checkBoxPhone.AutoSize = true;
            this.checkBoxPhone.Location = new System.Drawing.Point(12, 45);
            this.checkBoxPhone.Name = "checkBoxPhone";
            this.checkBoxPhone.Size = new System.Drawing.Size(57, 17);
            this.checkBoxPhone.TabIndex = 1;
            this.checkBoxPhone.Text = "Phone";
            this.checkBoxPhone.UseVisualStyleBackColor = true;
            this.checkBoxPhone.Click += new System.EventHandler(this.checkBoxPhone_Click);
            // 
            // checkBoxVin
            // 
            this.checkBoxVin.AutoSize = true;
            this.checkBoxVin.Location = new System.Drawing.Point(12, 91);
            this.checkBoxVin.Name = "checkBoxVin";
            this.checkBoxVin.Size = new System.Drawing.Size(41, 17);
            this.checkBoxVin.TabIndex = 2;
            this.checkBoxVin.Text = "Vin";
            this.checkBoxVin.UseVisualStyleBackColor = true;
            this.checkBoxVin.Click += new System.EventHandler(this.checkBoxVin_Click);
            // 
            // checkBoxModel
            // 
            this.checkBoxModel.AutoSize = true;
            this.checkBoxModel.Location = new System.Drawing.Point(12, 68);
            this.checkBoxModel.Name = "checkBoxModel";
            this.checkBoxModel.Size = new System.Drawing.Size(55, 17);
            this.checkBoxModel.TabIndex = 3;
            this.checkBoxModel.Text = "Model";
            this.checkBoxModel.UseVisualStyleBackColor = true;
            this.checkBoxModel.Click += new System.EventHandler(this.checkBoxModel_Click);
            // 
            // checkBoxEngine
            // 
            this.checkBoxEngine.AutoSize = true;
            this.checkBoxEngine.Location = new System.Drawing.Point(12, 114);
            this.checkBoxEngine.Name = "checkBoxEngine";
            this.checkBoxEngine.Size = new System.Drawing.Size(59, 17);
            this.checkBoxEngine.TabIndex = 4;
            this.checkBoxEngine.Text = "Engine";
            this.checkBoxEngine.UseVisualStyleBackColor = true;
            this.checkBoxEngine.Click += new System.EventHandler(this.checkBoxEngine_Click);
            // 
            // checkBoxPlate
            // 
            this.checkBoxPlate.AutoSize = true;
            this.checkBoxPlate.Location = new System.Drawing.Point(12, 137);
            this.checkBoxPlate.Name = "checkBoxPlate";
            this.checkBoxPlate.Size = new System.Drawing.Size(50, 17);
            this.checkBoxPlate.TabIndex = 5;
            this.checkBoxPlate.Text = "Plate";
            this.checkBoxPlate.UseVisualStyleBackColor = true;
            this.checkBoxPlate.Click += new System.EventHandler(this.checkBoxPlate_Click);
            // 
            // BtnExtract
            // 
            this.BtnExtract.Location = new System.Drawing.Point(122, 176);
            this.BtnExtract.Name = "BtnExtract";
            this.BtnExtract.Size = new System.Drawing.Size(127, 23);
            this.BtnExtract.TabIndex = 6;
            this.BtnExtract.Text = "Extract to Excell";
            this.BtnExtract.UseVisualStyleBackColor = true;
            this.BtnExtract.Click += new System.EventHandler(this.BtnExtract_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(159, 23);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(162, 134);
            this.listBox1.TabIndex = 7;
            // 
            // SelectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 211);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.BtnExtract);
            this.Controls.Add(this.checkBoxPlate);
            this.Controls.Add(this.checkBoxEngine);
            this.Controls.Add(this.checkBoxModel);
            this.Controls.Add(this.checkBoxVin);
            this.Controls.Add(this.checkBoxPhone);
            this.Controls.Add(this.checkBoxName);
            this.Name = "SelectWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SelectWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxName;
        private System.Windows.Forms.CheckBox checkBoxPhone;
        private System.Windows.Forms.CheckBox checkBoxVin;
        private System.Windows.Forms.CheckBox checkBoxModel;
        private System.Windows.Forms.CheckBox checkBoxEngine;
        private System.Windows.Forms.CheckBox checkBoxPlate;
        private System.Windows.Forms.Button BtnExtract;
        private System.Windows.Forms.ListBox listBox1;
    }
}