using System.IO;

namespace RawPrint
{
    public interface IPrinter
    {
        void PrintRawFile(string printer, string path, string documentName, bool paused = false);
        void PrintRawFile(string printer, string path, bool paused = false);
        void PrintRawStream(string printer, Stream stream, string documentName, bool paused = false);
    }
}