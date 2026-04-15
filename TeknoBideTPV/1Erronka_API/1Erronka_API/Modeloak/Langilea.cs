namespace _1Erronka_API.Domain
{
    public class Langilea
    {
        public virtual int Id { get; set; }
        public virtual string Izena { get; set; } = string.Empty;
        public virtual string Abizena { get; set; } = string.Empty;
        public virtual string Erabiltzaile_izena { get; set; }
        public virtual int Langile_kodea { get; set; }
        public virtual string Pasahitza { get; set; } = string.Empty;
        public virtual int Rola_id { get; set; }
        public virtual bool Ezabatua { get; set; }
        public virtual bool Chat { get; set; }
    }
}
