using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public static class TPVEstiloa
{
    private class KontrolenInformazioa
    {
        public Rectangle Bounds;
        public float FontSize;
    }

    private class FormInfo
    {
        public Size HasierakoTamaina;
        public Dictionary<Control, KontrolenInformazioa> Kontrolak;
    }

    private static readonly Dictionary<Form, FormInfo> hasierakoak = new();

    public static void PantailarenEskalatuaHasi(Form form)
    {
        if (hasierakoak.ContainsKey(form))
            return;

        var dic = new Dictionary<Control, KontrolenInformazioa>();
        HasierakoPosizioakGorde(form.Controls, dic);

        hasierakoak[form] = new FormInfo
        {
            HasierakoTamaina = form.ClientSize, 
            Kontrolak = dic
        };
    }

    private static void HasierakoPosizioakGorde(
        Control.ControlCollection controls,
        Dictionary<Control, KontrolenInformazioa> dic)
    {
        foreach (Control c in controls)
        {
            dic[c] = new KontrolenInformazioa
            {
                Bounds = c.Bounds,
                FontSize = c.Font.Size
            };

            if (c.HasChildren)
                HasierakoPosizioakGorde(c.Controls, dic);
        }
    }

    public static void EskalatuaAplikatu(Form form)
    {
        if (!hasierakoak.TryGetValue(form, out var info))
            return;

        var dic = info.Kontrolak;
        float hasieraZabalera = info.HasierakoTamaina.Width;
        float hasieraAltuera = info.HasierakoTamaina.Height;

        if (hasieraZabalera <= 0 || hasieraAltuera <= 0)
            return;

        float Xeskala = (float)form.ClientSize.Width / hasieraZabalera;
        float Yeskala = (float)form.ClientSize.Height / hasieraAltuera;
        float eskala = Math.Min(Xeskala, Yeskala);

        if (eskala < 0.7f) eskala = 0.7f;

        foreach (var kvp in dic)
        {
            var c = kvp.Key;
            var kInfo = kvp.Value;

            c.SetBounds(
                (int)(kInfo.Bounds.X * eskala),
                (int)(kInfo.Bounds.Y * eskala),
                (int)(kInfo.Bounds.Width * eskala),
                (int)(kInfo.Bounds.Height * eskala)
            );

            c.Font = new Font(
                c.Font.FontFamily,
                kInfo.FontSize * eskala,
                c.Font.Style
            );
        }
    }

    public static class Koloreak
    {
        public static readonly Color Primary = ColorTranslator.FromHtml("#6366F1");
        public static readonly Color Secondary = ColorTranslator.FromHtml("#4E8EF7");
        public static readonly Color PrimaryDark = ColorTranslator.FromHtml("#4338CA");
        public static readonly Color PrimaryHover = ColorTranslator.FromHtml("#4F46E5");

        public static readonly Color Background = ColorTranslator.FromHtml("#F9FAFB");
        public static readonly Color White = ColorTranslator.FromHtml("#FFFFFF");
        public static readonly Color Border = ColorTranslator.FromHtml("#E5E7EB");

        public static readonly Color TextTitle = ColorTranslator.FromHtml("#0F172A");
        public static readonly Color TextPrimary = ColorTranslator.FromHtml("#1E293B");
        public static readonly Color TextSecondary = ColorTranslator.FromHtml("#475569");
        public static readonly Color TextNormal = ColorTranslator.FromHtml("#1E293B");

        public static readonly Color Error = ColorTranslator.FromHtml("#EF4444");
        public static readonly Color ErrorHover = ColorTranslator.FromHtml("#DC2626");
        public static readonly Color Baieztatu = ColorTranslator.FromHtml("#22C55E");
        public static readonly Color BaieztatuAtzeko = ColorTranslator.FromHtml("#DCFCE7");

        public static readonly Color LoginGradienteaArgia = ColorTranslator.FromHtml("#4F8DF7");
        public static readonly Color LoginGradienteaIluna = ColorTranslator.FromHtml("#4338CA");
    }
}
