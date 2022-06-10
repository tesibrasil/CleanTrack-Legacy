using System;
using System.Collections.Generic;

using System.ServiceProcess;
using System.Text;

namespace WindowsService1
{
    static class Program
    {
        static void Main(string[] args)
        {
            if ((args.Length == 1) && (args[0].ToUpper() == "-D" || args[0].ToUpper() == "--DEBUG"))
            {
                (new CleanerSynchro()).DebugService();
                return;
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CleanerSynchro()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
