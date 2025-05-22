using System;
using System.IO;
using CustomDialogs;
using EncoderAddOn;

namespace FileEncoderAddOn {
    public class FileEncoder : IAddOn {
        public event EventHandler<Stream> OutputProvided;

        public void ProvideInput(Stream input) {
            throw new NotImplementedException();
        }
        public Stream ProvideNewOutput() {
            InputResult<string> filePathResult = CustomDialog.ShowBrowser(BrowseType.OpenFile);
            if (filePathResult.DialogClosed) return null;

            using (FileStream file = File.OpenRead(filePathResult.Value)) {
                MemoryStream output = new MemoryStream();
                file.CopyTo(output);

                return output;
            }
        }
    }
}
