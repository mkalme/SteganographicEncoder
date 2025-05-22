using System;
using System.IO;
using System.Windows.Forms;
using CustomDialogs;
using SteganographicEncoder;
using EncoderAddOn;
using FileEncoderAddOn;

namespace SteganographicEncoderGUI {
    static class Program {
        private static IAddOn _addOn = new FileEncoder();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 0) {
                Stream input = _addOn.ProvideNewOutput();
                input.Position = 0;

                InputResult<string> imagePathResult = CustomDialog.ShowBrowser(BrowseType.OpenFile, "Image files|*.png");
                if (imagePathResult.DialogClosed) return;

                InputResult<string> inputResult = CustomDialog.ShowPasswordInput(x => true, "Seed", "Enter the seed");
                if (inputResult.DialogClosed) return;

                InputResult<string> imageOutputPathResult = CustomDialog.ShowBrowser(BrowseType.SaveFile, "Image files|*.png;*.bmp");
                if (imageOutputPathResult.DialogClosed) return;

                ISteganographicEncoder encoder = new SteganographicEncoder.SteganographicEncoder() {
                    Seed = inputResult.Value.GetHashCode()
                };

                Stream output = encoder.Encode(File.OpenRead(imagePathResult.Value), input);
                output.Position = 0;

                using (FileStream file = File.Create(imageOutputPathResult.Value)) {
                    output.CopyTo(output);
                }
            } else {
                InputResult<string> inputResult = CustomDialog.ShowPasswordInput(x => true, "Seed", "Enter the seed");
                if (inputResult.DialogClosed) return;

                ISteganographicEncoder encoder = new SteganographicEncoder.SteganographicEncoder() { 
                    Seed = inputResult.Value.GetHashCode()
                };

                Stream input = encoder.Decode(File.OpenRead(args[0]));
                _addOn.ProvideInput(input);
            }
        }
    }
}
