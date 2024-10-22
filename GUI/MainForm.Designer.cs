namespace GUI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.comboBoxZones = new System.Windows.Forms.ComboBox();
            this.btnDatmay = new System.Windows.Forms.Button();
            this.flpDatmay = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lsbOrders = new System.Windows.Forms.ListView();
            this.btnThanhtoan = new System.Windows.Forms.Button();
            this.lblServiceInfo = new System.Windows.Forms.Label();
            this.lblMachineInfo = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lsbDichvu = new System.Windows.Forms.ListBox();
            this.btnOrder = new System.Windows.Forms.Button();
            this.flpService = new System.Windows.Forms.FlowLayoutPanel();
            this.qLCGDataSet4 = new GUI.QLCGDataSet4();
            this.ordersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ordersTableAdapter = new GUI.QLCGDataSet4TableAdapters.OrdersTableAdapter();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qLCGDataSet4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ordersBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxZones
            // 
            this.comboBoxZones.FormattingEnabled = true;
            this.comboBoxZones.Location = new System.Drawing.Point(53, 21);
            this.comboBoxZones.Name = "comboBoxZones";
            this.comboBoxZones.Size = new System.Drawing.Size(181, 21);
            this.comboBoxZones.TabIndex = 3;
            this.comboBoxZones.SelectionChangeCommitted += new System.EventHandler(this.comboBoxZones_SelectedIndexChanged);
            // 
            // btnDatmay
            // 
            this.btnDatmay.Location = new System.Drawing.Point(381, 3);
            this.btnDatmay.Name = "btnDatmay";
            this.btnDatmay.Size = new System.Drawing.Size(75, 23);
            this.btnDatmay.TabIndex = 4;
            this.btnDatmay.Text = "Đặt máy";
            this.btnDatmay.UseVisualStyleBackColor = true;
            this.btnDatmay.Click += new System.EventHandler(this.btnDatmay_Click);
            // 
            // flpDatmay
            // 
            this.flpDatmay.AllowDrop = true;
            this.flpDatmay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpDatmay.Location = new System.Drawing.Point(6, 59);
            this.flpDatmay.Name = "flpDatmay";
            this.flpDatmay.Size = new System.Drawing.Size(421, 245);
            this.flpDatmay.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, -2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(799, 371);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lsbOrders);
            this.tabPage1.Controls.Add(this.btnThanhtoan);
            this.tabPage1.Controls.Add(this.lblServiceInfo);
            this.tabPage1.Controls.Add(this.lblMachineInfo);
            this.tabPage1.Controls.Add(this.flpDatmay);
            this.tabPage1.Controls.Add(this.comboBoxZones);
            this.tabPage1.Controls.Add(this.btnDatmay);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(791, 345);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Trang chủ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lsbOrders
            // 
            this.lsbOrders.HideSelection = false;
            this.lsbOrders.Location = new System.Drawing.Point(449, 149);
            this.lsbOrders.Name = "lsbOrders";
            this.lsbOrders.Size = new System.Drawing.Size(333, 119);
            this.lsbOrders.TabIndex = 9;
            this.lsbOrders.UseCompatibleStateImageBehavior = false;
            // 
            // btnThanhtoan
            // 
            this.btnThanhtoan.Location = new System.Drawing.Point(668, 287);
            this.btnThanhtoan.Name = "btnThanhtoan";
            this.btnThanhtoan.Size = new System.Drawing.Size(105, 55);
            this.btnThanhtoan.TabIndex = 8;
            this.btnThanhtoan.Text = "Thanh toán";
            this.btnThanhtoan.UseVisualStyleBackColor = true;
            this.btnThanhtoan.Click += new System.EventHandler(this.btnThanhtoan_Click);
            // 
            // lblServiceInfo
            // 
            this.lblServiceInfo.AutoSize = true;
            this.lblServiceInfo.Location = new System.Drawing.Point(558, 222);
            this.lblServiceInfo.Name = "lblServiceInfo";
            this.lblServiceInfo.Size = new System.Drawing.Size(0, 13);
            this.lblServiceInfo.TabIndex = 7;
            // 
            // lblMachineInfo
            // 
            this.lblMachineInfo.AutoSize = true;
            this.lblMachineInfo.Location = new System.Drawing.Point(598, 36);
            this.lblMachineInfo.Name = "lblMachineInfo";
            this.lblMachineInfo.Size = new System.Drawing.Size(0, 13);
            this.lblMachineInfo.TabIndex = 6;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblTotalAmount);
            this.tabPage2.Controls.Add(this.lsbDichvu);
            this.tabPage2.Controls.Add(this.btnOrder);
            this.tabPage2.Controls.Add(this.flpService);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(791, 345);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Dịch vụ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Location = new System.Drawing.Point(105, 145);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(0, 13);
            this.lblTotalAmount.TabIndex = 6;
            // 
            // lsbDichvu
            // 
            this.lsbDichvu.FormattingEnabled = true;
            this.lsbDichvu.Location = new System.Drawing.Point(6, 6);
            this.lsbDichvu.Name = "lsbDichvu";
            this.lsbDichvu.Size = new System.Drawing.Size(191, 121);
            this.lsbDichvu.TabIndex = 5;
            // 
            // btnOrder
            // 
            this.btnOrder.Location = new System.Drawing.Point(6, 201);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(75, 23);
            this.btnOrder.TabIndex = 1;
            this.btnOrder.Text = "Đặt";
            this.btnOrder.UseVisualStyleBackColor = true;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // flpService
            // 
            this.flpService.AllowDrop = true;
            this.flpService.AutoScroll = true;
            this.flpService.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpService.Location = new System.Drawing.Point(241, 3);
            this.flpService.Name = "flpService";
            this.flpService.Size = new System.Drawing.Size(550, 313);
            this.flpService.TabIndex = 0;
            // 
            // qLCGDataSet4
            // 
            this.qLCGDataSet4.DataSetName = "QLCGDataSet4";
            this.qLCGDataSet4.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ordersBindingSource
            // 
            this.ordersBindingSource.DataMember = "Orders";
            this.ordersBindingSource.DataSource = this.qLCGDataSet4;
            // 
            // ordersTableAdapter
            // 
            this.ordersTableAdapter.ClearBeforeFill = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qLCGDataSet4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ordersBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxZones;
        private System.Windows.Forms.Button btnDatmay;
        private System.Windows.Forms.FlowLayoutPanel flpDatmay;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnOrder;
        private System.Windows.Forms.FlowLayoutPanel flpService;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.ListBox lsbDichvu;
        private System.Windows.Forms.Label lblMachineInfo;
        private System.Windows.Forms.Label lblServiceInfo;
        private System.Windows.Forms.Button btnThanhtoan;
        private QLCGDataSet4 qLCGDataSet4;
        private System.Windows.Forms.BindingSource ordersBindingSource;
        private QLCGDataSet4TableAdapters.OrdersTableAdapter ordersTableAdapter;
        private System.Windows.Forms.ListView lsbOrders;
    }
}