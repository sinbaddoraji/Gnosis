using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class MethodHandler
    {
        VariableHandler globalVariables;
        Method method; 
        public void DeclearGlobalVariables(ref VariableHandler variableHandler)
        { 
            globalVariables = variableHandler;
        }

        public void DoFunction(Method method)
        {
            this.method = method;

            //Run through every statement
            foreach (var statement in this.method.lot)
                IntepreteCommand(statement);
        }

        public void IntepreteCommand(Statement statement)
        {
            if(statement.tokens[0] == "print") Print(statement);
            else if(statement.tokens.Count == 2)
            {
                //If single statement like "pause"
                string command = statement.tokens[0];
                if(command == "pause")
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        void Print(Statement printStatement)
        {
            string output = "";

            for (int i = 1; i < printStatement.tokens.Count - 1; i++)
            {
                string token = printStatement.tokens[i];
                if (Lexer.IsString(token)) output += token.Substring(1,token.Length - 2);
                else if (method.lexer.isVariable(token))
                {
                    Variable variable = method.lexer.Variables.GetVariable(token);
                    output += variable.value.value.ToString();
                }
                else if(token != "<<")
                {
                    //Throw exception
                    //To be implemented
                }
            }

            Console.Write(output);

        }

    }

    class Method
    {
        public List<Statement> lot = new List<Statement>();
        public Lexer lexer;
        public Method(string code)
        {
            lexer = new Lexer(code,false);

            while(!lexer.Eof())
            {
                Statement nextStatement = lexer.NextStatement();
                lot.Add(nextStatement);
            }
        }

    }

    class Statement
    {
        public List<string> tokens = new List<string>();

        bool isConditional = false; // Is a statement with {}

        public Statement(List<string> statement)
        {
            tokens = statement;
        }
    }
}
