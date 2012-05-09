namespace SerialPortBridge
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSerialPort1 = new System.Windows.Forms.ComboBox();
            this.cmbSerialPort2 = new System.Windows.Forms.ComboBox();
            this.btnEnableBridge = new System.Windows.Forms.Button();
            this.btnDisableBridge = new System.Windows.Forms.Button();
            this.chkMonitorValues = new System.Windows.Forms.CheckBox();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBaudrate = new System.Windows.Forms.TextBox();
            this.cmbParity = new System.Windows.Forms.ComboBox();
            this.cmbDatabits = new System.Windows.Forms.ComboBox();
            this.cmbStopbits = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbFlowcontrol = new System.Windows.Forms.ComboBox();
            this.grpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port 2";
            // 
            // cmbSerialPort1
            // 
            this.cmbSerialPort1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSerialPort1.Location = new System.Drawing.Point(50, 12);
            this.cmbSerialPort1.Name = "cmbSerialPort1";
            this.cmbSerialPort1.Size = new System.Drawing.Size(121, 21);
            this.cmbSerialPort1.TabIndex = 4;
            // 
            // cmbSerialPort2
            // 
            this.cmbSerialPort2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSerialPort2.Location = new System.Drawing.Point(50, 39);
            this.cmbSerialPort2.Name = "cmbSerialPort2";
            this.cmbSerialPort2.Size = new System.Drawing.Size(121, 21);
            this.cmbSerialPort2.TabIndex = 5;
            // 
            // btnEnableBridge
            // 
            this.btnEnableBridge.Location = new System.Drawing.Point(50, 66);
            this.btnEnableBridge.Name = "btnEnableBridge";
            this.btnEnableBridge.Size = new System.Drawing.Size(121, 23);
            this.btnEnableBridge.TabIndex = 6;
            this.btnEnableBridge.Text = "Enable bridge";
            this.btnEnableBridge.UseVisualStyleBackColor = true;
            this.btnEnableBridge.Click += new System.EventHandler(this.btnEnableBridge_Click);
            // 
            // btnDisableBridge
            // 
            this.btnDisableBridge.Enabled = false;
            this.btnDisableBridge.Location = new System.Drawing.Point(50, 95);
            this.btnDisableBridge.Name = "btnDisableBridge";
            this.btnDisableBridge.Size = new System.Drawing.Size(121, 23);
            this.btnDisableBridge.TabIndex = 7;
            this.btnDisableBridge.Text = "Disable bridge";
            this.btnDisableBridge.UseVisualStyleBackColor = true;
            this.btnDisableBridge.Click += new System.EventHandler(this.btnDisableBridge_Click);
            // 
            // chkMonitorValues
            // 
            this.chkMonitorValues.AutoSize = true;
            this.chkMonitorValues.Location = new System.Drawing.Point(50, 124);
            this.chkMonitorValues.Name = "chkMonitorValues";
            this.chkMonitorValues.Size = new System.Drawing.Size(95, 17);
            this.chkMonitorValues.TabIndex = 8;
            this.chkMonitorValues.Text = "Monitor values";
            this.chkMonitorValues.UseVisualStyleBackColor = true;
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.label7);
            this.grpSettings.Controls.Add(this.label6);
            this.grpSettings.Controls.Add(this.label5);
            this.grpSettings.Controls.Add(this.label4);
            this.grpSettings.Controls.Add(this.cmbFlowcontrol);
            this.grpSettings.Controls.Add(this.cmbStopbits);
            this.grpSettings.Controls.Add(this.cmbDatabits);
            this.grpSettings.Controls.Add(this.cmbParity);
            this.grpSettings.Controls.Add(this.txtBaudrate);
            this.grpSettings.Controls.Add(this.label3);
            this.grpSettings.Location = new System.Drawing.Point(202, 12);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(183, 158);
            this.grpSettings.TabIndex = 9;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Baudrate";
            // 
            // txtBaudrate
            // 
            this.txtBaudrate.Location = new System.Drawing.Point(74, 19);
            this.txtBaudrate.Name = "txtBaudrate";
            this.txtBaudrate.Size = new System.Drawing.Size(103, 20);
            this.txtBaudrate.TabIndex = 1;
            this.txtBaudrate.Text = "9600";
            // 
            // cmbParity
            // 
            this.cmbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParity.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd",
            "Mark",
            "Space"});
            this.cmbParity.Location = new System.Drawing.Point(74, 72);
            this.cmbParity.Name = "cmbParity";
            this.cmbParity.Size = new System.Drawing.Size(103, 21);
            this.cmbParity.TabIndex = 2;
            // 
            // cmbDatabits
            // 
            this.cmbDatabits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabits.Items.AddRange(new object[] {
            "7",
            "8",
            "9"});
            this.cmbDatabits.Location = new System.Drawing.Point(74, 45);
            this.cmbDatabits.Name = "cmbDatabits";
            this.cmbDatabits.Size = new System.Drawing.Size(103, 21);
            this.cmbDatabits.TabIndex = 3;
            // 
            // cmbStopbits
            // 
            this.cmbStopbits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStopbits.Items.AddRange(new object[] {
            "None",
            "1",
            "1.5",
            "2"});
            this.cmbStopbits.Location = new System.Drawing.Point(74, 99);
            this.cmbStopbits.Name = "cmbStopbits";
            this.cmbStopbits.Size = new System.Drawing.Size(103, 21);
            this.cmbStopbits.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Parity";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Databits";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Stopbits";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Flowcontrol";
            // 
            // cmbFlowcontrol
            // 
            this.cmbFlowcontrol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFlowcontrol.Enabled = false;
            this.cmbFlowcontrol.Items.AddRange(new object[] {
            "None",
            "XON/XOFF",
            "RTS/CTS",
            "DSR/DTR"});
            this.cmbFlowcontrol.Location = new System.Drawing.Point(74, 126);
            this.cmbFlowcontrol.Name = "cmbFlowcontrol";
            this.cmbFlowcontrol.Size = new System.Drawing.Size(103, 21);
            this.cmbFlowcontrol.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 182);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.chkMonitorValues);
            this.Controls.Add(this.btnDisableBridge);
            this.Controls.Add(this.btnEnableBridge);
            this.Controls.Add(this.cmbSerialPort2);
            this.Controls.Add(this.cmbSerialPort1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Serial port bridger";
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSerialPort1;
        private System.Windows.Forms.ComboBox cmbSerialPort2;
        private System.Windows.Forms.Button btnEnableBridge;
        private System.Windows.Forms.Button btnDisableBridge;
        private System.Windows.Forms.CheckBox chkMonitorValues;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.ComboBox cmbParity;
        private System.Windows.Forms.TextBox txtBaudrate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbStopbits;
        private System.Windows.Forms.ComboBox cmbDatabits;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbFlowcontrol;
    }
}

