using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using _1Erronka_API.Mapeoak;
using _1Erronka_API.Modeloak;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace _1Erronka_API
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static NHibernate.ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static ISessionFactory SessionFactory =>
            _sessionFactory ??= CreateSessionFactory();

        private static ISessionFactory CreateSessionFactory()
        {
            var config = Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                //.ConnectionString("Server=192.168.10.5;Port=3306;Database=2mg3_1erronka;Uid=3Taldea;Pwd=2MG3_3Taldea3;"))
                .ConnectionString("Server=localhost;Port=3306;Database=2erronka;Uid=root;Pwd=1MG2024;"))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssembly(typeof(NHibernateHelper).Assembly);
                })
                .ExposeConfiguration(cfg =>
                {
                    cfg.SetProperty("current_session_context_class", "async_local");
                })
                .BuildConfiguration();

            dbEguneratu(config);

                return config.BuildSessionFactory();
        }

        public static void dbEguneratu(NHibernate.Cfg.Configuration config)
        {
            SchemaUpdate schemaUpdate = new SchemaUpdate(config);
            schemaUpdate.Execute(false, false);
        }       
    }
}
