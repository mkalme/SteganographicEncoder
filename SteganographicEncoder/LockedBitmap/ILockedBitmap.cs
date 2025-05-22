using System;
using System.Drawing;

namespace SteganographicEncoder {
    public interface ILockedBitmap {
        Bitmap Source { get; set; }

        byte[] Lock();
        void Unlock(byte[] rgbValues);
    }
}
