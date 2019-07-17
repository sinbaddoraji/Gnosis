using System.Collections.Generic;
using System.IO;

namespace Gnosis
{
    class EntryPoint
    {
        public static Dictionary<string, Method> Methods;
        public static VariableHandler globalVariableHandler;
        static void Main(string[] args)
        {
            args = new[] { @"C:\Users\Sage\Desktop\Gnosis\Gnosis\bin\Debug\Example Scripts\Calculator1.gno" };

            if(args.Length == 0)
                return; //exit program if no arguments

            Lexer lexer = new Lexer(File.ReadAllText(args[0])); // Get tokens from code file
            Methods = lexer.Methods;
            globalVariableHandler = new VariableHandler(); // variable handler for "main"

            Method mainMethod = lexer.MainMethod(); // Main method (entry point "main")

            //Make public and private variableHandler for "main" the same 
            lexer.MainMethod().lexer.Variables = new VariableHandler(); 

            // Create intepreter instance
            MethodHandler methodHandler = new MethodHandler(globalVariableHandler, mainMethod); 

            if (mainMethod != null)
                 methodHandler.DoFunction(mainMethod); // Inteprete commands in Main
        }
    }
}
