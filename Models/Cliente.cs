using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Api_Pedidos.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClienteID { get; set; }

       
        public string NomeCliente { get; set; }

        [Required]
        public string CPF { get; set; }

        public string Telefone { get; set; }

        public string Endereco { get; set; }
    }
}
