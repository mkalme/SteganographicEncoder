using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SteganographicEncoder {
    public interface IByteEncoder {
        void Encode(byte[] array, long index, byte[] inputData, long dataIndex, long dataLength);
        void Decode(byte[] array, long index, byte[] outputData, long dataIndex, long dataLength);
    }

    public class ByteEncoder : IByteEncoder {
        public void Encode(byte[] array, long index, byte[] inputData, long dataIndex, long dataLength) {
            for (long i = 0; i < dataLength; i++) {
                for (int j = 0; j < 4; j++) {
                    byte b = array[i * 4 + j + index];

                    int shift = j * 2;
                    byte d = (byte)((inputData[i + dataIndex] & (0x3 << shift)) >> shift);

                    array[i * 4 + j + index] = (byte)((b & 0xFC) | d);
                }
            }
        }
        public void Decode(byte[] array, long index, byte[] outputData, long dataIndex, long dataLength) {
            for (long i = 0; i < dataLength; i++) {
                byte d = 0;

                for (int j = 0; j < 4; j++) {
                    d |= (byte)((array[i * 4 + j + index] & 0x3) << j * 2);
                }

                outputData[i + dataIndex] = d;
            }
        }
    }

    public class SteganographicEncoderNew : ISteganographicEncoder {
        public Stream Encode(Stream inputImage, Stream input) {
            ILockedBitmap bitmap = new LockedBitmap(Image.FromStream(inputImage) as Bitmap);

            byte[] rgbData = bitmap.Lock();

            byte[] inputData = new byte[input.Length - input.Position];
            input.Read(inputData, 0, inputData.Length);

            ByteEncoder encoder = new ByteEncoder();

            Console.WriteLine(string.Join(", ", rgbData.Take(32)));

            encoder.Encode(rgbData, 0, BitConverter.GetBytes(inputData.GetLongLength(0)), 0, sizeof(long));
            //encoder.Encode(rgbData, sizeof(long), inputData, 0, inputData.Length);

            Console.WriteLine(string.Join(", ", rgbData.Take(32)));

            bitmap.Unlock(rgbData);

            rgbData = bitmap.Lock();

            //byte[] lBytes = new byte[8];
            //encoder.Decode(rgbData, 0, lBytes, 0, 8);

            Console.WriteLine(string.Join(", ", rgbData.Take(32)));

            MemoryStream output = new MemoryStream();
            bitmap.Source.Save(output, System.Drawing.Imaging.ImageFormat.Png);

            return output;
        }
        public Stream Decode(Stream inputImage) {
            ILockedBitmap bitmap = new LockedBitmap(Image.FromStream(inputImage) as Bitmap);

            byte[] rgbData = bitmap.Lock();

            byte[] sizeBytes = new byte[sizeof(long)];
            long size;

            ByteEncoder encoder = new ByteEncoder();
            encoder.Decode(rgbData, 0, sizeBytes, 0, sizeBytes.Length);
            size = BitConverter.ToInt64(sizeBytes, 0);

            //Console.WriteLine(string.Join(", ", sizeBytes));

            byte[] data = new byte[size];
            encoder.Decode(rgbData, sizeBytes.Length, data, 0, size);

            return new MemoryStream(data);
        }
    }

    public class FileEncoder { 
        public ISteganographicEncoder Encoder { get; set; }
        public string ImagePath { get; set; }
        public string FilePath { get; set; }

        public FileEncoder(string imagePath, string filePath) {
            Encoder = new SteganographicEncoderNew();
            ImagePath = imagePath;
            FilePath = filePath;
        }

        public void Encode(string outputImagePath) {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            using (FileStream imageFile = File.OpenRead(ImagePath))
            using (FileStream file = File.OpenRead(FilePath))
            using (FileStream outputImage = File.Create(outputImagePath)) {
                FileInfo info = new FileInfo(FilePath);

                writer.Write(info.Name);
                writer.Write(info.CreationTimeUtc.ToBinary());
                file.CopyTo(writer.BaseStream);

                writer.BaseStream.Position = 0;

                Stream image = Encoder.Encode(imageFile, writer.BaseStream);
                image.Position = 0;

                byte[] bytes = new byte[image.Length];
                image.Read(bytes, 0, bytes.Length);

                outputImage.Write(bytes, 0, bytes.Length);
            }
        }
    }

    public class FileDecoder {
        public ISteganographicEncoder Encoder { get; set; }
        public string ImagePath { get; set; }

        public FileDecoder(string imagePath) {
            Encoder = new SteganographicEncoderNew();
            ImagePath = imagePath;
        }

        public void Decode(string directory) {
            using (FileStream imageFile = File.OpenRead(ImagePath))
            using (BinaryReader reader = new BinaryReader(Encoder.Decode(imageFile))) {
                reader.BaseStream.Position = 0;

                Console.WriteLine(reader.ReadString());
            }
        }
    }
}
