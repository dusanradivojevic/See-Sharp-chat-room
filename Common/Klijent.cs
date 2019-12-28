using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Klijent
    {
        public string Nick { get; set; }
        [Browsable(false)]
        public Socket Soket { get; set; }
        [Browsable(false)]
        public NetworkStream tok { get; set; }

        public Klijent()
        {

        }

        public Klijent(string Nick)
        {
            this.Nick = Nick;
        }

        public Klijent(Socket sok)
        {
            Soket = sok;

            tok = new NetworkStream(sok);
        }

        public override bool Equals(object obj)
        {
            return ((Klijent)obj).Soket.Equals(this.Soket);
        }
    }
}
