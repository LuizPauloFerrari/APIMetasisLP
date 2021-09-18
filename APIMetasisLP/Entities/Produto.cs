using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIMetasisLP.Entities
{
    public class Produto
    {
       // [Key]
        public int ProdutoId { get; set; }

        public string Descricao { get; set; }
        public double Preco { get; set; }

    }
}
