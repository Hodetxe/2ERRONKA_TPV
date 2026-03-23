namespace TeknoBideTPV.UI
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            btn_Sartu = new Button();
            lbl_Erabiltzailea = new Label();
            lbl_Pasahitza = new Label();
            txt_Erabiltzailea = new TextBox();
            txt_Pasahitza = new TextBox();
            img_Logoa = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)img_Logoa).BeginInit();
            SuspendLayout();
            // 
            // btn_Sartu
            // 
            btn_Sartu.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_Sartu.Location = new Point(690, 541);
            btn_Sartu.Name = "btn_Sartu";
            btn_Sartu.Size = new Size(345, 96);
            btn_Sartu.TabIndex = 0;
            btn_Sartu.Text = "Sartu";
            btn_Sartu.UseVisualStyleBackColor = true;
            btn_Sartu.Click += btn_Sartu_Click;
            // 
            // lbl_Erabiltzailea
            // 
            lbl_Erabiltzailea.AutoSize = true;
            lbl_Erabiltzailea.Font = new Font("Segoe UI", 28.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Erabiltzailea.Location = new Point(396, 374);
            lbl_Erabiltzailea.Name = "lbl_Erabiltzailea";
            lbl_Erabiltzailea.Size = new Size(283, 62);
            lbl_Erabiltzailea.TabIndex = 1;
            lbl_Erabiltzailea.Text = "Erabiltzailea:";
            // 
            // lbl_Pasahitza
            // 
            lbl_Pasahitza.AutoSize = true;
            lbl_Pasahitza.Font = new Font("Segoe UI", 28.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Pasahitza.Location = new Point(396, 450);
            lbl_Pasahitza.Name = "lbl_Pasahitza";
            lbl_Pasahitza.Size = new Size(228, 62);
            lbl_Pasahitza.TabIndex = 2;
            lbl_Pasahitza.Text = "Pasahitza:";
            // 
            // txt_Erabiltzailea
            // 
            txt_Erabiltzailea.Font = new Font("Segoe UI", 28.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_Erabiltzailea.Location = new Point(672, 366);
            txt_Erabiltzailea.Name = "txt_Erabiltzailea";
            txt_Erabiltzailea.Size = new Size(381, 70);
            txt_Erabiltzailea.TabIndex = 3;
            // 
            // txt_Pasahitza
            // 
            txt_Pasahitza.Font = new Font("Segoe UI", 28.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_Pasahitza.Location = new Point(672, 442);
            txt_Pasahitza.Name = "txt_Pasahitza";
            txt_Pasahitza.PasswordChar = '*';
            txt_Pasahitza.Size = new Size(381, 70);
            txt_Pasahitza.TabIndex = 4;
            // 
            // img_Logoa
            // 
            img_Logoa.Image = (Image)resources.GetObject("img_Logoa.Image");
            img_Logoa.Location = new Point(1211, 230);
            img_Logoa.Name = "img_Logoa";
            img_Logoa.Size = new Size(512, 481);
            img_Logoa.TabIndex = 5;
            img_Logoa.TabStop = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 1033);
            Controls.Add(img_Logoa);
            Controls.Add(txt_Pasahitza);
            Controls.Add(txt_Erabiltzailea);
            Controls.Add(lbl_Pasahitza);
            Controls.Add(lbl_Erabiltzailea);
            Controls.Add(btn_Sartu);
            Name = "LoginForm";
            Text = "LoginForm";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)img_Logoa).EndInit();
            ResumeLayout(false);
            PerformLayout();

            btn_Sartu.Anchor = AnchorStyles.None;
            lbl_Erabiltzailea.Anchor = AnchorStyles.None;
            lbl_Pasahitza.Anchor = AnchorStyles.None;
            txt_Erabiltzailea.Anchor = AnchorStyles.None;
            txt_Pasahitza.Anchor = AnchorStyles.None;
            img_Logoa.Anchor = AnchorStyles.None;

            img_Logoa.SizeMode = PictureBoxSizeMode.Zoom;
        }

        #endregion

        private Button btn_Sartu;
        private Label lbl_Erabiltzailea;
        private Label lbl_Pasahitza;
        private TextBox txt_Erabiltzailea;
        private TextBox txt_Pasahitza;
        private PictureBox img_Logoa;
    }
}