using System;

using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Emboard
{
    class Sdk
    {
        public static Bitmap ToGray(Bitmap bm)
        {
            Bitmap bitmap = new Bitmap(bm);
            int x, y;
            Color c;
            Byte gray;
            for (y = 0; y < bm.Height - 1; y++)
            {
                for (x = 0; x < bm.Width - 1; x++)
                {
                    c = bm.GetPixel(x, y);
                    gray = Convert.ToByte(c.R * 0.287 + c.G * 0.599 + c.B * 0.114);
                    bitmap.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            return bitmap;
        }
    }
}
