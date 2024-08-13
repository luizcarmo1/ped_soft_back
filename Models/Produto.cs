using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Api_Pedidos.Models
{
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProdutoID { get; set; }

        [Required]
        public string NomeProduto { get; set; }

        [Required]
        public double Preco { get; set; }
    }
}
