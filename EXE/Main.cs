using System;

namespace EXE
{
    public class Main
    {
        public static void Main()
        {
            Console.WriteLine("Windows PowerShell");
            Console.WriteLine("Copyright(C) 2016 Microsoft Corporation. All rights reserved.\n");
            PShell pshell = new PShell();
            while (true)
            {
                Console.Write("PS " + pshell.ExecuteCmd("$(Get-Location).Path").Replace(System.Environment.NewLine, String.Empty) + "> ");
                string cmd = Console.ReadLine();
                Console.Write(pshell.ExecuteCmd(cmd));
            }
        }
    }
}
