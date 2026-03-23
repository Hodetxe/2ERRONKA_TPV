namespace TeknoBideTPV.UI
{
    partial class ErreserbakSortuForm
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
            pnl_EzkerrekoPanela = new Panel();
            nud_PertsonaKopurua = new NumericUpDown();
            lbl_PertsonaKopurua = new Label();
            txt_Telefonoa = new TextBox();
            lbl_Telefonoa = new Label();
            txt_BezeroIzena = new TextBox();
            lbl_BezeroIzena = new Label();
            pnl_EskubikoPanela = new Panel();
            cmb_Ordua = new ComboBox();
            cmb_Mahaiak = new ComboBox();
            lbl_Mahaia = new Label();
            lbl_Ordua = new Label();
            dtp_Eguna = new DateTimePicker();
            lbl_Eguna = new Label();
            btn_Gorde = new Button();
            headerControl_ErreserbakSortu = new TeknoBideTPV.UI.Controls.HeaderControl();
            footerControl_ErreserbakSortu = new TeknoBideTPV.UI.Controls.FooterControl();
            pnl_EzkerrekoPanela.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_PertsonaKopurua).BeginInit();
            pnl_EskubikoPanela.SuspendLayout();
            SuspendLayout();
            // 
            // pnl_EzkerrekoPanela
            // 
            pnl_EzkerrekoPanela.BorderStyle = BorderStyle.FixedSingle;
            pnl_EzkerrekoPanela.Controls.Add(nud_PertsonaKopurua);
            pnl_EzkerrekoPanela.Controls.Add(lbl_PertsonaKopurua);
            pnl_EzkerrekoPanela.Controls.Add(txt_Telefonoa);
            pnl_EzkerrekoPanela.Controls.Add(lbl_Telefonoa);
            pnl_EzkerrekoPanela.Controls.Add(txt_BezeroIzena);
            pnl_EzkerrekoPanela.Controls.Add(lbl_BezeroIzena);
            pnl_EzkerrekoPanela.Location = new Point(80, 150);
            pnl_EzkerrekoPanela.Name = "pnl_EzkerrekoPanela";
            pnl_EzkerrekoPanela.Padding = new Padding(30);
            pnl_EzkerrekoPanela.Size = new Size(800, 700);
            pnl_EzkerrekoPanela.TabIndex = 11;
            // 
            // nud_PertsonaKopurua
            // 
            nud_PertsonaKopurua.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nud_PertsonaKopurua.Location = new Point(20, 400);
            nud_PertsonaKopurua.Name = "nud_PertsonaKopurua";
            nud_PertsonaKopurua.Size = new Size(745, 61);
            nud_PertsonaKopurua.TabIndex = 5;
            nud_PertsonaKopurua.Controls[0].Visible = false;
            // 
            // lbl_PertsonaKopurua
            // 
            lbl_PertsonaKopurua.AutoSize = true;
            lbl_PertsonaKopurua.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_PertsonaKopurua.Location = new Point(20, 334);
            lbl_PertsonaKopurua.Name = "lbl_PertsonaKopurua";
            lbl_PertsonaKopurua.Size = new Size(336, 54);
            lbl_PertsonaKopurua.TabIndex = 4;
            lbl_PertsonaKopurua.Text = "Pertsona Kopurua";
            // 
            // txt_Telefonoa
            // 
            txt_Telefonoa.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_Telefonoa.Location = new Point(20, 246);
            txt_Telefonoa.Name = "txt_Telefonoa";
            txt_Telefonoa.Size = new Size(745, 61);
            txt_Telefonoa.TabIndex = 3;
            // 
            // lbl_Telefonoa
            // 
            lbl_Telefonoa.AutoSize = true;
            lbl_Telefonoa.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Telefonoa.Location = new Point(20, 170);
            lbl_Telefonoa.Name = "lbl_Telefonoa";
            lbl_Telefonoa.Size = new Size(194, 54);
            lbl_Telefonoa.TabIndex = 2;
            lbl_Telefonoa.Text = "Telefonoa";
            // 
            // txt_BezeroIzena
            // 
            txt_BezeroIzena.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_BezeroIzena.Location = new Point(20, 88);
            txt_BezeroIzena.Name = "txt_BezeroIzena";
            txt_BezeroIzena.Size = new Size(745, 61);
            txt_BezeroIzena.TabIndex = 1;
            // 
            // lbl_BezeroIzena
            // 
            lbl_BezeroIzena.AutoSize = true;
            lbl_BezeroIzena.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_BezeroIzena.Location = new Point(20, 20);
            lbl_BezeroIzena.Name = "lbl_BezeroIzena";
            lbl_BezeroIzena.Size = new Size(247, 54);
            lbl_BezeroIzena.TabIndex = 0;
            lbl_BezeroIzena.Text = "Bezero Izena";
            // 
            // pnl_EskubikoPanela
            // 
            pnl_EskubikoPanela.BorderStyle = BorderStyle.FixedSingle;
            pnl_EskubikoPanela.Controls.Add(cmb_Ordua);
            pnl_EskubikoPanela.Controls.Add(cmb_Mahaiak);
            pnl_EskubikoPanela.Controls.Add(lbl_Mahaia);
            pnl_EskubikoPanela.Controls.Add(lbl_Ordua);
            pnl_EskubikoPanela.Controls.Add(dtp_Eguna);
            pnl_EskubikoPanela.Controls.Add(lbl_Eguna);
            pnl_EskubikoPanela.Location = new Point(1040, 150);
            pnl_EskubikoPanela.Name = "pnl_EskubikoPanela";
            pnl_EskubikoPanela.Padding = new Padding(30);
            pnl_EskubikoPanela.Size = new Size(800, 700);
            pnl_EskubikoPanela.TabIndex = 12;
            // 
            // cmb_Ordua
            // 
            cmb_Ordua.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmb_Ordua.FormattingEnabled = true;
            cmb_Ordua.Location = new Point(20, 245);
            cmb_Ordua.Name = "cmb_Ordua";
            cmb_Ordua.Size = new Size(723, 62);
            cmb_Ordua.TabIndex = 6;
            // 
            // cmb_Mahaiak
            // 
            cmb_Mahaiak.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmb_Mahaiak.FormattingEnabled = true;
            cmb_Mahaiak.Location = new Point(20, 404);
            cmb_Mahaiak.Name = "cmb_Mahaiak";
            cmb_Mahaiak.Size = new Size(723, 62);
            cmb_Mahaiak.TabIndex = 5;
            // 
            // lbl_Mahaia
            // 
            lbl_Mahaia.AutoSize = true;
            lbl_Mahaia.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Mahaia.Location = new Point(20, 334);
            lbl_Mahaia.Name = "lbl_Mahaia";
            lbl_Mahaia.Size = new Size(152, 54);
            lbl_Mahaia.TabIndex = 4;
            lbl_Mahaia.Text = "Mahaia";
            // 
            // lbl_Ordua
            // 
            lbl_Ordua.AutoSize = true;
            lbl_Ordua.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Ordua.Location = new Point(20, 170);
            lbl_Ordua.Name = "lbl_Ordua";
            lbl_Ordua.Size = new Size(134, 54);
            lbl_Ordua.TabIndex = 2;
            lbl_Ordua.Text = "Ordua";
            // 
            // dtp_Eguna
            // 
            dtp_Eguna.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtp_Eguna.Location = new Point(20, 88);
            dtp_Eguna.Name = "dtp_Eguna";
            dtp_Eguna.Size = new Size(723, 61);
            dtp_Eguna.TabIndex = 1;
            // 
            // lbl_Eguna
            // 
            lbl_Eguna.AutoSize = true;
            lbl_Eguna.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Eguna.Location = new Point(20, 20);
            lbl_Eguna.Name = "lbl_Eguna";
            lbl_Eguna.Size = new Size(133, 54);
            lbl_Eguna.TabIndex = 0;
            lbl_Eguna.Text = "Eguna";
            // 
            // btn_Gorde
            // 
            btn_Gorde.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_Gorde.Location = new Point(1663, 865);
            btn_Gorde.Name = "btn_Gorde";
            btn_Gorde.Size = new Size(177, 71);
            btn_Gorde.TabIndex = 13;
            btn_Gorde.Text = "Gorde";
            btn_Gorde.UseVisualStyleBackColor = true;
            btn_Gorde.Click += btn_Gorde_Click;
            // 
            // headerControl_ErreserbakSortu
            // 
            headerControl_ErreserbakSortu.BackColor = Color.FromArgb(210, 210, 210);
            headerControl_ErreserbakSortu.DataOrdua = "Data - Ordua";
            headerControl_ErreserbakSortu.Dock = DockStyle.Top;
            headerControl_ErreserbakSortu.Erabiltzailea = "Erabiltzailea";
            headerControl_ErreserbakSortu.Izena = "TXAPELA";
            headerControl_ErreserbakSortu.Location = new Point(0, 0);
            headerControl_ErreserbakSortu.Name = "headerControl_ErreserbakSortu";
            headerControl_ErreserbakSortu.Size = new Size(1902, 100);
            headerControl_ErreserbakSortu.TabIndex = 14;
            headerControl_ErreserbakSortu.Titulo = "IZENBURUA";
            // 
            // footerControl_ErreserbakSortu
            // 
            footerControl_ErreserbakSortu.AtzeraTestua = "Atzera";
            footerControl_ErreserbakSortu.BackColor = Color.Silver;
            footerControl_ErreserbakSortu.Dock = DockStyle.Bottom;
            footerControl_ErreserbakSortu.Location = new Point(0, 933);
            footerControl_ErreserbakSortu.Name = "footerControl_ErreserbakSortu";
            footerControl_ErreserbakSortu.Size = new Size(1902, 100);
            footerControl_ErreserbakSortu.TabIndex = 15;
            // 
            // ErreserbakSortuForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 1033);
            Controls.Add(footerControl_ErreserbakSortu);
            Controls.Add(headerControl_ErreserbakSortu);
            Controls.Add(btn_Gorde);
            Controls.Add(pnl_EskubikoPanela);
            Controls.Add(pnl_EzkerrekoPanela);
            Name = "ErreserbakSortuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ErreserbakSortuForm";
            WindowState = FormWindowState.Maximized;
            pnl_EzkerrekoPanela.ResumeLayout(false);
            pnl_EzkerrekoPanela.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_PertsonaKopurua).EndInit();
            pnl_EskubikoPanela.ResumeLayout(false);
            pnl_EskubikoPanela.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel pnl_EzkerrekoPanela;
        private TextBox txt_Telefonoa;
        private Label lbl_Telefonoa;
        private TextBox txt_BezeroIzena;
        private Label lbl_BezeroIzena;
        private NumericUpDown nud_PertsonaKopurua;
        private Label lbl_PertsonaKopurua;
        private Panel pnl_EskubikoPanela;
        private Label lbl_Eguna;
        private ComboBox cmb_Mahaiak;
        private Label lbl_Mahaia;
        private Label lbl_Ordua;
        private DateTimePicker dtp_Eguna;
        private Button btn_Gorde;
        private ComboBox cmb_Ordua;
        private Controls.HeaderControl headerControl_ErreserbakSortu;
        private Controls.FooterControl footerControl_ErreserbakSortu;
    }
}