using System;
using RawPrint;

// modified by Jos Schaars to allow single argument

namespace RP
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] BSDlic = {
                "RawPrint - sends a file directly to a printer ",
                "Copyright (c) 2014, Tony Edgecombe",
                "Modified 2016 for WPDOS.org",
                "",
                "Syntax: RawPrint [printer name] <file name>" };
            try
            {
            IPrinter printer = new Printer();
                if (args.Length == 1)
                    printer.PrintRawFile("", args[0], args[0]);
                else if (args.Length == 2)
                    printer.PrintRawFile(args[0], args[1], args[1]);
                else
                    foreach (string line in BSDlic)
                        Console.WriteLine(line);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}
