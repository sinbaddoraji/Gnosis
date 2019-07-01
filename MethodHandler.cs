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
        LogicHandler logicHandler;
        ValueHandler valueHanlder;
        Method method; 

        public MethodHandler(ref VariableHandler globalVars, ref Method m)
        {
            method = m;
            globalVariables = globalVars;
            valueHanlder = new ValueHandler(ref globalVariables, ref method);

            logicHandler = new LogicHandler(valueHanlder);
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
            else if (statement.tokens[0] == "if") IfStatement(ref statement);
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

            List<string> internalTokens = new List<string>();

            for (int i = 1; i < printStatement.tokens.Count; i++)
            {
                string token = printStatement.tokens[i];

                if (token == "<<" || token == ";")
                {
                    output += valueHanlder.ParseStringExpression(internalTokens, false);
                    internalTokens.Clear();
                }
                else internalTokens.Add(token);
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
            //Variable type will be based on the first variable in statement (tokens[3])
            string variableName = varStatement.tokens[1];
            int valueType = valueHanlder.ValueType(varStatement.tokens[3]);
            //var value = blah + blah + blah

            // value types : double, float, int, long, bool, string
            // double -> 0
            //  float -> 1
            //    int -> 2
            //   long -> 3
            //   bool -> 4
            // string -> 5
            //  other -> 6;
            dynamic value;
            switch (valueType)
            {
                case 0:
                    value = valueHanlder.ParseDoubleExpression(varStatement.tokens);
                    break;
                case 1:
                    value = valueHanlder.ParseFloatExpression(varStatement.tokens);
                    break;
                case 2:
                    value = valueHanlder.ParseIntExpression(varStatement.tokens);
                    break;
                case 3:
                    value = valueHanlder.ParseLongExpression(varStatement.tokens);
                    break;
                case 4:
                    value = null;
                    //To be implemented (Bool)
                    break;
                case 5:
                    value = valueHanlder.ParseStringExpression(varStatement.tokens);
                    break;
                default:
                    //Throw exception
                    //To be implemented
                    return;
            }

            method.lexer.Variables.AddVariable(variableName,value,valueType);
        }

        void IfStatement(ref Statement ifStatement)
        {
            //if (value == 15 && value == 16)
            int boolStart = 2, boolEnd = 2;
            int i;

            for (i = 1; i < ifStatement.tokens.Count; i++)
            {
                if(ifStatement.tokens[i] == ")")
                {
                    boolEnd = i;
                    i++;
                    break;
                }
            }

            string[] boolTokens = new string[boolEnd - boolStart];
            ifStatement.tokens.CopyTo(boolStart,boolTokens,0, boolEnd - boolStart);

            //Do statment or not
            bool doStatement = logicHandler.IntepreteBoolExpression(boolTokens);


            //Remove all tokens before starting of {
            if(doStatement)
            {
                ifStatement.tokens.RemoveRange(0, i + 1);

                ifStatement.internalMethod = new Method(ifStatement.tokens);
                ifStatement.internalVariableHandler = new VariableHandler();
                ifStatement.internalMethodHandler = new MethodHandler(ref globalVariables, ref ifStatement.internalMethod);

                ifStatement.RunStatement();
            }
        }

    }
}
