namespace TeknoBideTPV.UI.Controls
{
    partial class HeaderControl
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel tlp;
        private Label lbl_Izena;
        private Label lbl_Izenburua;
        private Panel pnlUser;
        private Label lbl_Erabiltzailea;
        private Label lbl_DataOrdua;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            tlp = new TableLayoutPanel();
            lbl_Izena = new Label();
            lbl_Izenburua = new Label();
            pnlUser = new Panel();
            tlpUser = new TableLayoutPanel();
            lbl_Erabiltzailea = new Label();
            lbl_DataOrdua = new Label();
            tlp.SuspendLayout();
            pnlUser.SuspendLayout();
            tlpUser.SuspendLayout();
            SuspendLayout();
            // 
            // tlp
            // 
            tlp.ColumnCount = 3;
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tlp.Controls.Add(lbl_Izena, 0, 0);
            tlp.Controls.Add(lbl_Izenburua, 1, 0);
            tlp.Controls.Add(pnlUser, 2, 0);
            tlp.Dock = DockStyle.Fill;
            tlp.Location = new Point(0, 0);
            tlp.Name = "tlp";
            tlp.RowCount = 1;
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlp.Size = new Size(934, 150);
            tlp.TabIndex = 0;
            // 
            // lbl_Izena
            // 
            lbl_Izena.Dock = DockStyle.Fill;
            lbl_Izena.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lbl_Izena.Location = new Point(3, 0);
            lbl_Izena.Name = "lbl_Izena";
            lbl_Izena.Size = new Size(302, 150);
            lbl_Izena.TabIndex = 0;
            lbl_Izena.Text = "TXAPELA";
            lbl_Izena.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbl_Izenburua
            // 
            lbl_Izenburua.Dock = DockStyle.Fill;
            lbl_Izenburua.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lbl_Izenburua.Location = new Point(311, 0);
            lbl_Izenburua.Name = "lbl_Izenburua";
            lbl_Izenburua.Size = new Size(311, 150);
            lbl_Izenburua.TabIndex = 1;
            lbl_Izenburua.Text = "IZENBURUA";
            lbl_Izenburua.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlUser
            // 
            pnlUser.Controls.Add(tlpUser);
            pnlUser.Dock = DockStyle.Fill;
            pnlUser.Location = new Point(628, 3);
            pnlUser.Name = "pnlUser";
            pnlUser.Size = new Size(303, 144);
            pnlUser.TabIndex = 2;
            // 
            // tlpUser
            // 
            tlpUser.ColumnCount = 1;
            tlpUser.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpUser.Controls.Add(lbl_Erabiltzailea, 0, 0);
            tlpUser.Controls.Add(lbl_DataOrdua, 0, 1);
            tlpUser.Dock = DockStyle.Fill;
            tlpUser.Location = new Point(0, 0);
            tlpUser.Name = "tlpUser";
            tlpUser.RowCount = 2;
            tlpUser.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpUser.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpUser.Size = new Size(303, 144);
            tlpUser.TabIndex = 0;
            // 
            // lbl_Erabiltzailea
            // 
            lbl_Erabiltzailea.Dock = DockStyle.Fill;
            lbl_Erabiltzailea.Font = new Font("Segoe UI", 16F);
            lbl_Erabiltzailea.Location = new Point(3, 0);
            lbl_Erabiltzailea.Name = "lbl_Erabiltzailea";
            lbl_Erabiltzailea.Size = new Size(297, 72);
            lbl_Erabiltzailea.TabIndex = 0;
            lbl_Erabiltzailea.Text = "Erabiltzailea";
            lbl_Erabiltzailea.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbl_DataOrdua
            // 
            lbl_DataOrdua.Dock = DockStyle.Fill;
            lbl_DataOrdua.Font = new Font("Segoe UI", 14F);
            lbl_DataOrdua.Location = new Point(3, 72);
            lbl_DataOrdua.Name = "lbl_DataOrdua";
            lbl_DataOrdua.Size = new Size(297, 72);
            lbl_DataOrdua.TabIndex = 1;
            lbl_DataOrdua.Text = "Data - Ordua";
            lbl_DataOrdua.TextAlign = ContentAlignment.MiddleRight;
            lbl_DataOrdua.Click += lbl_DataOrdua_Click;
            // 
            // HeaderControl
            // 
            BackColor = Color.FromArgb(210, 210, 210);
            Controls.Add(tlp);
            Name = "HeaderControl";
            Size = new Size(934, 150);
            tlp.ResumeLayout(false);
            pnlUser.ResumeLayout(false);
            tlpUser.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpUser;
    }
}
