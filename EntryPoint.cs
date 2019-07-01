using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Gnosis
{

    class EntryPoint
    {
        static void Main(string[] args)
        {
            args = new[] { "Calculator2.gno" };

            Lexer lexer = new Lexer(File.ReadAllText(args[0])); // Get tokens from code file
            VariableHandler globalVariableHandler = new VariableHandler(); // variable handler for "main"

            Method mainMethod = lexer.MainMethod(); // Main method (entry point "main")

            // Create intepreter instance
            MethodHandler methodHandler = new MethodHandler(ref globalVariableHandler, ref mainMethod); 

            if (mainMethod != null)
                 methodHandler.DoFunction(mainMethod); // Inteprete commands in Main
              
        }
    }
}
