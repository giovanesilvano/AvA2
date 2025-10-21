using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuilhermeDuduBerti
{
    public class Cliente
    {

        public int id { get; set; }
        public int cpf { get; set; }
        public int mes { get; set; }
        public int ano { get; set; }
        public double m3Consumidos { get; set; }
        public string bandeira { get; set; } = string.Empty;
        public bool possuiEsgoto { get; set; }
    }
}