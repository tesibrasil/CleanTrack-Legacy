using IMS7_PARSER;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
// using YamlDotNet.Serialization;

namespace imsTicketParser
{
    class Program
    {

        static void Main(string[] args)
        {
            if ((args.Length < 1) || (!System.IO.File.Exists(args[0])))
                return;

            Cycle myCycle = IMS7_PARSER.IMS7Object.GetCycleFromFile(args[0]);
            Console.WriteLine(myCycle.Dump());
        }
    }
}
