using exemploNHibernate.Models;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernate;

namespace exemploNHibernate.Mappings
{
    public class ProdutoMap : ClassMapping<Produto>
    {
        public ProdutoMap()
        {

            Id(x => x.Id, x =>
            {
                x.Generator(Generators.Increment);
                x.Type(NHibernateUtil.Int64);
                x.Column("Id");
            });

            Property(b => b.Nome, x =>
            {
                x.Length(520);
                x.Type(NHibernateUtil.String);
                x.NotNullable(true);
            });

            Property(b => b.Quantidade, x =>
            {
                x.Type(NHibernateUtil.Int32);
                x.NotNullable(true);
            });

            Property(b => b.Preco, x =>
            {
                x.Type(NHibernateUtil.Double);
                x.Scale(2);
                x.Precision(15);
                x.NotNullable(true);
            });

            Table("Produtos");
        }
    }
}
