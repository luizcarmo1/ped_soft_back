using Api_Pedidos.Models;


namespace Api_Pedidos.DTO
{
    public class PedidosDTO
    {
        public int QtdProduto { get; set; }
        public DateTime Data_entrada_pedido { get; set; }
        public string FormaPagamento { get; set; }
        public string StatusPedido { get; set; }

        public int ClienteID { get; set; }

        public int[] ProductsId { get; set; }
    }
}
