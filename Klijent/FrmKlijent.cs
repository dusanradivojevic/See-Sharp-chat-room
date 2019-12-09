using Common;
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
    public partial class FrmKlijent : Form
    {
        Obrada ob;
        public FrmKlijent()
        {
            InitializeComponent();

            //Komunikacija.Instance.Forma = this;
            ob = new Obrada(Komunikacija.Instance.klijentskiSoket, this);
        }

        public void AzurirajPoruke(Poruka poruka)
        {
            Invoke(new Action(() =>
            {
                rtbChat.AppendText(poruka.ToString() + "\n");
            }));
        }

        private void btnPosalji_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPoruka.Text))
                return;

            ob.PosaljiPoruku(txtPoruka.Text);
        }

    }
}
