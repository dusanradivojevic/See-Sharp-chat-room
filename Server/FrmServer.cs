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

namespace Server
{
    public partial class FrmServer : Form
    {
        Server server;
        BindingList<Klijent> klijenti;
        public FrmServer()
        {
            InitializeComponent();
            server = new Server(this);
            btnZaustavi.Enabled = false;

            klijenti = new BindingList<Klijent>();
            dgvKlijenti.DataSource = klijenti;
            //dgvKlijenti.Columns[0].HeaderText = "Nick";
        }

        public void IspisiPoruku(string tekst)
        {
            Invoke(new Action(() =>
            {
                rtbStatus.AppendText(tekst + "\n");
            }));
        }

        private void btnPokreni_Click(object sender, EventArgs e)
        {
            if (server.PokreniServer())
            {
                btnZaustavi.Enabled = true;
                btnPokreni.Enabled = false;
            }
        }

        private void btnZaustavi_Click(object sender, EventArgs e)
        {
            if (server.ZaustaviServer())
            {
                btnZaustavi.Enabled = false;
                btnPokreni.Enabled = true;
            }
        }

        public void RefresujDgv()
        {
            Invoke(new Action(() => {
                dgvKlijenti.Refresh();
            }));
            
        }

        internal void ObrisiKlijenta(Klijent k)
        {
            Invoke(new Action(() =>
            {
                klijenti.Remove(k);
            }));
        }

        internal void ObrisiListu()
        {
            Invoke(new Action(() =>
            {
                klijenti.Clear();
            }));
        }

        internal void DodajKlijenta(Klijent k)
        {
            Invoke(new Action(() =>
            {
                klijenti.Add(k);
            }));
        }

        private void btnIzbaci_Click(object sender, EventArgs e)
        {
            Klijent k = dgvKlijenti.CurrentRow.DataBoundItem as Klijent;
            server.ZatvoriKlijenta(k);
        }
    }
}
