namespace TeknoBideTPV.UI.Controls
{
    partial class FooterControl
    {
        private System.ComponentModel.IContainer components = null;

        private Button btn_Txata;
        private Button btn_Atzera;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btn_Txata = new Button();
            btn_Atzera = new Button();
            SuspendLayout();

            // 
            // btn_Txata
            // 
            btn_Txata.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btn_Txata.Name = "btn_Txata";
            btn_Txata.Text = "Txata";
            btn_Txata.Size = new Size(180, 50);
            btn_Txata.Location = new Point(20, 15);
            btn_Txata.BackColor = Color.White;
            btn_Txata.FlatStyle = FlatStyle.Flat;
            btn_Txata.FlatAppearance.BorderSize = 0;
            btn_Txata.TextAlign = ContentAlignment.MiddleCenter;
            btn_Txata.Padding = new Padding(10, 0, 10, 0);
            btn_Txata.Click += btn_Txata_Click;

            // 
            // btn_Atzera
            // 
            btn_Atzera.Size = new Size(200, 60);
            btn_Atzera.Location = new Point(this.Width - 220, 10);
            btn_Atzera.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            btn_Atzera.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btn_Atzera.Text = "Atzera";
            btn_Atzera.BackColor = Color.White;
            btn_Atzera.FlatStyle = FlatStyle.Flat;
            btn_Atzera.FlatAppearance.BorderSize = 0;
            btn_Atzera.Image = Properties.Resources.ico_atzera;
            btn_Atzera.ImageAlign = ContentAlignment.MiddleLeft;
            btn_Atzera.TextAlign = ContentAlignment.MiddleRight;
            btn_Atzera.Padding = new Padding(10, 0, 10, 0);
            btn_Atzera.Click += btn_Atzera_Click;

            // 
            // FooterControl
            // 
            BackColor = Color.Silver;
            Controls.Add(btn_Txata);
            Controls.Add(btn_Atzera);
            Name = "FooterControl";
            Size = new Size(1245, 80);
            ResumeLayout(false);
        }
    }
}
