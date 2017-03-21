using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace DLL
{
    public class Main
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
        private const int MY_CODE_PAGE = 437;
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleTitle(String IpConsoleTitle);
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

            PShell pshell = new PShell();
            while (true)
            {
                Console.Write("PS " + pshell.ExecuteCmd(stdHandle, "$(Get-Location).Path").Replace(System.Environment.NewLine, String.Empty) + "> ");
                string cmd = Console.ReadLine();
                Console.Write(pshell.ExecuteCmd(stdHandle, cmd));
            }
        }
     
    }
    public class ExportMain
    {
        [DllExport("Main", CallingConvention = CallingConvention.Cdecl)]
        public static void Main()
        {
            DLL.Main.LoadTerminal();
        }
    }
}