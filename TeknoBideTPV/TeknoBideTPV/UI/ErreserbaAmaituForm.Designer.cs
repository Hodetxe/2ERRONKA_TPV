namespace TeknoBideTPV.UI
{
    partial class ErreserbaAmaituForm
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
            headerControl_ErreserbaAmaitu = new TeknoBideTPV.UI.Controls.HeaderControl();
            footerControl_ErreserbaAmaitu = new TeknoBideTPV.UI.Controls.FooterControl();
            pnl_Eskariak = new Panel();
            dgv_Eskariak = new DataGridView();
            pnl_Ordainketa = new Panel();
            cmb_Erreserbak = new ComboBox();
            lbl_ErreserbaAukeratu = new Label();
            btn_Ordaindu = new Button();
            lbl_Itzulia = new Label();
            lbl_ItzuliIzenburua = new Label();
            txt_JasoDenDirua = new TextBox();
            lbl_JasoDenDirua = new Label();
            btn_Txartela = new Button();
            btn_Eskudirua = new Button();
            lbl_OrdainketaMetodoa = new Label();
            lbl_Guztira = new Label();
            lbl_GuztiraIzenburua = new Label();
            pnl_Eskariak.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_Eskariak).BeginInit();
            pnl_Ordainketa.SuspendLayout();
            SuspendLayout();
            // 
            // headerControl_ErreserbaAmaitu
            // 
            headerControl_ErreserbaAmaitu.BackColor = Color.FromArgb(255, 140, 0);
            headerControl_ErreserbaAmaitu.DataOrdua = "Data - Ordua";
            headerControl_ErreserbaAmaitu.Dock = DockStyle.Top;
            headerControl_ErreserbaAmaitu.Erabiltzailea = "Erabiltzailea";
            headerControl_ErreserbaAmaitu.ForeColor = Color.White;
            headerControl_ErreserbaAmaitu.Izena = "TXAPELA";
            headerControl_ErreserbaAmaitu.Location = new Point(0, 0);
            headerControl_ErreserbaAmaitu.Name = "headerControl_ErreserbaAmaitu";
            headerControl_ErreserbaAmaitu.Size = new Size(1902, 100);
            headerControl_ErreserbaAmaitu.TabIndex = 0;
            headerControl_ErreserbaAmaitu.Titulo = "IZENBURUA";
            // 
            // footerControl_ErreserbaAmaitu
            // 
            footerControl_ErreserbaAmaitu.AtzeraTestua = "Atzera";
            footerControl_ErreserbaAmaitu.BackColor = Color.FromArgb(245, 124, 0);
            footerControl_ErreserbaAmaitu.Dock = DockStyle.Bottom;
            footerControl_ErreserbaAmaitu.ForeColor = Color.FromArgb(62, 39, 35);
            footerControl_ErreserbaAmaitu.Location = new Point(0, 933);
            footerControl_ErreserbaAmaitu.Name = "footerControl_ErreserbaAmaitu";
            footerControl_ErreserbaAmaitu.Size = new Size(1902, 100);
            footerControl_ErreserbaAmaitu.TabIndex = 1;
            footerControl_ErreserbaAmaitu.TxataTestua = "Txata";
            // 
            // pnl_Eskariak
            // 
            pnl_Eskariak.Controls.Add(dgv_Eskariak);
            pnl_Eskariak.Location = new Point(40, 180);
            pnl_Eskariak.Name = "pnl_Eskariak";
            pnl_Eskariak.Size = new Size(1100, 700);
            pnl_Eskariak.TabIndex = 2;
            // 
            // dgv_Eskariak
            // 
            dgv_Eskariak.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_Eskariak.Location = new Point(0, 0);
            dgv_Eskariak.Name = "dgv_Eskariak";
            dgv_Eskariak.RowHeadersWidth = 51;
            dgv_Eskariak.Size = new Size(1100, 700);
            dgv_Eskariak.TabIndex = 0;
            // 
            // pnl_Ordainketa
            // 
            pnl_Ordainketa.Controls.Add(cmb_Erreserbak);
            pnl_Ordainketa.Controls.Add(lbl_ErreserbaAukeratu);
            pnl_Ordainketa.Controls.Add(btn_Ordaindu);
            pnl_Ordainketa.Controls.Add(lbl_Itzulia);
            pnl_Ordainketa.Controls.Add(lbl_ItzuliIzenburua);
            pnl_Ordainketa.Controls.Add(txt_JasoDenDirua);
            pnl_Ordainketa.Controls.Add(lbl_JasoDenDirua);
            pnl_Ordainketa.Controls.Add(btn_Txartela);
            pnl_Ordainketa.Controls.Add(btn_Eskudirua);
            pnl_Ordainketa.Controls.Add(lbl_OrdainketaMetodoa);
            pnl_Ordainketa.Controls.Add(lbl_Guztira);
            pnl_Ordainketa.Controls.Add(lbl_GuztiraIzenburua);
            pnl_Ordainketa.Location = new Point(1180, 180);
            pnl_Ordainketa.Name = "pnl_Ordainketa";
            pnl_Ordainketa.Size = new Size(650, 700);
            pnl_Ordainketa.TabIndex = 3;
            // 
            // cmb_Erreserbak
            // 
            cmb_Erreserbak.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_Erreserbak.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmb_Erreserbak.FormattingEnabled = true;
            cmb_Erreserbak.Location = new Point(0, 50);
            cmb_Erreserbak.Name = "cmb_Erreserbak";
            cmb_Erreserbak.Size = new Size(400, 45);
            cmb_Erreserbak.TabIndex = 11;
            // 
            // lbl_ErreserbaAukeratu
            // 
            lbl_ErreserbaAukeratu.AutoSize = true;
            lbl_ErreserbaAukeratu.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_ErreserbaAukeratu.Location = new Point(0, 0);
            lbl_ErreserbaAukeratu.Name = "lbl_ErreserbaAukeratu";
            lbl_ErreserbaAukeratu.Size = new Size(264, 41);
            lbl_ErreserbaAukeratu.TabIndex = 10;
            lbl_ErreserbaAukeratu.Text = "Erreserba aukeratu";
            // 
            // btn_Ordaindu
            // 
            btn_Ordaindu.BackColor = Color.FromArgb(128, 255, 128);
            btn_Ordaindu.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Ordaindu.ForeColor = Color.White;
            btn_Ordaindu.Location = new Point(247, 572);
            btn_Ordaindu.Name = "btn_Ordaindu";
            btn_Ordaindu.Size = new Size(400, 120);
            btn_Ordaindu.TabIndex = 9;
            btn_Ordaindu.Text = "ORDAINDU";
            btn_Ordaindu.UseVisualStyleBackColor = false;
            // 
            // lbl_Itzulia
            // 
            lbl_Itzulia.AutoSize = true;
            lbl_Itzulia.Font = new Font("Segoe UI", 31.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Itzulia.Location = new Point(0, 620);
            lbl_Itzulia.Name = "lbl_Itzulia";
            lbl_Itzulia.Size = new Size(184, 72);
            lbl_Itzulia.TabIndex = 8;
            lbl_Itzulia.Text = "0,00 €";
            lbl_Itzulia.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbl_ItzuliIzenburua
            // 
            lbl_ItzuliIzenburua.AutoSize = true;
            lbl_ItzuliIzenburua.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_ItzuliIzenburua.Location = new Point(0, 570);
            lbl_ItzuliIzenburua.Name = "lbl_ItzuliIzenburua";
            lbl_ItzuliIzenburua.Size = new Size(190, 41);
            lbl_ItzuliIzenburua.TabIndex = 7;
            lbl_ItzuliIzenburua.Text = "Itzuli beharra";
            // 
            // txt_JasoDenDirua
            // 
            txt_JasoDenDirua.Font = new Font("Segoe UI", 28.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_JasoDenDirua.Location = new Point(0, 490);
            txt_JasoDenDirua.Name = "txt_JasoDenDirua";
            txt_JasoDenDirua.Size = new Size(300, 70);
            txt_JasoDenDirua.TabIndex = 6;
            txt_JasoDenDirua.TextAlign = HorizontalAlignment.Right;
            txt_JasoDenDirua.UseWaitCursor = true;
            // 
            // lbl_JasoDenDirua
            // 
            lbl_JasoDenDirua.AutoSize = true;
            lbl_JasoDenDirua.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_JasoDenDirua.Location = new Point(0, 440);
            lbl_JasoDenDirua.Name = "lbl_JasoDenDirua";
            lbl_JasoDenDirua.Size = new Size(209, 41);
            lbl_JasoDenDirua.TabIndex = 5;
            lbl_JasoDenDirua.Text = "Jaso den dirua";
            // 
            // btn_Txartela
            // 
            btn_Txartela.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Txartela.Location = new Point(330, 300);
            btn_Txartela.Name = "btn_Txartela";
            btn_Txartela.Size = new Size(300, 100);
            btn_Txartela.TabIndex = 4;
            btn_Txartela.Text = "Txartela";
            btn_Txartela.UseVisualStyleBackColor = true;
            // 
            // btn_Eskudirua
            // 
            btn_Eskudirua.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Eskudirua.Location = new Point(0, 300);
            btn_Eskudirua.Name = "btn_Eskudirua";
            btn_Eskudirua.Size = new Size(300, 100);
            btn_Eskudirua.TabIndex = 3;
            btn_Eskudirua.Text = "Eskudirua";
            btn_Eskudirua.UseVisualStyleBackColor = true;
            // 
            // lbl_OrdainketaMetodoa
            // 
            lbl_OrdainketaMetodoa.AutoSize = true;
            lbl_OrdainketaMetodoa.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_OrdainketaMetodoa.Location = new Point(0, 240);
            lbl_OrdainketaMetodoa.Name = "lbl_OrdainketaMetodoa";
            lbl_OrdainketaMetodoa.Size = new Size(415, 46);
            lbl_OrdainketaMetodoa.TabIndex = 2;
            lbl_OrdainketaMetodoa.Text = "ORDAINKETA METODOA";
            // 
            // lbl_Guztira
            // 
            lbl_Guztira.AutoSize = true;
            lbl_Guztira.Font = new Font("Segoe UI", 28.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Guztira.Location = new Point(3, 160);
            lbl_Guztira.Name = "lbl_Guztira";
            lbl_Guztira.Size = new Size(148, 62);
            lbl_Guztira.TabIndex = 1;
            lbl_Guztira.Text = "0,00€";
            lbl_Guztira.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbl_GuztiraIzenburua
            // 
            lbl_GuztiraIzenburua.AutoSize = true;
            lbl_GuztiraIzenburua.Font = new Font("Segoe UI", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_GuztiraIzenburua.Location = new Point(0, 110);
            lbl_GuztiraIzenburua.Name = "lbl_GuztiraIzenburua";
            lbl_GuztiraIzenburua.Size = new Size(182, 50);
            lbl_GuztiraIzenburua.TabIndex = 0;
            lbl_GuztiraIzenburua.Text = "GUZTIRA";
            // 
            // ErreserbaAmaituForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 1033);
            Controls.Add(pnl_Ordainketa);
            Controls.Add(pnl_Eskariak);
            Controls.Add(footerControl_ErreserbaAmaitu);
            Controls.Add(headerControl_ErreserbaAmaitu);
            Name = "ErreserbaAmaituForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ErreserbaAmaituForm";
            WindowState = FormWindowState.Maximized;
            pnl_Eskariak.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgv_Eskariak).EndInit();
            pnl_Ordainketa.ResumeLayout(false);
            pnl_Ordainketa.PerformLayout();
            ResumeLayout(false);
        }


        #endregion

        private Controls.HeaderControl headerControl_ErreserbaAmaitu;
        private Controls.FooterControl footerControl_ErreserbaAmaitu;
        private Panel pnl_Eskariak;
        private DataGridView dgv_Eskariak;
        private Panel pnl_Ordainketa;
        private Label lbl_Guztira;
        private Label lbl_GuztiraIzenburua;
        private Button btn_Txartela;
        private Button btn_Eskudirua;
        private Label lbl_OrdainketaMetodoa;
        private Label lbl_Itzulia;
        private Label lbl_ItzuliIzenburua;
        private TextBox txt_JasoDenDirua;
        private Label lbl_JasoDenDirua;
        private Button btn_Ordaindu;
        private ComboBox cmb_Erreserbak;
        private Label lbl_ErreserbaAukeratu;
    }
}