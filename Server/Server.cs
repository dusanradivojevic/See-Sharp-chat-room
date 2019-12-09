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
        private List<Klijent> klijenti;
        private Socket osluskujuciSoket;
        private BinaryFormatter formatter = new BinaryFormatter();

        public static void Main()
        {
            Server ser = new Server();
            ser.PokreniServer();
        }

        public Server()
        {
            klijenti = new List<Klijent>();
        }

        public void PokreniServer()
        {
            try
            {
                osluskujuciSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                        ProtocolType.Tcp);
                osluskujuciSoket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"),
                    17510));
                osluskujuciSoket.Listen(5);

                Console.WriteLine("Server je pokrenut!");
                Osluskuj();
            }
            catch (Exception e)
            {
                Console.WriteLine("Neuspesno pokretanje servera. >>> " + e.Message);

            }
        }

        private void Osluskuj()
        {
            Console.WriteLine("Server ceka na povezivanje PRVOG klijenta.");
            while (true)
            {
                try
                {
                    Socket klijentSoket = osluskujuciSoket.Accept();

                    Klijent kl = new Klijent(klijenti.Count + 1, klijentSoket);
                    byte[] nazivOdKlijenta = new byte[100];
                    klijentSoket.Receive(nazivOdKlijenta);
                    kl.Nick = Encoding.ASCII.GetString(nazivOdKlijenta);

                    klijenti.Add(kl);

                    Thread nit = new Thread(() =>
                    {
                        PrimajPoruke(new NetworkStream(klijentSoket));
                    });
                    nit.IsBackground = true;
                    nit.Start();

                    Console.WriteLine("Klijent je povezan." + kl.Nick);                    
                }
                catch (Exception)
                {
                    Console.WriteLine("Neuspesno povezivanje klijenta na server.");
                }
            }
        }

        public void PrimajPoruke(NetworkStream tok)
        {
            while (true)
            {
                Poruka p = formatter.Deserialize(tok) as Poruka;

                //Console.WriteLine(p.ToString());

                SaljiPoruke(p);
            }
        }

        public void SaljiPoruke(Poruka p)
        {
            foreach (Klijent kl in klijenti)
            {
                formatter.Serialize(kl.tok, p);
            }
        }
    }

}
