using System;
using System.Diagnostics;
using System.Text;

namespace RemoteSystemAuditor
{
    public class Audit
    {
        public String GetSystemInfo(string executableName)
        {
            // Start the child process.
            Process cmd = new Process();

            StringBuilder results = new StringBuilder();

            results.Append(executableName);
            results.AppendLine(string.Empty);
            results.AppendLine(InitialCMD(cmd, "wmic computersystem get manufacturer,model,name,domain"));
            results.AppendLine(FollowingCMD(cmd, "wmic bios get serialnumber"));
            results.AppendLine(FollowingCMD(cmd, "wmic os get caption,csdversion,osarchitecture,version"));
            results.AppendLine(FollowingCMD(cmd, "wmic BIOS get Manufacturer,Name,description,SMBIOSBIOSVersion,Version,serialnumber"));
            results.AppendLine(FollowingCMD(cmd, "wmic CPU get Name,NumberOfCores,NumberOfLogicalProcessors"));
            results.AppendLine(FollowingCMD(cmd, "wmic MEMORYCHIP get Capacity,DeviceLocator,manufacturer,PartNumber,serialnumber,Tag"));
            results.AppendLine(FollowingCMD(cmd, "wmic MEMPHYSICAL get MaxCapacity"));
            results.AppendLine(FollowingCMD(cmd, "wmic DISKDRIVE get InterfaceType,Name,model,Size,Status,statusinfo,serialnumber"));
            results.AppendLine(FollowingCMD(cmd, "wmic USERACCOUNT get Caption,Name,localaccount,PasswordRequired,Status"));
            results.AppendLine(FollowingCMD(cmd, "wmic printer get name,portname,drivername"));

            return results.ToString();
        }

        private string InitialCMD(Process cmd, string param)
        {
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Arguments = param;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(param);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string output = cmd.StandardOutput.ReadToEnd();

            //string value = output.Substring(output.IndexOf(param) + param.Length);
            string value = output.After(param);

            cmd.WaitForExit();

            return value;
        }

        private string FollowingCMD(Process cmd, string param)
        {
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.Start();
            cmd.StartInfo.Arguments = param;

            cmd.StandardInput.WriteLine(param);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string output = cmd.StandardOutput.ReadToEnd();

            //string value = output.Substring(output.IndexOf(param) + param.Length);
            string value = output.After(param);
            cmd.WaitForExit();

            return value;
        }
    }
}
