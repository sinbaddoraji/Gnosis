using System.IO;

namespace Gnosis
{

    class EntryPoint
    {
        static void Main(string[] args)
        {
            //args = new[] { @"C:\Users\Sage\Desktop\Gnosis\Gnosis\bin\Debug\Example Scripts\CountUp.gno" };

            if(args.Length == 0)
                return; //exit program if no arguments

            Lexer lexer = new Lexer(File.ReadAllText(args[0])); // Get tokens from code file
            VariableHandler globalVariableHandler = new VariableHandler(); // variable handler for "main"

            Method mainMethod = lexer.MainMethod(); // Main method (entry point "main")

            //Make public and private variableHandler for "main" the same 
            lexer.MainMethod().lexer.Variables = globalVariableHandler; 

            // Create intepreter instance
            MethodHandler methodHandler = new MethodHandler(globalVariableHandler, mainMethod); 

            if (mainMethod != null)
                 methodHandler.DoFunction(mainMethod); // Inteprete commands in Main
              
        }
    }
}
