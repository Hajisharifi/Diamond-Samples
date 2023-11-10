using System;
using System.Linq;
using Diamond.ISO8583.Serialization;

namespace ISOParser
{
    internal class Program
    {
        //________________________________________________________________________

        private static void LogError(object? exceptionObject)
        {
            Console.WriteLine("--------------");
            Console.WriteLine("C-ERROR");

            if (exceptionObject is IsoSerializerException ex1)
            {
                exceptionObject = ex1.IsoMessage + Environment.NewLine + ex1.Message + $" bit:{ex1.BitIndex} config:{ex1.Configuration}";
            }
            else if (exceptionObject is Exception ex2)
            {
                exceptionObject = ex2?.Message;
            }

            Console.WriteLine(exceptionObject != null ? exceptionObject : "NULL!");
            Console.WriteLine("--------------");
        }
        //________________________________________________________________________

        private static string? GetDump(string[] args)
        {
            if (args is null) return null;
            return args
                .Select(a => a?.Trim()?.Trim('\'', '\"')?.Trim())
                .Where(a => !string.IsNullOrEmpty(a))
                .OrderByDescending(a => a!.Length)
                .FirstOrDefault();
        }
        //________________________________________________________________________

        private static string? GetXMLFile()
        {
            var files = System.IO.Directory
                .GetFiles(".", "*.iso8583.xml")
                .Where(f => !string.IsNullOrEmpty(f))
                .Select(f => new System.IO.FileInfo(f))
                .Where(f => f.Exists && 200 < f.Length && f.Length < 10 * 1024 * 1024)
                .Select(f => (f.FullName, Name: System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileNameWithoutExtension(f.Name))))
                .ToList();

            if (files is null || files.Count <= 0)
            {
                Console.WriteLine("not found any protocol file (*.iso8583.xml)");
                return null;
            }

            if (files.Count == 1) return files[0].FullName;

            Console.WriteLine();
            Console.WriteLine("Select protocol:");
            Console.WriteLine("================");
            for (int ix = 0; ix < files.Count; ix++)
            {
                Console.WriteLine($"[{ix + 1}] {files[ix].Name}");
            }

            string? sel = Console.ReadLine();
            if (string.IsNullOrEmpty(sel) || (!int.TryParse(sel, out int index)) || index <= 0 || index > files.Count)
            {
                Console.WriteLine("Invalid input");
                return null;
            }

            return files[index - 1].FullName;
        }
        //________________________________________________________________________

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                AppDomain.CurrentDomain.UnhandledException += (s, e) => LogError(e?.ExceptionObject);

                string? xmlFile = GetXMLFile();
                if (string.IsNullOrEmpty(xmlFile)) return;

                string xml = System.IO.File.ReadAllText(xmlFile);
                var config = Diamond.ISO8583.XML.RootModel.Deserialize(xml);
                if (config is null)
                {
                    Console.WriteLine("invalid the xml file");
                    return;
                }

                var serializer = new Diamond.ISO8583.Serialization.IsoSerializer(config, null);
                var buf = new byte[64 * 1024 + 10];
                string? dump = GetDump(args);

                while (true)
                {
                    if (string.IsNullOrEmpty(dump))
                    {
                        Console.WriteLine("Dump: [exit?]");
                        dump = Console.ReadLine()?.Trim();
                        if (string.IsNullOrEmpty(dump)) continue;
                    }
                    if (string.Equals(dump, "exit", StringComparison.OrdinalIgnoreCase)) return;

                    try
                    {
                        var msg = new IsoMessage();
                        int len = StrEncoding.Hex.GetBytes(dump, buf);
                        len = serializer.Deserialize(msg, buf.AsMemory().Slice(0, len));
                        string pars = msg.ToString();
                        if (string.IsNullOrEmpty(pars)) pars = "ERROR-EMPTY";

                        Console.WriteLine();
                        Console.WriteLine($"ISO: Offset=0 Length={len}");
                        Console.WriteLine(pars);
                        Console.WriteLine();
                    }
                    catch (OutOfMemoryException ex)
                    {
                        LogError(ex);
                    }
                    finally
                    {
                        dump = null;
                    }
                }
            }
            catch (OutOfMemoryException ex)
            {
                LogError(ex);
            }
        }
        //________________________________________________________________________
    }
}
