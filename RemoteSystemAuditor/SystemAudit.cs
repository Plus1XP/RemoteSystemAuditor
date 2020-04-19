using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace RemoteSystemAuditor
{
    public class SystemAudit
    {
        private readonly Process process;

        public SystemAudit()
        {
            this.process = new Process();
            this.ConfigureCMD(this.process);
        }

        public String GetSystemInfo(string executableName, string directoryName)
        {
            StringBuilder results = new StringBuilder();

            results.Append(executableName);
            results.AppendLine(string.Empty);
            results.AppendLine(this.ExecuteCMD(this.process, "wmic computersystem get manufacturer,model,name,domain"));
            results.AppendLine(this.ExecuteCMD(this.process, "wmic bios get serialnumber"));
            results.AppendLine(this.ExecuteCMD(this.process, "wmic os get caption,csdversion,osarchitecture,version"));
            results.AppendLine(this.ExecuteCMD(this.process, "wmic BIOS get Manufacturer,Name,description,SMBIOSBIOSVersion,Version,serialnumber"));
            results.AppendLine(this.ExecuteCMD(this.process, "wmic CPU get Name,NumberOfCores,NumberOfLogicalProcessors"));
            results.AppendLine(this.ExecuteCMD(this.process, "wmic MEMORYCHIP get Capacity,DeviceLocator,manufacturer,PartNumber,serialnumber,Tag"));
            results.AppendLine(this.ExecuteCMD(this.process, "wmic MEMPHYSICAL get MaxCapacity"));
            results.AppendLine(this.ExecuteCMD(this.process, "wmic DISKDRIVE get InterfaceType,Name,model,Size,Status,statusinfo,serialnumber"));
            results.AppendLine(this.ExecuteCMD(this.process, "wmic USERACCOUNT get Caption,Name,localaccount,PasswordRequired,Status"));
            results.AppendLine(this.ExecuteCMD(this.process, "wmic printer get name,portname,drivername"));

            return Regex.Replace(results.ToString().Replace($"{directoryName}>",string.Empty), @"^\s+$[\r\n]*", Environment.NewLine, RegexOptions.Multiline);
        }

        private void ConfigureCMD(Process process)
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
        }

        private string ExecuteCMD(Process process, string param)
        {
            process.Start();
            process.StartInfo.Arguments = param;

            process.StandardInput.WriteLine(param);
            process.StandardInput.Flush();
            process.StandardInput.Close();

            string value = process.StandardOutput.ReadToEnd().After(param);

            process.WaitForExit();

            return value;
        }
    }
}
