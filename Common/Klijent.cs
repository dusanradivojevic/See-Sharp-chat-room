using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Klijent
    {
        public int Id { get; set; }
        public string Nick { get; set; }
        public Socket Soket { get; set; }
        public NetworkStream tok { get; set; }

        public Klijent()
        {

        }

        public Klijent(int Id, string Nick)
        {
            this.Id = Id;
            this.Nick = Nick;
        }

        public Klijent(int Id, Socket sok)
        {
            this.Id = Id;
            Soket = sok;

            tok = new NetworkStream(sok);
        }
    }
}
