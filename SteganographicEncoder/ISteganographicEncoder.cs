using System;
using System.IO;

namespace SteganographicEncoder {
    public interface ISteganographicEncoder {
        Stream Encode(Stream inputImage, Stream input);
        Stream Decode(Stream inputImage);
    }
}
