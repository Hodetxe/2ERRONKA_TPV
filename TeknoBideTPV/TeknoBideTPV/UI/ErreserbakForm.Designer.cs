namespace TeknoBideTPV.UI
{
    partial class ErreserbakForm
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
            dgv_ErreserbakIkusi = new DataGridView();
            lbl_Bilatu = new Label();
            txt_Bilatu = new TextBox();
            lbl_Eguna = new Label();
            dtp_Eguna = new DateTimePicker();
            btn_Bilatu = new Button();
            btn_Garbitu = new Button();
            pnl_Filtroak = new Panel();
            headerControl_Erreserbak = new TeknoBideTPV.UI.Controls.HeaderControl();
            footerControl_Erreserbak = new TeknoBideTPV.UI.Controls.FooterControl();
            ((System.ComponentModel.ISupportInitialize)dgv_ErreserbakIkusi).BeginInit();
            pnl_Filtroak.SuspendLayout();
            SuspendLayout();
            // 
            // dgv_ErreserbakIkusi
            // 
            dgv_ErreserbakIkusi.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_ErreserbakIkusi.Dock = DockStyle.Fill;
            dgv_ErreserbakIkusi.Location = new Point(0, 249);
            dgv_ErreserbakIkusi.Name = "dgv_ErreserbakIkusi";
            dgv_ErreserbakIkusi.RowHeadersWidth = 51;
            dgv_ErreserbakIkusi.Size = new Size(1541, 496);
            dgv_ErreserbakIkusi.TabIndex = 0;
            // 
            // lbl_Bilatu
            // 
            lbl_Bilatu.AutoSize = true;
            lbl_Bilatu.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Bilatu.Location = new Point(53, 44);
            lbl_Bilatu.Name = "lbl_Bilatu";
            lbl_Bilatu.Size = new Size(92, 38);
            lbl_Bilatu.TabIndex = 3;
            lbl_Bilatu.Text = "Bilatu:";
            // 
            // txt_Bilatu
            // 
            txt_Bilatu.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_Bilatu.Location = new Point(165, 41);
            txt_Bilatu.Name = "txt_Bilatu";
            txt_Bilatu.Size = new Size(300, 43);
            txt_Bilatu.TabIndex = 4;
            // 
            // lbl_Eguna
            // 
            lbl_Eguna.AutoSize = true;
            lbl_Eguna.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Eguna.Location = new Point(470, 39);
            lbl_Eguna.Name = "lbl_Eguna";
            lbl_Eguna.Size = new Size(99, 38);
            lbl_Eguna.TabIndex = 5;
            lbl_Eguna.Text = "Eguna:";
            // 
            // dtp_Eguna
            // 
            dtp_Eguna.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtp_Eguna.Format = DateTimePickerFormat.Short;
            dtp_Eguna.Location = new Point(575, 39);
            dtp_Eguna.Name = "dtp_Eguna";
            dtp_Eguna.ShowCheckBox = true;
            dtp_Eguna.Size = new Size(220, 43);
            dtp_Eguna.TabIndex = 6;
            // 
            // btn_Bilatu
            // 
            btn_Bilatu.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_Bilatu.Location = new Point(801, 36);
            btn_Bilatu.Name = "btn_Bilatu";
            btn_Bilatu.Size = new Size(155, 45);
            btn_Bilatu.TabIndex = 7;
            btn_Bilatu.Text = "Bilatu";
            btn_Bilatu.UseVisualStyleBackColor = true;
            btn_Bilatu.Click += btn_Bilatu_Click;
            // 
            // btn_Garbitu
            // 
            btn_Garbitu.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_Garbitu.Location = new Point(1639, 32);
            btn_Garbitu.Name = "btn_Garbitu";
            btn_Garbitu.Size = new Size(226, 51);
            btn_Garbitu.TabIndex = 8;
            btn_Garbitu.Text = "Filtroak Garbitu";
            btn_Garbitu.UseVisualStyleBackColor = true;
            btn_Garbitu.Click += btn_Garbitu_Click;
            // 
            // pnl_Filtroak
            // 
            pnl_Filtroak.Controls.Add(btn_Garbitu);
            pnl_Filtroak.Controls.Add(btn_Bilatu);
            pnl_Filtroak.Controls.Add(lbl_Bilatu);
            pnl_Filtroak.Controls.Add(txt_Bilatu);
            pnl_Filtroak.Controls.Add(lbl_Eguna);
            pnl_Filtroak.Controls.Add(dtp_Eguna);
            pnl_Filtroak.Dock = DockStyle.Top;
            pnl_Filtroak.Location = new Point(0, 105);
            pnl_Filtroak.Name = "pnl_Filtroak";
            pnl_Filtroak.Size = new Size(1541, 144);
            pnl_Filtroak.TabIndex = 10;
            // 
            // headerControl_Erreserbak
            // 
            headerControl_Erreserbak.BackColor = Color.FromArgb(210, 210, 210);
            headerControl_Erreserbak.DataOrdua = "Data - Ordua";
            headerControl_Erreserbak.Dock = DockStyle.Top;
            headerControl_Erreserbak.Erabiltzailea = "Erabiltzailea";
            headerControl_Erreserbak.Izena = "TXAPELA";
            headerControl_Erreserbak.Location = new Point(0, 0);
            headerControl_Erreserbak.Name = "headerControl_Erreserbak";
            headerControl_Erreserbak.Size = new Size(1541, 105);
            headerControl_Erreserbak.TabIndex = 11;
            headerControl_Erreserbak.Titulo = "IZENBURUA";
            // 
            // footerControl_Erreserbak
            // 
            footerControl_Erreserbak.AtzeraTestua = "Atzera";
            footerControl_Erreserbak.BackColor = Color.Silver;
            footerControl_Erreserbak.Dock = DockStyle.Bottom;
            footerControl_Erreserbak.Location = new Point(0, 745);
            footerControl_Erreserbak.Name = "footerControl_Erreserbak";
            footerControl_Erreserbak.Size = new Size(1541, 100);
            footerControl_Erreserbak.TabIndex = 12;
            footerControl_Erreserbak.TxataTestua = "Txata";
            // 
            // ErreserbakForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1541, 845);
            Controls.Add(dgv_ErreserbakIkusi);
            Controls.Add(pnl_Filtroak);
            Controls.Add(headerControl_Erreserbak);
            Controls.Add(footerControl_Erreserbak);
            Name = "ErreserbakForm";
            Text = "Erreserbak";
            WindowState = FormWindowState.Maximized;
            Load += ErreserbakForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgv_ErreserbakIkusi).EndInit();
            pnl_Filtroak.ResumeLayout(false);
            pnl_Filtroak.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgv_ErreserbakIkusi;
        private Label lbl_Bilatu;
        private TextBox txt_Bilatu;
        private Label lbl_Eguna;
        private DateTimePicker dtp_Eguna;
        private Button btn_Bilatu;
        private Button btn_Garbitu;
        private Panel pnl_Filtroak;
        private Controls.HeaderControl headerControl_Erreserbak;
        private Controls.FooterControl footerControl_Erreserbak;
    }
}