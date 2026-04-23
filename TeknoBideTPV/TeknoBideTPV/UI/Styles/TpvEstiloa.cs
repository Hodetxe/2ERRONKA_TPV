using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

    public static void ProfesionalizatuKontrolak(Control root)
    {
        if (root is Form f)
        {
            if (f.Font == null || !string.Equals(f.Font.FontFamily.Name, "Segoe UI", StringComparison.OrdinalIgnoreCase))
            {
                f.Font = new Font("Segoe UI", f.Font?.Size ?? 10F, f.Font?.Style ?? FontStyle.Regular);
            }

            if (f.BackColor == SystemColors.Control)
                f.BackColor = Koloreak.Background;
        }

        ProfesionalizatuKontrola(root);
        if (root.HasChildren)
            ProfesionalizatuZuhaitza(root.Controls);
    }

    private static void ProfesionalizatuZuhaitza(Control.ControlCollection controls)
    {
        foreach (Control c in controls)
        {
            ProfesionalizatuKontrola(c);
            if (c.HasChildren)
                ProfesionalizatuZuhaitza(c.Controls);
        }
    }

    private static void ProfesionalizatuKontrola(Control c)
    {
        if (c is Label lbl)
        {
            if (lbl.ForeColor == SystemColors.ControlText)
                lbl.ForeColor = Koloreak.TextPrimary;
            return;
        }

        if (c is Button btn)
        {
            if (btn.FlatStyle != FlatStyle.Flat)
                btn.FlatStyle = FlatStyle.Flat;

            btn.UseVisualStyleBackColor = false;

            if (btn.FlatAppearance.BorderSize == 0 && btn.BackColor == Koloreak.White)
            {
                btn.FlatAppearance.BorderSize = 1;
                if (btn.FlatAppearance.BorderColor.IsEmpty)
                    btn.FlatAppearance.BorderColor = Koloreak.Border;
            }

            if (btn.FlatAppearance.MouseOverBackColor.IsEmpty)
                btn.FlatAppearance.MouseOverBackColor = Ilundu(btn.BackColor, 0.08f);

            if (btn.FlatAppearance.MouseDownBackColor.IsEmpty)
                btn.FlatAppearance.MouseDownBackColor = Ilundu(btn.BackColor, 0.14f);

            BiribilduBotoia(btn);
            return;
        }

        if (c is TextBoxBase tb)
        {
            if (tb.BorderStyle == BorderStyle.None)
                tb.BorderStyle = BorderStyle.FixedSingle;

            if (tb.BackColor == SystemColors.Window || tb.BackColor == SystemColors.Control)
                tb.BackColor = Koloreak.White;

            if (tb.ForeColor == SystemColors.WindowText || tb.ForeColor == SystemColors.ControlText)
                tb.ForeColor = Koloreak.TextPrimary;

            return;
        }

        if (c is ComboBox cb)
        {
            if (cb.FlatStyle != FlatStyle.Flat)
                cb.FlatStyle = FlatStyle.Flat;

            if (cb.BackColor == SystemColors.Window || cb.BackColor == SystemColors.Control)
                cb.BackColor = Koloreak.White;

            if (cb.ForeColor == SystemColors.WindowText || cb.ForeColor == SystemColors.ControlText)
                cb.ForeColor = Koloreak.TextPrimary;

            return;
        }

        if (c is NumericUpDown nud)
        {
            if (nud.BackColor == SystemColors.Window || nud.BackColor == SystemColors.Control)
                nud.BackColor = Koloreak.White;

            if (nud.ForeColor == SystemColors.WindowText || nud.ForeColor == SystemColors.ControlText)
                nud.ForeColor = Koloreak.TextPrimary;

            return;
        }

        if (c is DateTimePicker dtp)
        {
            if (dtp.CalendarMonthBackground == SystemColors.Window || dtp.CalendarMonthBackground == SystemColors.Control)
                dtp.CalendarMonthBackground = Koloreak.White;

            if (dtp.CalendarForeColor == SystemColors.WindowText || dtp.CalendarForeColor == SystemColors.ControlText)
                dtp.CalendarForeColor = Koloreak.TextPrimary;

            return;
        }

        if (c is DataGridView dgv)
        {
            EstilatuDataGridView(dgv);
            return;
        }

        if (c is Panel or FlowLayoutPanel or TableLayoutPanel)
        {
            if (c.BackColor == SystemColors.Control)
                c.BackColor = Koloreak.Background;

            if (c.BackColor == Koloreak.White)
            {
                if (c is Panel pnl && pnl.BorderStyle == BorderStyle.FixedSingle)
                    pnl.BorderStyle = BorderStyle.None;

                BiribilduTxartela(c);
                TxartelBordeaMarraztu(c);
            }
        }
    }

    public static void EstilatuDataGridView(DataGridView dgv)
    {
        dgv.EnableHeadersVisualStyles = false;
        dgv.BackgroundColor = Koloreak.Background;
        dgv.BorderStyle = BorderStyle.None;
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgv.GridColor = Koloreak.Border;
        dgv.RowHeadersVisible = false;
        dgv.SelectionMode = dgv.SelectionMode == DataGridViewSelectionMode.CellSelect
            ? DataGridViewSelectionMode.FullRowSelect
            : dgv.SelectionMode;

        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Koloreak.Primary;
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Koloreak.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", Math.Max(10F, dgv.Font.Size), FontStyle.Bold);
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 0, 12, 0);
        if (dgv.ColumnHeadersHeight < 44)
            dgv.ColumnHeadersHeight = 44;

        dgv.DefaultCellStyle.BackColor = Koloreak.White;
        dgv.DefaultCellStyle.ForeColor = Koloreak.TextPrimary;
        dgv.DefaultCellStyle.Font = new Font("Segoe UI", Math.Max(10F, dgv.Font.Size), FontStyle.Regular);
        dgv.DefaultCellStyle.SelectionBackColor = Koloreak.Secondary;
        dgv.DefaultCellStyle.SelectionForeColor = Koloreak.White;
        dgv.DefaultCellStyle.Padding = new Padding(12, 0, 12, 0);

        dgv.AlternatingRowsDefaultCellStyle.BackColor = Argitu(Koloreak.Background, 0.55f);

        if (dgv.RowTemplate.Height < 44)
            dgv.RowTemplate.Height = 44;

        foreach (DataGridViewColumn col in dgv.Columns)
        {
            if (col is DataGridViewButtonColumn)
            {
                col.DefaultCellStyle.BackColor = Koloreak.Primary;
                col.DefaultCellStyle.ForeColor = Koloreak.White;
                col.DefaultCellStyle.SelectionBackColor = Koloreak.PrimaryHover;
                col.DefaultCellStyle.SelectionForeColor = Koloreak.White;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
    }

    private static void BiribilduBotoia(Button btn)
    {
        btn.Resize -= Botoia_ResizeBiribildua;
        btn.Resize += Botoia_ResizeBiribildua;
        EzarriEskualdeaBiribildua(btn, BotoiErradioa(btn));
    }

    private static int BotoiErradioa(Button btn)
    {
        int h = btn.Height;
        if (h >= 72) return 18;
        if (h >= 56) return 16;
        if (h >= 44) return 14;
        return 12;
    }

    private static void Botoia_ResizeBiribildua(object sender, EventArgs e)
    {
        if (sender is Button btn)
            EzarriEskualdeaBiribildua(btn, BotoiErradioa(btn));
    }

    private static void BiribilduTxartela(Control c)
    {
        c.Resize -= Txartela_ResizeBiribildua;
        c.Resize += Txartela_ResizeBiribildua;
        EzarriEskualdeaBiribildua(c, 16);
    }

    private static void Txartela_ResizeBiribildua(object sender, EventArgs e)
    {
        if (sender is Control c)
            EzarriEskualdeaBiribildua(c, 16);
    }

    private static void TxartelBordeaMarraztu(Control c)
    {
        c.Paint -= Txartela_PaintBordea;
        c.Paint += Txartela_PaintBordea;
        c.Invalidate();
    }

    private static void Txartela_PaintBordea(object sender, PaintEventArgs e)
    {
        if (sender is not Control c)
            return;

        if (c.Width <= 2 || c.Height <= 2)
            return;

        if (c.BackColor != Koloreak.White)
            return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var rect = new Rectangle(1, 1, c.Width - 2, c.Height - 2);

        using var path = BiribilduaPath(rect, 16);
        using var pen = new Pen(Koloreak.Border, 1);
        e.Graphics.DrawPath(pen, path);
    }

    private static void EzarriEskualdeaBiribildua(Control c, int radius)
    {
        if (c.Width <= 2 || c.Height <= 2)
            return;

        if (radius < 2)
        {
            c.Region = null;
            return;
        }

        var rect = new Rectangle(0, 0, c.Width, c.Height);
        using var path = BiribilduaPath(rect, radius);
        c.Region = new Region(path);
    }

    private static GraphicsPath BiribilduaPath(Rectangle rect, int radius)
    {
        int d = radius * 2;
        var path = new GraphicsPath();

        path.StartFigure();
        path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();

        return path;
    }

    private static Color Ilundu(Color c, float amount)
    {
        return Nahastu(c, Color.Black, amount);
    }

    private static Color Argitu(Color c, float amount)
    {
        return Nahastu(c, Color.White, amount);
    }

    private static Color Nahastu(Color a, Color b, float t)
    {
        if (t < 0f) t = 0f;
        if (t > 1f) t = 1f;

        int r = (int)(a.R + (b.R - a.R) * t);
        int g = (int)(a.G + (b.G - a.G) * t);
        int bl = (int)(a.B + (b.B - a.B) * t);
        return Color.FromArgb(a.A, r, g, bl);
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
