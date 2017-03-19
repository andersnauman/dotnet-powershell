using System;
using System.Text;
using System.Management.Automation;

namespace EXE
{
    public class MainProgram
    {
        public static string ExecutePowerShell(string cmd)
        {
            try
            {
                PowerShell ps = PowerShell.Create();
                ps.AddScript(cmd);
                StringBuilder stringBuilder = new StringBuilder();
                var result = ps.Invoke();
                if (ps.Streams.Error.Count != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var obj in ps.Streams.Error)
                    {
                        stringBuilder.AppendLine(obj.ToString());
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
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

        public static void Main()
        {
            Console.WriteLine("Windows PowerShell");
            Console.WriteLine("Copyright(C) 2016 Microsoft Corporation. All rights reserved.\n");
            while (true)
            {
                Console.Write("PS " + ExecutePowerShell("$(get-location).Path").Replace(System.Environment.NewLine, String.Empty) + ">");
                string cmd = Console.ReadLine();
                Console.Write(ExecutePowerShell(cmd));
            }
        }
    }
}
