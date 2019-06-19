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
            args = new[] { "HelloWorld.gno" };

            Lexer lexer = new Lexer(File.ReadAllText(args[0])); // Get tokens from code file
            MethodHandler methodHandler = new MethodHandler(); // Create intepreter instance
            VariableHandler globalVariableHandler = new VariableHandler(); // variable handler for "main"

            Method mainMethod = lexer.MainMethod(); // Main method (entry point "main")
            methodHandler.DeclearGlobalVariables(ref globalVariableHandler);

            if(mainMethod != null)
                 methodHandler.DoFunction(mainMethod); // Inteprete commands in Main
              
        }
    }
}
