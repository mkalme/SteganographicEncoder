using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace SteganographicEncoder {
    public class SteganographicEncoder : ISteganographicEncoder {
        public int Seed { get; set; }

        public Stream Encode(Stream inputImage, Stream input) {
            ILockedBitmap bitmap = new LockedBitmap(new Bitmap(Image.FromStream(inputImage)));

            using (MemoryStream inputMs = new MemoryStream()) {
                input.CopyTo(inputMs);

                ArrayEncoder encoder = new ArrayEncoder();
                byte[] rgbValues = bitmap.Lock();

                encoder.Encode(rgbValues, inputMs.ToArray(), Seed);

                bitmap.Unlock(rgbValues);

                Stream output = new MemoryStream();
                bitmap.Source.Save(output, ImageFormat.Png);

                return output;
            }
        }
        public Stream Decode(Stream inputImage) {
            MemoryStream output = new MemoryStream();

            ILockedBitmap bitmap = new LockedBitmap(new Bitmap(Image.FromStream(inputImage)));
            ArrayEncoder encoder = new ArrayEncoder();

            byte[] outputBytes = encoder.Decode(bitmap.Lock(), Seed);
            output.Write(outputBytes, 0, outputBytes.Length);

            return output;
        }
    }
}
