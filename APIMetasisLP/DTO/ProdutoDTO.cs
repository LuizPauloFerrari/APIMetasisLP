using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMetasisLP.DTO
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }
        public string Descricao { get; set; }
        public double PrecoIni { get; set; }
        public double PrecoFim { get; set; }
    }
}
