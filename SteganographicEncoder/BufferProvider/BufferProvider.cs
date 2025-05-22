using System;
using System.Collections.Generic;
using System.Linq;

namespace SteganographicEncoder {
    public class BufferProvider : IBufferProvider {
        public byte[] Buffer { get; set; }
        public HashSet<int> Indexes { get; set; }
        public Random Random { get; set; }

        public BufferProvider(byte[] buffer, int seed) {
            Buffer = buffer;

            Indexes = new HashSet<int>();
            for (int i = 0; i < Buffer.Length; i++) {
                Indexes.Add(i);
            }

            Random = new Random(seed);
        }

        public int ProvideByte(out byte value) {
            int index = Indexes.ElementAt(Random.Next(0, Indexes.Count - 1));

            value = Buffer[index];
            Indexes.Remove(index);

            return index;
        }
        public void SetByte(byte value, int index) {
            Buffer[index] = value;
        }
    }
}
