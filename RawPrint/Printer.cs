using System;
using System.IO;
using System.Linq;

namespace RawPrint
{
    public class Printer : IPrinter
    {
        public void PrintRawFile(string printer, string path, bool paused)
        {
            PrintRawFile(printer, path, path, paused);
        }

        public void PrintRawFile(string printer, string path, string documentName, bool paused)
        {
            using (var stream = File.OpenRead(path))
            {
                PrintRawStream(printer, stream, documentName, paused);
            }
        }

        public void PrintRawStream(string printer, Stream stream, string documentName, bool paused)
        {
            var defaults = new PRINTER_DEFAULTS
            {
                DesiredPrinterAccess = PRINTER_ACCESS_MASK.PRINTER_ACCESS_USE
            };

            using (var safePrinter = SafePrinter.OpenPrinter(printer, ref defaults))
            {
                DocPrinter(safePrinter, documentName, IsXPSDriver(safePrinter) ? "XPS_PASS" : "RAW", stream, paused);
            }
        }

        private static bool IsXPSDriver(SafePrinter printer)
        {
            var files = printer.GetPrinterDriverDependentFiles();

            return files.Any(f => f.EndsWith("pipelineconfig.xml", StringComparison.InvariantCultureIgnoreCase));
        }

        private static void DocPrinter(SafePrinter printer, string documentName, string dataType, Stream stream, bool paused)
        {
            var di1 = new DOC_INFO_1
            {
                pDataType = dataType,
                pDocName = documentName,
            };

            var id = printer.StartDocPrinter(di1);

            if (paused)
            {
                NativeMethods.SetJob(printer.DangerousGetHandle(), id, 0, IntPtr.Zero, (int) JobControl.Pause);
            }

            try
            {
                PagePrinter(printer, stream);
            }
            finally
            {
                printer.EndDocPrinter();
            }
        }

        private static void PagePrinter(SafePrinter printer, Stream stream)
        {
            printer.StartPagePrinter();

            try
            {
                WritePrinter(printer, stream);
            }
            finally
            {
                printer.EndPagePrinter();
            }
        }

        private static void WritePrinter(SafePrinter printer, Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            const int bufferSize = 1048576;
            var buffer = new byte[bufferSize];

            int read;
            while ((read = stream.Read(buffer, 0, bufferSize)) != 0)
            {
                printer.WritePrinter(buffer, read);
            }
        }

        [Obsolete]
        public static void PrintFile(string printer, string path, string documentName)
        {
            using (var stream = File.OpenRead(path))
            {
                PrintStream(printer, stream, documentName);
            }
        }

        [Obsolete]
        public static void PrintFile(string printer, string path)
        {
            PrintFile(printer, path, path);
        }

        [Obsolete]
        public static void PrintStream(string printer, Stream stream, string documentName)
        {
            var defaults = new PRINTER_DEFAULTS
            {
                DesiredPrinterAccess = PRINTER_ACCESS_MASK.PRINTER_ACCESS_USE
            };

            using (var safePrinter = SafePrinter.OpenPrinter(printer, ref defaults))
            {
                DocPrinter(safePrinter, documentName, IsXPSDriver(safePrinter) ? "XPS_PASS" : "RAW", stream, false);
            }
        }
    }
}
