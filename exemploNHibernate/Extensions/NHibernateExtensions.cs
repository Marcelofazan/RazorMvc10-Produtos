using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Microsoft.Data.Sqlite; // Garante acesso ao SqliteConnectionStringBuilder

namespace exemploNHibernate.Extensions
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(NHibernateExtensions).Assembly.ExportedTypes);
            HbmMapping entityMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var configuration = new Configuration();
            configuration.Properties[NHibernate.Cfg.Environment.UseProxyValidator] = "false";
            configuration.Properties[NHibernate.Cfg.Environment.Hbm2ddlKeyWords] = "none";
            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<SQLiteDialect>();
                c.Driver<MicrosoftDataSqliteDriver>();
                c.ConnectionString = connectionString;

                c.LogFormattedSql = true;
                c.LogSqlInConsole = true;
            });

            configuration.AddMapping(entityMapping);

            var sessionFactory = configuration.BuildSessionFactory();

            // --- CORREÇÃO DEFINITIVA AQUI ---

            // 1. Extrai o caminho do arquivo físico a partir da connection string
            var builder = new SqliteConnectionStringBuilder(connectionString);
            string caminhoBanco = builder.DataSource;

            // 2. Se for um banco em memória (:memory:), avisa ou ignora a verificação de arquivo
            if (caminhoBanco.Equals(":memory:", StringComparison.OrdinalIgnoreCase))
            {
                var schemaExport = new SchemaExport(configuration);
                schemaExport.Execute(false, true, false);
            }
            else
            {
                // 3. Só cria a estrutura se o arquivo do banco NÃO existir na pasta
                if (!File.Exists(caminhoBanco))
                {
                    var schemaExport = new SchemaExport(configuration);
                    schemaExport.Execute(false, true, false);
                }
                else
                {
                    // Se o arquivo já existe, apenas atualiza novas colunas/tabelas (Seguro!)
                    var schemaUpdate = new SchemaUpdate(configuration);
                    schemaUpdate.Execute(false, true);
                }
            }
            // ---------------------------------

            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());

            return services;
        }
    }

    public class MicrosoftDataSqliteDriver : NHibernate.Driver.DriverBase
    {
        public override System.Data.Common.DbConnection CreateConnection()
        {
            return new Microsoft.Data.Sqlite.SqliteConnection();
        }

        public override System.Data.Common.DbCommand CreateCommand()
        {
            return new Microsoft.Data.Sqlite.SqliteCommand();
        }

        public override bool UseNamedPrefixInSql => true;
        public override bool UseNamedPrefixInParameter => true;
        public override string NamedPrefix => "$";
        public override bool SupportsMultipleOpenReaders => false;
        public new System.Data.Common.DbProviderFactory DbProviderFactory => Microsoft.Data.Sqlite.SqliteFactory.Instance;
        public override bool SupportsMultipleQueries => false;
    }
}