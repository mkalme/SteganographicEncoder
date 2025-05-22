using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace SteganographicEncoder {
    public class LockedBitmap : ILockedBitmap {
        public Bitmap Source { get; set; }
        public BitmapData Data { get; set; }

        public LockedBitmap(Bitmap bitmap) {
            Source = bitmap;
        }

        public byte[] Lock() {
            Rectangle rect = new Rectangle(0, 0, Source.Width, Source.Height);
            BitmapData data = Source.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(data.Stride) * data.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(data.Scan0, rgbValues, 0, bytes);
;
            Data = data;

            return rgbValues;
        }
        public void Unlock(byte[] rgbValues) {
            if (Data == null) {
                using (MemoryStream stream = new MemoryStream(rgbValues)) {
                    Source = new Bitmap(stream);
                }
            } else {
                Marshal.Copy(rgbValues, 0, Data.Scan0, rgbValues.Length);
                Source.UnlockBits(Data);
            }
        }
    }
}