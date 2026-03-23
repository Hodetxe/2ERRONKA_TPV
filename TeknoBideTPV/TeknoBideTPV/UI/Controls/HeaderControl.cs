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
