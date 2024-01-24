RawPrint
========

.Net library to send files directly to a Windows printer bypassing the printer driver.

Send PostScript, PCL or other print file types directly to a printer.

Requires .Net 4 runtime on Windows XP to 10 and Server 2003 to 2012.

Usage:

	using RawPrint;
	
	Printer.PrintFile("Printer Name", @"C:\Path\To\Print\File.prn", "Document Name");

Installation:

To install Raw Print, run the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console)

	PM> Install-Package RawPrint

*2016-02-21*

Static methods are now obsolete.
Introduced IPrinter interface to make mocking easier.
Support for spooling a paused print job

*2015-10-20*

Fixed an issue with some HP drivers that misname their pipelineconfig.xml file.