using System;
using System.IO;
using System.Reflection;

namespace RemoteSystemAuditor
{
    class Program
    {
        // Rename the Executable file once compiled, with a meaningful name to keep track of the audits.
        static void Main(string[] args)
        {
            string Results;
            string DirectoryName = Directory.GetCurrentDirectory();
            string ExecutableName = $"{Path.GetFileName(Assembly.GetEntryAssembly().Location).Before(".exe")}";

            SystemAudit audit = new SystemAudit();
            SendMail send = new SendMail();

            Console.WriteLine("Please Wait...");
            Results = audit.GetSystemInfo(ExecutableName, DirectoryName);
            Console.WriteLine(Results);
            send.NewMessage($"System Audit {ExecutableName}", Results);
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}
