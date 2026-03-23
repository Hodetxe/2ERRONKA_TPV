namespace TeknoBideTPV.Txata
{
    partial class TxatPantaila
    {

        private System.ComponentModel.IContainer components = null;

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 

        private void InitializeComponent()
        {
            BidaliBotoia = new Button();
            MezuIdazlea = new TextBox();
            MezuPantaila = new RichTextBox();
            SuspendLayout();

            BidaliBotoia.BackColor = Color.FromArgb(255, 243, 224);
            BidaliBotoia.Location = new Point(470, 539);
            BidaliBotoia.Name = "BidaliBotoia";
            BidaliBotoia.Size = new Size(111, 50);
            BidaliBotoia.TabIndex = 0;
            BidaliBotoia.Text = "Bidali";
            BidaliBotoia.UseVisualStyleBackColor = false;
            BidaliBotoia.Click += BidaliBotoia_Click;

            MezuIdazlea.BackColor = Color.FromArgb(255, 243, 224);
            MezuIdazlea.Location = new Point(16, 539);
            MezuIdazlea.Multiline = true;
            MezuIdazlea.Name = "MezuIdazlea";
            MezuIdazlea.Size = new Size(445, 50);
            MezuIdazlea.TabIndex = 1;
           

            MezuPantaila.BackColor = Color.FromArgb(255, 243, 224);
            MezuPantaila.ForeColor = Color.Black;
            MezuPantaila.Location = new Point(16, 16);
            MezuPantaila.Name = "MezuPantaila";
            MezuPantaila.Size = new Size(565, 505);
            MezuPantaila.TabIndex = 2;
            MezuPantaila.Text = "";
            MezuPantaila.TextChanged += MezuPantaila_TextChanged;
           
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 204, 188);
            Controls.Add(MezuPantaila);
            Controls.Add(MezuIdazlea);
            Controls.Add(BidaliBotoia);
            Name = "TxatPantaila";
            Size = new Size(600, 600);
            Load += TxatPantaila_Load_1;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BidaliBotoia;
        private TextBox MezuIdazlea;
        private RichTextBox MezuPantaila;
    }
}
