using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TeknoBideTPV.UI.Styles
{
    public static class TPVEstiloaFinkoa
    {
        public static class Koloreak
        {
            public static Color Primary = Color.FromArgb(255, 140, 0); 
            public static Color Secondary = Color.FromArgb(255, 200, 100);
            public static Color Background = Color.FromArgb(253, 245, 230);
            public static Color TextTitle = Color.FromArgb(50, 50, 50);
            public static Color White = Color.White;
        }

        private class KontrolInfo
        {
            public Rectangle Bounds;
            public float FontSize;
            public int RowHeight;
            public int ColumnHeadersHeight;
            public Dictionary<string, int> ColumnWidths = new(); 
            public Padding Padding;
            public Padding Margin;
        }

        private static readonly Dictionary<Form, Dictionary<Control, KontrolInfo>> originalak = new();

        public static void Prestatu(Form form)
        {
            if (originalak.ContainsKey(form))
                return;

            var dic = new Dictionary<Control, KontrolInfo>();
            Gorde(form.Controls, dic);
            originalak[form] = dic;
        }

        public static void EguneratuKontrola(Control c, Form form)
        {
            if (!originalak.TryGetValue(form, out var dic))
            {
                dic = new Dictionary<Control, KontrolInfo>();
                originalak[form] = dic;
            }

            GordeBat(c, dic);

            AplikatuKontrola(c, form, dic[c]);
        }

        private static void Gorde(Control.ControlCollection controls, Dictionary<Control, KontrolInfo> dic)
        {
            foreach (Control c in controls)
            {
                GordeBat(c, dic);
            }
        }

        private static void GordeBat(Control c, Dictionary<Control, KontrolInfo> dic)
        {
            var info = new KontrolInfo
            {
                Bounds = c.Bounds,
                FontSize = c.Font.Size,
                Padding = c.Padding,
                Margin = c.Margin
            };

            if (c is DataGridView dgv)
            {
                info.RowHeight = dgv.RowTemplate.Height;
                info.ColumnHeadersHeight = dgv.ColumnHeadersHeight;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    info.ColumnWidths[col.Name] = col.Width;
                }
            }

            dic[c] = info;

            if (c.HasChildren)
                Gorde(c.Controls, dic);
        }

        public static void Aplikatu(Form form)
        {
            if (!originalak.TryGetValue(form, out var dic))
                return;

            foreach (var kvp in dic)
            {
                AplikatuKontrola(kvp.Key, form, kvp.Value);
            }
        }

        private static void AplikatuKontrola(Control c, Form form, KontrolInfo info)
        {
            float baseW = 1920f;
            float baseH = 1080f;

            Screen screen = Screen.FromControl(form);
            if (screen == null) screen = Screen.PrimaryScreen;

            float scaleX = (float)screen.Bounds.Width / baseW;
            float scaleY = (float)screen.Bounds.Height / baseH;

            if (c.Dock == DockStyle.None)
            {
                c.SetBounds(
                    (int)(info.Bounds.X * scaleX),
                    (int)(info.Bounds.Y * scaleY),
                    (int)(info.Bounds.Width * scaleX),
                    (int)(info.Bounds.Height * scaleY)
                );
            }
            else if (c.Dock == DockStyle.Top || c.Dock == DockStyle.Bottom)
            {
                c.Height = (int)(info.Bounds.Height * scaleY);
            }
            else if (c.Dock == DockStyle.Left || c.Dock == DockStyle.Right)
            {
                c.Width = (int)(info.Bounds.Width * scaleX);
            }

            try
            {
                c.Padding = new Padding(
                    (int)(info.Padding.Left * scaleX),
                    (int)(info.Padding.Top * scaleY),
                    (int)(info.Padding.Right * scaleX),
                    (int)(info.Padding.Bottom * scaleY)
                );

                c.Margin = new Padding(
                    (int)(info.Margin.Left * scaleX),
                    (int)(info.Margin.Top * scaleY),
                    (int)(info.Margin.Right * scaleX),
                    (int)(info.Margin.Bottom * scaleY)
                );
            }
            catch { }

            if (info.FontSize > 0)
            {
                try
                {
                    c.Font = new Font(
                        c.Font.FontFamily,
                        info.FontSize * scaleY,
                        c.Font.Style
                    );
                }
                catch { /* Ignore font errors */ }
            }

            if (c is DataGridView dgv)
            {
                if (info.RowHeight > 0)
                {
                    int rowHeight = (int)(info.RowHeight * scaleY);
                    if (rowHeight < 36)
                        rowHeight = 36;
                    dgv.RowTemplate.Height = rowHeight;
                }

                if (info.ColumnHeadersHeight > 0)
                {
                    int headerHeight = (int)(info.ColumnHeadersHeight * scaleY);
                    if (headerHeight < 38)
                        headerHeight = 38;
                    dgv.ColumnHeadersHeight = headerHeight;
                }

                if (dgv.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.Fill)
                {
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (info.ColumnWidths.TryGetValue(col.Name, out int originalWidth))
                        {
                            col.Width = (int)(originalWidth * scaleX);
                        }
                    }
                }
            }
        }
    }
}
