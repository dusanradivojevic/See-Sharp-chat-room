using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klijent
{
    public partial class FrmPrijava : Form
    {
        public FrmPrijava()
        {
            InitializeComponent();
        }

        private void btnKonektujSe_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNickname.Text))
            {
                return;
            }

            Komunikacija.Instance.PoveziSe(txtNickname.Text);

            FrmKlijent forma = new FrmKlijent();
            forma.ShowDialog();

            Dispose();
        }
    }
}
