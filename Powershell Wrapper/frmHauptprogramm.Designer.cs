﻿namespace Powershell_Wrapper
{
    partial class frmHauptprogramm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.cc
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            //nnnc
            base.Dispose(disposing);
            //sssscssscc
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHauptprogramm));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.cmdConvert = new System.Windows.Forms.Button();
            this.toJavascriptCheck = new System.Windows.Forms.CheckBox();
            this.toBadUSBCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.AllowDrop = true;
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(4, 4);
            this.listBox1.MultiColumn = true;
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(444, 82);
            this.listBox1.TabIndex = 0;
            this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
            this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            // 
            // cmdConvert
            // 
            this.cmdConvert.Location = new System.Drawing.Point(4, 106);
            this.cmdConvert.Name = "cmdConvert";
            this.cmdConvert.Size = new System.Drawing.Size(448, 23);
            this.cmdConvert.TabIndex = 1;
            this.cmdConvert.Text = "Convert for Flipper";
            this.cmdConvert.UseVisualStyleBackColor = true;
            this.cmdConvert.Click += new System.EventHandler(this.cmdConvert_Click);
            // 
            // toJavascriptCheck
            // 
            this.toJavascriptCheck.AutoSize = true;
            this.toJavascriptCheck.Location = new System.Drawing.Point(4, 88);
            this.toJavascriptCheck.Name = "toJavascriptCheck";
            this.toJavascriptCheck.Size = new System.Drawing.Size(90, 17);
            this.toJavascriptCheck.TabIndex = 2;
            this.toJavascriptCheck.Text = "To Javascript";
            this.toJavascriptCheck.UseVisualStyleBackColor = true;
            this.toJavascriptCheck.CheckStateChanged += new System.EventHandler(this.toJavascriptCheck_CheckStateChanged);
            // 
            // toBadUSBCheck
            // 
            this.toBadUSBCheck.AutoSize = true;
            this.toBadUSBCheck.Location = new System.Drawing.Point(91, 88);
            this.toBadUSBCheck.Name = "toBadUSBCheck";
            this.toBadUSBCheck.Size = new System.Drawing.Size(83, 17);
            this.toBadUSBCheck.TabIndex = 3;
            this.toBadUSBCheck.Text = "To BadUSB";
            this.toBadUSBCheck.UseVisualStyleBackColor = true;
            this.toBadUSBCheck.CheckStateChanged += new System.EventHandler(this.toBadUSBCheck_CheckStateChanged);
            // 
            // frmHauptprogramm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 132);
            this.Controls.Add(this.toBadUSBCheck);
            this.Controls.Add(this.toJavascriptCheck);
            this.Controls.Add(this.cmdConvert);
            this.Controls.Add(this.listBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(470, 171);
            this.MinimumSize = new System.Drawing.Size(470, 171);
            this.Name = "frmHauptprogramm";
            this.Text = "Flipper BadUSB Wrapper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button cmdConvert;
        private System.Windows.Forms.CheckBox toJavascriptCheck;
        private System.Windows.Forms.CheckBox toBadUSBCheck;
    }
}

