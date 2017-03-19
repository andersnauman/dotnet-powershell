using System;
using System.Text;
using System.Management.Automation;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace DLL
{
    public class MainProgram
    {
        [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 65001;
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleTitle(String IpConsoleTitle);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);

        public static void LoadTerminal()
        {
            AllocConsole();
            IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            SetConsoleTitle("Windows PowerShell");
            SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
            FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
            StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            Console.WriteLine("Windows PowerShell");
            Console.WriteLine("Copyright(C) 2016 Microsoft Corporation. All rights reserved.\n");

            while (true)
            {
                Console.Write("PS " + ExecutePowerShell(stdHandle, "$(get-location).Path").Replace(System.Environment.NewLine, String.Empty) + ">");
                string cmd = Console.ReadLine();
                Console.Write(ExecutePowerShell(stdHandle, cmd));
            }
        }
        public static string ExecutePowerShell(IntPtr stdHandle, string cmd)
        {
            try
            {
                PowerShell ps = PowerShell.Create();
                ps.AddScript(cmd);
                StringBuilder stringBuilder = new StringBuilder();
                var result = ps.Invoke();
                if (ps.Streams.Error.Count != 0)
                {
                    SetConsoleTextAttribute(stdHandle, 4);
                    foreach (var obj in ps.Streams.Error)
                    {
                        stringBuilder.AppendLine(obj.ToString());
                    }
                }
                else
                {
                    SetConsoleTextAttribute(stdHandle, 7);
                    foreach (var obj in result)
                    {
                        stringBuilder.AppendLine(obj.ToString());
                    }
                }
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}