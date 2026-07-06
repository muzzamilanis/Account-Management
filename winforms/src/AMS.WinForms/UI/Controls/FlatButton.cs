using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AMS.WinForms.UI.Themes;

namespace AMS.WinForms.UI.Controls
{
    public class FlatButton : Button
    {
        public int BorderRadius { get; set; } = 4;
        public bool IsPrimary { get; set; } = true;
        public Color HoverColor { get; set; }

        private bool _isHovered = false;

        public FlatButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Cursor = Cursors.Hand;
            this.Font = ThemeManager.BodyFont;
            
            this.BackColor = ThemeManager.AccentColor;
            this.ForeColor = Color.White;
            this.HoverColor = ThemeManager.AccentHover;
            
            this.MouseEnter += (s, e) => { _isHovered = true; this.Invalidate(); };
            this.MouseLeave += (s, e) => { _isHovered = false; this.Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using (GraphicsPath path = ThemeManager.GetRoundedPath(rect, BorderRadius))
            {
                Color bgColor = _isHovered ? HoverColor : this.BackColor;
                using (SolidBrush brush = new SolidBrush(bgColor))
                {
                    pevent.Graphics.FillPath(brush, path);
                }
                
                if (!IsPrimary)
                {
                    using (Pen pen = new Pen(ThemeManager.BorderColor, 1))
                    {
                        pevent.Graphics.DrawPath(pen, path);
                    }
                }
            }

            // Draw Text
            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, rect, this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
