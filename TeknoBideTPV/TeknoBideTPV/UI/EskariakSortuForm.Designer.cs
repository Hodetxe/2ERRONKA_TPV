namespace TeknoBideTPV.UI
{
    partial class EskariakSortuForm
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
            flp_Produktuak = new FlowLayoutPanel();
            dgv_EskariaProduktua = new DataGridView();
            lbl_PrezioTotala = new Label();
            txt_PrezioTotala = new TextBox();
            cbo_Erreserba = new ComboBox();
            lbl_AukeratutakoErreserba = new Label();
            btn_SortuEskaria = new Button();
            pnl_ProduktuMotak = new Panel();
            flp_ProduktuMotak = new FlowLayoutPanel();
            headerControl_EskariakSortu = new TeknoBideTPV.UI.Controls.HeaderControl();
            footerControl_EskariakSortu = new TeknoBideTPV.UI.Controls.FooterControl();
            ((System.ComponentModel.ISupportInitialize)dgv_EskariaProduktua).BeginInit();
            SuspendLayout();
            // 
            // flp_Produktuak
            // 
            flp_Produktuak.AutoScroll = true;
            flp_Produktuak.Location = new Point(34, 246);
            flp_Produktuak.Name = "flp_Produktuak";
            flp_Produktuak.Size = new Size(1005, 679);
            flp_Produktuak.TabIndex = 1;
            // 
            // dgv_EskariaProduktua
            // 
            dgv_EskariaProduktua.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_EskariaProduktua.Location = new Point(1132, 190);
            dgv_EskariaProduktua.Name = "dgv_EskariaProduktua";
            dgv_EskariaProduktua.RowHeadersWidth = 51;
            dgv_EskariaProduktua.Size = new Size(720, 580);
            dgv_EskariaProduktua.TabIndex = 2;
            // 
            // lbl_PrezioTotala
            // 
            lbl_PrezioTotala.AutoSize = true;
            lbl_PrezioTotala.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_PrezioTotala.Location = new Point(1250, 800);
            lbl_PrezioTotala.Name = "lbl_PrezioTotala";
            lbl_PrezioTotala.Size = new Size(148, 31);
            lbl_PrezioTotala.TabIndex = 3;
            lbl_PrezioTotala.Text = "Prezio Totala:";
            // 
            // txt_PrezioTotala
            // 
            txt_PrezioTotala.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_PrezioTotala.Location = new Point(1480, 797);
            txt_PrezioTotala.Name = "txt_PrezioTotala";
            txt_PrezioTotala.Size = new Size(125, 38);
            txt_PrezioTotala.TabIndex = 4;
            // 
            // cbo_Erreserba
            // 
            cbo_Erreserba.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbo_Erreserba.FormattingEnabled = true;
            cbo_Erreserba.Location = new Point(1480, 144);
            cbo_Erreserba.Name = "cbo_Erreserba";
            cbo_Erreserba.Size = new Size(219, 39);
            cbo_Erreserba.TabIndex = 5;
            // 
            // lbl_AukeratutakoErreserba
            // 
            lbl_AukeratutakoErreserba.AutoSize = true;
            lbl_AukeratutakoErreserba.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_AukeratutakoErreserba.Location = new Point(1184, 144);
            lbl_AukeratutakoErreserba.Name = "lbl_AukeratutakoErreserba";
            lbl_AukeratutakoErreserba.Size = new Size(257, 31);
            lbl_AukeratutakoErreserba.TabIndex = 6;
            lbl_AukeratutakoErreserba.Text = "Aukeratutako Erreserba:";
            // 
            // btn_SortuEskaria
            // 
            btn_SortuEskaria.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_SortuEskaria.Location = new Point(1334, 850);
            btn_SortuEskaria.Name = "btn_SortuEskaria";
            btn_SortuEskaria.Size = new Size(216, 68);
            btn_SortuEskaria.TabIndex = 8;
            btn_SortuEskaria.Text = "Eskaria Sortu";
            btn_SortuEskaria.UseVisualStyleBackColor = true;
            btn_SortuEskaria.Click += btn_SortuEskaria_Click;
            // 
            // pnl_ProduktuMotak
            // 
            pnl_ProduktuMotak.AutoSize = true;
            pnl_ProduktuMotak.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnl_ProduktuMotak.Location = new Point(34, 144);
            pnl_ProduktuMotak.Name = "pnl_ProduktuMotak";
            pnl_ProduktuMotak.Size = new Size(0, 0);
            pnl_ProduktuMotak.TabIndex = 9;
            // 
            // flp_ProduktuMotak
            // 
            flp_ProduktuMotak.Location = new Point(34, 115);
            flp_ProduktuMotak.Name = "flp_ProduktuMotak";
            flp_ProduktuMotak.Size = new Size(1005, 125);
            flp_ProduktuMotak.TabIndex = 10;
            // 
            // headerControl_EskariakSortu
            // 
            headerControl_EskariakSortu.BackColor = Color.FromArgb(255, 140, 0);
            headerControl_EskariakSortu.DataOrdua = "Data - Ordua";
            headerControl_EskariakSortu.Dock = DockStyle.Top;
            headerControl_EskariakSortu.Erabiltzailea = "Erabiltzailea";
            headerControl_EskariakSortu.ForeColor = Color.White;
            headerControl_EskariakSortu.Izena = "TXAPELA";
            headerControl_EskariakSortu.Location = new Point(0, 0);
            headerControl_EskariakSortu.Name = "headerControl_EskariakSortu";
            headerControl_EskariakSortu.Size = new Size(1902, 100);
            headerControl_EskariakSortu.TabIndex = 13;
            headerControl_EskariakSortu.Titulo = "IZENBURUA";
            // 
            // footerControl_EskariakSortu
            // 
            footerControl_EskariakSortu.AtzeraTestua = "Atzera";
            footerControl_EskariakSortu.BackColor = Color.FromArgb(245, 124, 0);
            footerControl_EskariakSortu.Dock = DockStyle.Bottom;
            footerControl_EskariakSortu.ForeColor = Color.FromArgb(62, 39, 35);
            footerControl_EskariakSortu.Location = new Point(0, 933);
            footerControl_EskariakSortu.Name = "footerControl_EskariakSortu";
            footerControl_EskariakSortu.Size = new Size(1902, 100);
            footerControl_EskariakSortu.TabIndex = 14;
            footerControl_EskariakSortu.TxataTestua = "Txata";
            // 
            // EskariakSortuForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 1033);
            Controls.Add(footerControl_EskariakSortu);
            Controls.Add(headerControl_EskariakSortu);
            Controls.Add(flp_ProduktuMotak);
            Controls.Add(pnl_ProduktuMotak);
            Controls.Add(btn_SortuEskaria);
            Controls.Add(lbl_AukeratutakoErreserba);
            Controls.Add(cbo_Erreserba);
            Controls.Add(txt_PrezioTotala);
            Controls.Add(lbl_PrezioTotala);
            Controls.Add(dgv_EskariaProduktua);
            Controls.Add(flp_Produktuak);
            Name = "EskariakSortuForm";
            Text = "EskariakSortuForm";
            WindowState = FormWindowState.Maximized;
            Load += EskariakSortuForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgv_EskariaProduktua).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private FlowLayoutPanel flp_Produktuak;
        private DataGridView dgv_EskariaProduktua;
        private Label lbl_PrezioTotala;
        private TextBox txt_PrezioTotala;
        private ComboBox cbo_Erreserba;
        private Label lbl_AukeratutakoErreserba;
        private Button btn_SortuEskaria;
        private Panel pnl_ProduktuMotak;
        private FlowLayoutPanel flp_ProduktuMotak;
        private Controls.HeaderControl headerControl_EskariakSortu;
        private Controls.FooterControl footerControl_EskariakSortu;
    }
}