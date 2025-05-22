using System;

namespace SteganographicEncoder {
    public class ArrayEncoder {
        public void Encode(byte[] container, byte[] input, int seed) {
            IBufferProvider provider = new BufferProvider(container, seed);

            SetBytes(provider, BitConverter.GetBytes(input.Length));
            SetBytes(provider, input);
        }
        private static void SetBytes(IBufferProvider provider, byte[] input) {
            for (int i = 0; i < input.Length; i++) {
                byte inputB = input[i];

                for (int j = 0; j < 8; j++) {
                    int index = provider.ProvideByte(out byte outputB);
                    byte bit = (byte)(inputB << (7 - j) >> 7);

                    outputB |= bit;
                    provider.SetByte(outputB, index);
                }
            }
        }

        public byte[] Decode(byte[] input, int seed) {
            IBufferProvider provider = new BufferProvider(input, seed);

            int length = BitConverter.ToInt32(ReadBytes(provider, sizeof(int)));
            return ReadBytes(provider, length);
        }
        private static byte[] ReadBytes(IBufferProvider provider, int length) {
            byte[] output = new byte[length];

            for (int i = 0; i < length; i++) {
                byte outputB = 0;

                for (int j = 0; j < 8; j++) {
                    provider.ProvideByte(out byte inputB);
                    outputB |= (byte)(inputB << j);
                }

                output[i] = outputB;
            }

            return output;
        }
    }
}
