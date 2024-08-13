using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_Pedidos.Models
{
    public class PedidoProduto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PedidoID { get; set; }
        public Produto Produto { get; set; }
        public double Quantidade { get; set; }
    }
}
