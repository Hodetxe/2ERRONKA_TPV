namespace TeknoBideTPV.UI
{
    partial class MahaiakKudeatuForm
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
            footerControl_MahaiakKudeatu = new TeknoBideTPV.UI.Controls.FooterControl();
            headerControl_MahaiakKudeatu = new TeknoBideTPV.UI.Controls.HeaderControl();
            pnl_Edukia = new Panel();
            pnl_Ezaugarriak = new Panel();
            btn_MahaiBerria = new Button();
            btn_Ezabatu = new Button();
            btn_Gorde = new Button();
            cmb_Kokapena = new ComboBox();
            lbl_Kokapena = new Label();
            nud_PertsonaKopurua = new NumericUpDown();
            lbl_PertsonaKopurua = new Label();
            txt_Zenbakia = new TextBox();
            lbl_Izena = new Label();
            lbl_Izenburua = new Label();
            flp_Mahaiak = new FlowLayoutPanel();
            btn_Garbitu = new Button();
            pnl_Edukia.SuspendLayout();
            pnl_Ezaugarriak.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_PertsonaKopurua).BeginInit();
            SuspendLayout();
            // 
            // footerControl_MahaiakKudeatu
            // 
            footerControl_MahaiakKudeatu.AtzeraTestua = "Atzera";
            footerControl_MahaiakKudeatu.BackColor = Color.Silver;
            footerControl_MahaiakKudeatu.Dock = DockStyle.Bottom;
            footerControl_MahaiakKudeatu.Location = new Point(0, 933);
            footerControl_MahaiakKudeatu.Name = "footerControl_MahaiakKudeatu";
            footerControl_MahaiakKudeatu.Size = new Size(1902, 100);
            footerControl_MahaiakKudeatu.TabIndex = 0;
            // 
            // headerControl_MahaiakKudeatu
            // 
            headerControl_MahaiakKudeatu.BackColor = Color.FromArgb(210, 210, 210);
            headerControl_MahaiakKudeatu.DataOrdua = "Data - Ordua";
            headerControl_MahaiakKudeatu.Dock = DockStyle.Top;
            headerControl_MahaiakKudeatu.Erabiltzailea = "Erabiltzailea";
            headerControl_MahaiakKudeatu.Izena = "TXAPELA";
            headerControl_MahaiakKudeatu.Location = new Point(0, 0);
            headerControl_MahaiakKudeatu.Name = "headerControl_MahaiakKudeatu";
            headerControl_MahaiakKudeatu.Size = new Size(1902, 100);
            headerControl_MahaiakKudeatu.TabIndex = 1;
            headerControl_MahaiakKudeatu.Titulo = "IZENBURUA";
            // 
            // pnl_Edukia
            // 
            pnl_Edukia.AllowDrop = true;
            pnl_Edukia.Controls.Add(pnl_Ezaugarriak);
            pnl_Edukia.Controls.Add(flp_Mahaiak);
            pnl_Edukia.Dock = DockStyle.Fill;
            pnl_Edukia.Location = new Point(0, 100);
            pnl_Edukia.Name = "pnl_Edukia";
            pnl_Edukia.Size = new Size(1902, 833);
            pnl_Edukia.TabIndex = 0;
            // 
            // pnl_Ezaugarriak
            // 
            pnl_Ezaugarriak.Controls.Add(btn_Garbitu);
            pnl_Ezaugarriak.Controls.Add(btn_MahaiBerria);
            pnl_Ezaugarriak.Controls.Add(btn_Ezabatu);
            pnl_Ezaugarriak.Controls.Add(btn_Gorde);
            pnl_Ezaugarriak.Controls.Add(cmb_Kokapena);
            pnl_Ezaugarriak.Controls.Add(lbl_Kokapena);
            pnl_Ezaugarriak.Controls.Add(nud_PertsonaKopurua);
            pnl_Ezaugarriak.Controls.Add(lbl_PertsonaKopurua);
            pnl_Ezaugarriak.Controls.Add(txt_Zenbakia);
            pnl_Ezaugarriak.Controls.Add(lbl_Izena);
            pnl_Ezaugarriak.Controls.Add(lbl_Izenburua);
            pnl_Ezaugarriak.Dock = DockStyle.Fill;
            pnl_Ezaugarriak.Location = new Point(1200, 0);
            pnl_Ezaugarriak.Name = "pnl_Ezaugarriak";
            pnl_Ezaugarriak.Size = new Size(702, 833);
            pnl_Ezaugarriak.TabIndex = 1;
            // 
            // btn_MahaiBerria
            // 
            btn_MahaiBerria.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_MahaiBerria.Location = new Point(20, 450);
            btn_MahaiBerria.Name = "btn_MahaiBerria";
            btn_MahaiBerria.Size = new Size(520, 70);
            btn_MahaiBerria.TabIndex = 9;
            btn_MahaiBerria.Text = "MAHAI BERRIA";
            btn_MahaiBerria.UseVisualStyleBackColor = true;
            btn_MahaiBerria.Click += btn_MahaiBerria_Click;
            // 
            // btn_Ezabatu
            // 
            btn_Ezabatu.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_Ezabatu.Location = new Point(290, 360);
            btn_Ezabatu.Name = "btn_Ezabatu";
            btn_Ezabatu.Size = new Size(250, 70);
            btn_Ezabatu.TabIndex = 8;
            btn_Ezabatu.Text = "EZABATU";
            btn_Ezabatu.UseVisualStyleBackColor = true;
            btn_Ezabatu.Click += btn_Ezabatu_Click;
            // 
            // btn_Gorde
            // 
            btn_Gorde.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_Gorde.Location = new Point(20, 360);
            btn_Gorde.Name = "btn_Gorde";
            btn_Gorde.Size = new Size(250, 70);
            btn_Gorde.TabIndex = 7;
            btn_Gorde.Text = "GORDE";
            btn_Gorde.UseVisualStyleBackColor = true;
            btn_Gorde.Click += btn_Gorde_Click;
            // 
            // cmb_Kokapena
            // 
            cmb_Kokapena.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_Kokapena.Enabled = false;
            cmb_Kokapena.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmb_Kokapena.FormattingEnabled = true;
            cmb_Kokapena.Location = new Point(240, 250);
            cmb_Kokapena.Name = "cmb_Kokapena";
            cmb_Kokapena.Size = new Size(300, 49);
            cmb_Kokapena.TabIndex = 6;
            // 
            // lbl_Kokapena
            // 
            lbl_Kokapena.AutoSize = true;
            lbl_Kokapena.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Kokapena.Location = new Point(20, 250);
            lbl_Kokapena.Name = "lbl_Kokapena";
            lbl_Kokapena.RightToLeft = RightToLeft.No;
            lbl_Kokapena.Size = new Size(155, 41);
            lbl_Kokapena.TabIndex = 5;
            lbl_Kokapena.Text = "Kokapena:";
            // 
            // nud_PertsonaKopurua
            // 
            nud_PertsonaKopurua.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nud_PertsonaKopurua.Location = new Point(266, 175);
            nud_PertsonaKopurua.Name = "nud_PertsonaKopurua";
            nud_PertsonaKopurua.Size = new Size(274, 47);
            nud_PertsonaKopurua.TabIndex = 4;
            // 
            // lbl_PertsonaKopurua
            // 
            lbl_PertsonaKopurua.AutoSize = true;
            lbl_PertsonaKopurua.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_PertsonaKopurua.Location = new Point(20, 180);
            lbl_PertsonaKopurua.Name = "lbl_PertsonaKopurua";
            lbl_PertsonaKopurua.RightToLeft = RightToLeft.No;
            lbl_PertsonaKopurua.Size = new Size(240, 38);
            lbl_PertsonaKopurua.TabIndex = 3;
            lbl_PertsonaKopurua.Text = "Pertsona kopurua:";
            // 
            // txt_Zenbakia
            // 
            txt_Zenbakia.BackColor = Color.White;
            txt_Zenbakia.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_Zenbakia.Location = new Point(190, 104);
            txt_Zenbakia.Name = "txt_Zenbakia";
            txt_Zenbakia.ReadOnly = true;
            txt_Zenbakia.Size = new Size(350, 47);
            txt_Zenbakia.TabIndex = 2;
            // 
            // lbl_Izena
            // 
            lbl_Izena.AutoSize = true;
            lbl_Izena.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Izena.Location = new Point(20, 110);
            lbl_Izena.Name = "lbl_Izena";
            lbl_Izena.Size = new Size(145, 41);
            lbl_Izena.TabIndex = 1;
            lbl_Izena.Text = "Zenbakia:";
            // 
            // lbl_Izenburua
            // 
            lbl_Izenburua.AutoSize = true;
            lbl_Izenburua.Font = new Font("Segoe UI Semibold", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Izenburua.Location = new Point(20, 20);
            lbl_Izenburua.Name = "lbl_Izenburua";
            lbl_Izenburua.Size = new Size(349, 60);
            lbl_Izenburua.TabIndex = 0;
            lbl_Izenburua.Text = "Mahaia kudeatu";
            // 
            // flp_Mahaiak
            // 
            flp_Mahaiak.AutoScroll = true;
            flp_Mahaiak.Dock = DockStyle.Left;
            flp_Mahaiak.Location = new Point(0, 0);
            flp_Mahaiak.Name = "flp_Mahaiak";
            flp_Mahaiak.Padding = new Padding(10);
            flp_Mahaiak.Size = new Size(1200, 833);
            flp_Mahaiak.TabIndex = 0;
            // 
            // btn_Garbitu
            // 
            btn_Garbitu.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_Garbitu.Location = new Point(447, 27);
            btn_Garbitu.Name = "btn_Garbitu";
            btn_Garbitu.Size = new Size(188, 53);
            btn_Garbitu.TabIndex = 10;
            btn_Garbitu.Text = "GARBITU";
            btn_Garbitu.UseVisualStyleBackColor = true;
            btn_Garbitu.Click += btn_Garbitu_Click;
            // 
            // MahaiakKudeatuForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 1033);
            Controls.Add(pnl_Edukia);
            Controls.Add(headerControl_MahaiakKudeatu);
            Controls.Add(footerControl_MahaiakKudeatu);
            Name = "MahaiakKudeatuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MahaiakKudeatuForm";
            WindowState = FormWindowState.Maximized;
            pnl_Edukia.ResumeLayout(false);
            pnl_Ezaugarriak.ResumeLayout(false);
            pnl_Ezaugarriak.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_PertsonaKopurua).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Controls.FooterControl footerControl_MahaiakKudeatu;
        private Controls.HeaderControl headerControl_MahaiakKudeatu;
        private Panel pnl_Edukia;
        private FlowLayoutPanel flp_Mahaiak;
        private Panel pnl_Ezaugarriak;
        private Label lbl_PertsonaKopurua;
        private TextBox txt_Zenbakia;
        private Label lbl_Izena;
        private Label lbl_Izenburua;
        private NumericUpDown nud_PertsonaKopurua;
        private ComboBox cmb_Kokapena;
        private Label lbl_Kokapena;
        private Button btn_MahaiBerria;
        private Button btn_Ezabatu;
        private Button btn_Gorde;
        private Button btn_Garbitu;
    }
}