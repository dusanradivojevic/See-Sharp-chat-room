using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        public List<Klijent> listaKlijenata;
        private Socket osluskujuciSoket;
        private BinaryFormatter formatter = new BinaryFormatter();
        private FrmServer forma;
        
        public Server(FrmServer f)
        {
            listaKlijenata = new List<Klijent>();
            forma = f;
        }

        public bool PokreniServer()
        {
            try
            {
                osluskujuciSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                        ProtocolType.Tcp);
                osluskujuciSoket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"),
                    17510));
                osluskujuciSoket.Listen(5);

                forma.IspisiPoruku("Server je pokrenut!");

                Thread nit = new Thread(Osluskuj);
                nit.IsBackground = true;
                nit.Start();

                return true;
            }
            catch (Exception e)
            {
                forma.IspisiPoruku("Neuspesno pokretanje servera. >>> " + e.Message);
                return false;
            }
        }

        private void Osluskuj()
        {
            forma.IspisiPoruku("Server ceka na povezivanje klijenata.");
            while (true)
            {
                try
                {
                    Socket klijentSoket = osluskujuciSoket.Accept();

                    Klijent kl = new Klijent(klijentSoket);
                    byte[] nazivOdKlijenta = new byte[100];
                    klijentSoket.Receive(nazivOdKlijenta);
                    kl.Nick = Encoding.ASCII.GetString(nazivOdKlijenta);

                    listaKlijenata.Add(kl);
                    forma.DodajKlijenta(kl);

                    Thread nit = new Thread(() =>
                    {
                        PrimajPoruke(kl);
                    });
                    nit.IsBackground = true;
                    nit.Start();

                    forma.IspisiPoruku($"{kl.Nick} se povezao.");                    
                }
                catch (Exception)
                {
                    if(osluskujuciSoket.Connected)
                        forma.IspisiPoruku("Neuspesno povezivanje klijenta na server.");
                }
            }
        }

        public void PrimajPoruke(Klijent k)
        {
            NetworkStream tok = new NetworkStream(k.Soket);

            try
            {
                while (true)
                {
                    Poruka p = formatter.Deserialize(tok) as Poruka;

                    //Console.WriteLine(p.ToString());

                    SaljiPoruke(p);
                }
            }
            catch
            {
                ZatvoriKlijenta(k);
            }
        }

        public void ZatvoriKlijenta(Klijent k)
        {
            foreach(Klijent klijent in listaKlijenata)
            {
                if (klijent.Equals(k))
                {
                    try
                    {
                        klijent.Soket.Close();
                        listaKlijenata.Remove(klijent);
                        forma.ObrisiKlijenta(klijent);
                    }
                    catch
                    {

                    }
                    break;
                }
            }
        }

        public void SaljiPoruke(Poruka p)
        {
            foreach (Klijent kl in listaKlijenata)
            {
                formatter.Serialize(kl.tok, p);
            }
        }

        public bool ZaustaviServer()
        {
            try
            {
                osluskujuciSoket.Close();

                foreach(Klijent k in listaKlijenata)
                {
                    k.Soket.Close();

                }

                listaKlijenata.Clear();
                forma.ObrisiListu();

                forma.IspisiPoruku("Server je zaustavljen!");
                return true;
            }
            catch
            {
                forma.IspisiPoruku("Doslo je do greske prilikom zaustavljanja servera.");
                return false;
            }
        }
    }

}
