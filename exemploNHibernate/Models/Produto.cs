using exemploNHibernate.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace exemploNHibernate.Models
{
    public class Produto
    {
        public virtual long Id { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public virtual string Nome { get; set; } = string.Empty;

        [DisplayName("Quantidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public virtual int Quantidade { get; set; }

        [DisplayName("Preco")]
        [Moeda]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public virtual double Preco { get; set; }
    }
}
