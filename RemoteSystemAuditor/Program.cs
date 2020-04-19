using System;
using System.IO;
using System.Reflection;

namespace RemoteSystemAuditor
{
    class Program
    {
        // Rename the Executable file once compiled, with a meaningful name to keep track of the audits.
        public static string ExecutableName = $"{Path.GetFileName(Assembly.GetEntryAssembly().Location).Before(".exe")}";
        static void Main(string[] args)
        {
            Audit audit = new Audit();

            Console.WriteLine("Please Wait...");
            string results = audit.GetSystemInfo(ExecutableName);
            Console.WriteLine(results);
            SendMail.NewMessage($"System Audit {ExecutableName}", results);
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}
