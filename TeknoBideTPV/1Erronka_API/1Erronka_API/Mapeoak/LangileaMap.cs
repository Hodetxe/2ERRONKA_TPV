using FluentNHibernate.Mapping;
using _1Erronka_API.Domain;

namespace _1Erronka_API.Mapeoak
{
    public class LangileaMap : ClassMap<Langilea>
    {
        public LangileaMap()
        {
            Table("langileak");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Izena).Column("izena");
            Map(x => x.Abizena).Column("abizena");
            Map(x => x.Erabiltzaile_izena).Column("erabiltzaile_izena");
            Map(x => x.Langile_kodea).Column("langile_kodea");
            Map(x => x.Pasahitza).Column("pasahitza");
            Map(x => x.Rola_id).Column("rola_id");
            Map(x => x.Ezabatua).Column("ezabatua");
            Map(x => x.Chat).Column("chat");
        }
    }
}
