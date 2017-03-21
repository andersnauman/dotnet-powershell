using System;
using System.Text;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace DLL
{
    public class PShell
    {
        Runspace runspace;
        public PShell() {
            runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
        }
        public string ExecuteCmd(IntPtr stdHandle, string cmd)
        {
            try
            {
                PowerShell ps = PowerShell.Create();
                ps.Runspace = runspace;
                StringBuilder stringBuilder = new StringBuilder();
                using (ps)
                {
                    ps.AddScript(cmd);
                    Collection<PSObject> result = ps.Invoke();
                    if (ps.Streams.Error.Count != 0)
                    {
                        Main.SetConsoleTextAttribute(stdHandle, 12);
                        foreach (var obj in ps.Streams.Error)
                        {
                            stringBuilder.AppendLine(obj.ToString());
                        }
                    }
                    else
                    {
                        Main.SetConsoleTextAttribute(stdHandle, 7);
                        foreach (var obj in result)
                        {
                            stringBuilder.AppendLine(obj.ToString());
                        }
                    }
                }
                ps = null;
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
