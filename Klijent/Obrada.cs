using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Klijent
{
    public class Obrada
    {
        private Socket klijentskiSoket;
        private FrmKlijent forma;
        private BinaryFormatter formatter = new BinaryFormatter();
        private NetworkStream tok;

        public Obrada()
        {

        }

        public Obrada(Socket sok, FrmKlijent frm)
        {
            klijentskiSoket = sok;
            forma = frm;
            tok = new NetworkStream(klijentskiSoket);

            Thread nit = new Thread(IspisujPoruke)
            {
                IsBackground = true
            };
            nit.Start();
        }

        public void IspisujPoruke()
        {
            try
            {
                while (true)
                {
                    Poruka poruka = formatter.Deserialize(tok) as Poruka;
                    forma.AzurirajPoruke(poruka);
                }
            }
            catch
            {
                forma.AzurirajPoruke(new Poruka("SERVER", "Server je prestao sa radom."));
                klijentskiSoket.Close();
            }            
        }

        public void PosaljiPoruku(string tekst)
        {
            try
            {
                Poruka poruka = new Poruka(Komunikacija.Instance.nick, tekst);
                formatter.Serialize(tok, poruka);

            }
            catch
            {
                forma.AzurirajPoruke(new Poruka("SERVER", "Server je prestao sa radom."));
                klijentskiSoket.Close();
            }
        }
    }
}
