using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeknoBideTPV.UI.Styles;

namespace TeknoBideTPV.UI.Controls
{
    public partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            this.BackColor = TPVEstiloaFinkoa.Koloreak.Primary;
            this.ForeColor = TPVEstiloaFinkoa.Koloreak.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var pen = new Pen(Color.FromArgb(60, 0, 0, 0), 1);
            e.Graphics.DrawLine(pen, 0, this.Height - 1, this.Width, this.Height - 1);
        }

        public string Titulo
        {
            get => lbl_Izenburua.Text;
            set => lbl_Izenburua.Text = value;
        }

        public string Izena
        {
            get => lbl_Izena.Text;
            set => lbl_Izena.Text = value;
        }

        public string Erabiltzailea
        {
            get => lbl_Erabiltzailea.Text;
            set => lbl_Erabiltzailea.Text = value;
        }

        public string DataOrdua
        {
            get => lbl_DataOrdua.Text;
            set => lbl_DataOrdua.Text = value;
        }

        private void lbl_DataOrdua_Click(object sender, EventArgs e)
        {

        }
    }
}
