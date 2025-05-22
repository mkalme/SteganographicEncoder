using System;

namespace SteganographicEncoder {
    public interface IBufferProvider {
        int ProvideByte(out byte value);
        void SetByte(byte value, int index);
    }
}
