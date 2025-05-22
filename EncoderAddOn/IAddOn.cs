using System;
using System.IO;

namespace EncoderAddOn {
    public interface IAddOn {
        event EventHandler<Stream> OutputProvided;

        void ProvideInput(Stream input);
        Stream ProvideNewOutput();
    }
}
