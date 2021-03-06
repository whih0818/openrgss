﻿using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace OpenRGSS.Runtime.RGSS
{
    public class Bitmap
    {
        private static string[] ImageExtension = new string[] { ".png", ".jpg" };

        public Rect _rect;
        public Font font = new Font();
        private System.Drawing.Bitmap data;
        private int _textureId = -1;

        public int width
        {
            get
            {
                return this.rect.width;
            }
        }

        public int height
        {
            get
            {
                return this.rect.height;
            }
        }

        public Rect rect
        {
            get
            {
                return _rect;
            }
        }

        public int TextureId
        {
            get
            {
                return _textureId;
            }
        }

        public Bitmap(string filename)
        {
            this.data = new System.Drawing.Bitmap(this.FindFile(filename));
            this._rect = new Rect(0, 0, this.data.Width, this.data.Height);
            this.GenTexture();
        }

        public Bitmap(int width, int height)
        {
            this.data = new System.Drawing.Bitmap(width, height);

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(this.data))
            {
                g.Clear(System.Drawing.Color.Transparent);
            }

            this._rect = new Rect(0, 0, width, height);
            this.GenTexture();
        }

        private string FindFile(string filename)
        {
            foreach(string ext in ImageExtension) {
                if (System.IO.File.Exists(filename + ext))
                    return filename + ext;
            }

            return filename;
        }

        private void GenTexture()
        {
            if (this._textureId == -1)
            {
                GL.DeleteTexture(this._textureId);
            }

            this._textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, this.TextureId);

            System.Drawing.Imaging.BitmapData lowData = this.data.LockBits(new Rectangle(0, 0, this.data.Width, this.data.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, lowData.Width, lowData.Height, 0,
                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, lowData.Scan0);

            this.data.UnlockBits(lowData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
        }

        public void dispose()
        {
            GL.DeleteTexture(this.TextureId);
            this.data.Dispose();
        }

        public bool disposedQM()
        {
            return false;
        }

        public void blt(int x, int y, Bitmap src_bitmap, Rect src_rect, int opacity=0)
        {
        }

        public void stretch_blt(Rect dest_rect, Bitmap src_bitmap, Rect src_rect, int opacity=0)
        {
        }

        public void fill_rect(int x, int y, int width, int height, Color color)
        {
        }

        public void fill_rect(Rect rect, Color color)
        {
        }

        public Bitmap clone()
        {
            return null;
        }

        public void clear()
        {
        }

        public Color get_pixel(int x, int y)
        {
            return null;
        }

        public void set_pixel(int x, int y, Color color)
        {
        }

        public void hue_change(int hug)
        {
        }

        public void draw_text(int x, int y, int width, int height, string str, int align=0)
        {
            FontStyle style = FontStyle.Regular;
            if (this.font.bold) style |= FontStyle.Bold;
            if (this.font.italic) style |= FontStyle.Italic;

            System.Drawing.Font font = new System.Drawing.Font(this.font.name, this.font.size, style);

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(this.data))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawString(str, font, new SolidBrush(this.font.color.GetNative()), new RectangleF(x, y, width, height));
                g.Flush();
            }

            this.GenTexture();
        }

        public void draw_text(Rect rect, string str, int align=0)
        {
            this.draw_text(rect.x, rect.y, rect.width, rect.height, str, align);
        }

        public void text_size(string str)
        {
        }
    }
}
