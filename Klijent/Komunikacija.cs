using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Klijent
{
    public class Komunikacija
    {
        public string nick;
        private static Komunikacija _instance;
        public Socket klijentskiSoket;

        private Komunikacija()
        {

        }
        public static Komunikacija Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Komunikacija();
                }

                return _instance;
            }
        }

        public void PoveziSe(string nickname)
        {
            try
            {
                klijentskiSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                        ProtocolType.Tcp);
                klijentskiSoket.Connect("127.0.0.1", 17510);

                klijentskiSoket.Send(Encoding.ASCII.GetBytes(nickname));
                nick = nickname;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Greska u komunikaciji. " + e.Message);
            }
        }

        
    }
}
