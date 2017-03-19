using System.Runtime.InteropServices;

namespace DLL
{
    public class ExportMain
    {
        [DllExport("Main", CallingConvention = CallingConvention.Cdecl)]
        public static void Main()
        {
            MainProgram.LoadTerminal();
        }
    }
}