using System;
using SteganographicEncoder;

namespace DemoConsole {
    class Program {
        static void Main(string[] args) {
            FileEncoder encoder = new FileEncoder("D:\\test\\baseImage.png", "D:\\test\\test.txt");
            encoder.Encode("D:\\test\\outputImage.png");

            //FileDecoder decoder = new FileDecoder("D:\\test\\outputImage.png");
            //decoder.Decode("D:\\test\\output");

            //long l = 17;

            //byte[] array = new byte[] { 206, 134, 70, 255, 204, 132, 68, 255, 204, 130, 64, 255, 200, 126, 60, 255, 198, 124, 58, 255, 195, 121, 55, 255, 196, 121, 53, 255, 196, 121, 52, 255 };
            //byte[] data = BitConverter.GetBytes(l);

            //ByteEncoder e = new ByteEncoder();
            //e.Encode(array, 0, data, 0, data.Length);

            //byte[] decodedData = new byte[data.Length];
            //e.Decode(array, 0, decodedData, 0, decodedData.Length);

            //Console.WriteLine(string.Join(", ", data));
            //Console.WriteLine(string.Join(", ", decodedData));

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
