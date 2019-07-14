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

        public MethodHandler(VariableHandler globalVars,  Method m)
        {
            method = m;
            globalVariables = globalVars;

            valueHanlder = new ValueHandler(ref globalVariables, ref method);
            logicHandler = new LogicHandler(valueHanlder);

            valueHanlder.logicHandler = logicHandler;
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
            if(statement.tokens.Count == 0) return;
            else if(statement.tokens[0] == "print") Print(ref statement);
            else if (statement.tokens[0] == "input") Input(ref statement);
            else if (statement.tokens[0] == "var") VarDeclaration(ref statement);
            else if (statement.tokens[0] == "if") IfStatement(ref statement);
            else if (statement.tokens[0] == "while") WhileLoopStatement(statement);
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
            var valueType = valueHanlder.ValueType(varStatement.tokens[3]);

            //if given "other" value type.. find out if is actually numeical or boolean value
            if(valueType == Value.Value_Type.Other)
            {
                if(varStatement.tokens.Contains("!=")) valueType = Value.Value_Type.Bool;
                else if (varStatement.tokens.Contains("==")) valueType = Value.Value_Type.Bool;
                else if(varStatement.tokens[3] == "(")
                {
                    //Assume that value is numerical "double"
                    valueType = Value.Value_Type.Double;
                }
                else
                {
                    //To be implemented
                    //Methods with return statements
                }
            }
            //var value = blah + blah + blah

            dynamic value;
            switch (valueType)
            {
                case Value.Value_Type.Double:
                    value = valueHanlder.ParseDoubleExpression(varStatement.tokens);
                    break;
                case Value.Value_Type.Float:
                    value = valueHanlder.ParseFloatExpression(varStatement.tokens);
                    break;
                case Value.Value_Type.Int:
                    value = valueHanlder.ParseIntExpression(varStatement.tokens);
                    break;
                case Value.Value_Type.Long:
                    value = valueHanlder.ParseLongExpression(varStatement.tokens);
                    break;
                case Value.Value_Type.Bool:
                    value = valueHanlder.ParseBoolExpression(varStatement.tokens,true);
                    //To be implemented (Bool)
                    break;
                case Value.Value_Type.String:
                    value = valueHanlder.ParseStringExpression(varStatement.tokens);
                    break;
                case Value.Value_Type.Other:
                    //Throw exception
                    //To be implemented
                    value = null;
                    break;
                default:
                    //Throw exception
                    //To be implemented
                    return;
            }

            
            if (globalVariables.IsVariable(variableName))
            {
                globalVariables.AddVariable(variableName,value,valueType);
            }
            else
            {
                method.lexer.Variables.AddVariable(variableName, value, valueType);
            }

            
        }

        void PrepareConditionalStatement(ref Statement statement,out string[] boolTokens, out int i)
        {
            //if (value == 15 && value == 16)
            int boolStart = 2, boolEnd = 2;

            
            for (i = 1; i < statement.tokens.Count; i++)
            {
                if (statement.tokens[i] == ")")
                {
                    boolEnd = i;
                    i++;
                    break;
                }
            }

            boolTokens = new string[boolEnd - boolStart];
            statement.tokens.CopyTo(boolStart, boolTokens, 0, boolEnd - boolStart);
        }

        void IfStatement(ref Statement ifStatement)
        {
            //i -> index of {

            PrepareConditionalStatement(ref ifStatement, out string[] boolTokens, out int i);

            if (logicHandler.IntepreteBoolExpression(boolTokens))
            {
                //Remove all tokens before starting of {
                ifStatement.tokens.RemoveRange(0, i + 1);

                ifStatement.internalMethod = new Method(ifStatement.tokens);
                ifStatement.internalVariableHandler = new VariableHandler();
                ifStatement.internalMethodHandler = new MethodHandler(globalVariables, ifStatement.internalMethod);

                ifStatement.RunStatement();
            }
        }

        void WhileLoopStatement(Statement whileLoopStatement)
        {
            //i -> index of {

            PrepareConditionalStatement(ref whileLoopStatement, out string[] boolTokens, out int i);

            //Remove all tokens before starting of {
            whileLoopStatement.tokens.RemoveRange(0, i + 1);
            whileLoopStatement.internalVariableHandler = new VariableHandler();

            whileLoopStatement.internalMethod = new Method(whileLoopStatement.tokens);

            whileLoopStatement.internalMethodHandler = new MethodHandler(globalVariables, whileLoopStatement.internalMethod);

            var doFunc = boolTokens;

            while (logicHandler.IntepreteBoolExpression(boolTokens))
            {
                whileLoopStatement.RunStatement();
            }
        }

    }
}
