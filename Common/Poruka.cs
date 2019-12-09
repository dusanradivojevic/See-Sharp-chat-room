using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class Poruka
    {
        public string ImeKlijenta { get; set; }
        public string TekstPoruke { get; set; }

        public Poruka()
        {

        }

        public Poruka(string ime, string tekst)
        {
            ImeKlijenta = ime;
            TekstPoruke = tekst;
        }

        public override string ToString()
        {
            return ImeKlijenta + ": " + TekstPoruke;
        }
    }
}
