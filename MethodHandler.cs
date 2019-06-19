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
            for (int i = 0; i < this.method.lot.Count; i++)
            {
                var statement = this.method.lot[i];
                IntepreteCommand(ref statement);
            }
                
        }

        public void IntepreteCommand(ref Statement statement)
        {
            if(statement.tokens[0] == "print") Print(ref statement);
            else if (statement.tokens[0] == "input") Input(ref statement);
            else if (statement.tokens[0] == "var") VarDeclaration(ref statement);
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

        void Print(ref Statement printStatement)
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
                else if (globalVariables.IsVariable(token))
                {
                    Variable variable = globalVariables.GetVariable(token);
                    output += variable.value.value.ToString();
                }
                else if(token != "<<")
                {
                    //Throw exception
                    //To be implemented
                    return;
                }
            }

            Console.Write(output);

        }

        void Input(ref Statement printStatement)
        {
            //input >> variable;
            //input "Display string" >> variable;

            string variableName;
            string value;

            if (printStatement.tokens.Count == 4)
            {
                //if input does not have display message
                variableName = printStatement.tokens[2];
            }
            else if (printStatement.tokens.Count == 5)
            {
                //if input has a display message
                string display = printStatement.tokens[1];
                Console.Write(display.Substring(1, display.Length -2));

                variableName = printStatement.tokens[3];
            }
            else
            {
                //Throw exception
                //To be implemented
                return;
            }

            value = Console.ReadLine();

            globalVariables.AddVariable(variableName,value);
        }

        void VarDeclaration(ref Statement varStatement)
        {

        }

    }
}
