using System;
using System.Collections.Generic;

namespace Gnosis
{
    class MethodHandler
    {
        readonly VariableHandler globalVariables;
        public readonly LogicHandler logicHandler;
        readonly ValueHandler valueHanlder;
        Method method; 

        bool IsVariable(string variableName)
        {
            return globalVariables.IsVariable(variableName) || method.lexer.IsVariable(variableName);
        }

        bool IsArray(string variableName)
        {
            return (globalVariables.IsArray(variableName) || method.lexer.IsArray(variableName));
        }

        public MethodHandler(VariableHandler globalVars,  Method m)
        {
            method = m;
            globalVariables = globalVars;

            valueHanlder = new ValueHandler(globalVariables, method);
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
                IntepreteCommand(statement);
            }
                
        }

        public void IntepreteCommand(Statement statement)
        {
            while(statement.tokens.Contains("[") && statement.tokens.Contains("]"))
            {
                for (int i = 0; i < statement.tokens.Count; i++)
                {
                    //elements [ 0 ]
                    if (IsArray(statement.tokens[i]))
                    {
                        //i+1,i+3
                        statement.tokens[i] += statement.tokens[i + 1];
                        statement.tokens[i] += statement.tokens[i + 2];
                        statement.tokens[i] += statement.tokens[i + 3];
                        statement.tokens.RemoveRange(i + 1, 3);
                        break;
                    }
                }
            }
            

            if(statement.tokens.Count == 0) return;
            else if(statement.tokens[0] == "print") Print(statement);
            else if (statement.tokens[0] == "input") Input(statement);
            else if (statement.tokens[0] == "var") VarDeclaration(statement);
            else if (statement.tokens[0] == "if") IfStatement(statement, false);
            else if (statement.tokens[0] == "while") IfStatement(statement, true);
            else if (statement.tokens[0] == "for") ForStatement(statement);
            else if (IsVariable(statement.tokens[0]))
            {
                //variable = blahblah + blah
                //var variable = variable + blahblah + blah

                statement.tokens.Insert(0, "var");
                string variableName = statement.tokens[1];

                //i++
                //var i ++
                //var i = i + 1
                switch (statement.tokens[2])
                {
                    case "=":
                        //Do nothing
                        break;

                    case "+=":
                    case "-=":
                    case "*=":
                    case "/=":
                    case "%=":
                        statement.tokens.InsertRange(3, new List<string>(){ variableName, statement.tokens[2][0].ToString()});
                        break;

                    case "++":
                    case "--":
                        statement.tokens.InsertRange(3, new List<string>() { variableName, statement.tokens[2][0].ToString(), "1", ";" });
                        break;
                }

                statement.tokens[2] = "=";
                IntepreteCommand(statement);
            }
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

        void Input(Statement printStatement)
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

        void VarDeclaration(Statement varStatement)
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

            dynamic value;

            if (valueType == Value.Value_Type.Array)
            {
                var arrayValueType = valueHanlder.ValueType(varStatement.tokens[4]);
                List<dynamic> arr = new List<dynamic>();

                for (int i = 4; i < varStatement.tokens.IndexOf("}"); i += 2)
                    arr.Add(valueHanlder.GetValue(varStatement.tokens[i]));

                Array a = new Array(new Value(arr));

                if (globalVariables.IsVariable(variableName))
                {
                    globalVariables.AddVariable(variableName, a);
                }
                else
                {
                    method.lexer.Variables.AddVariable(variableName, a);
                }

            }

            //var value = blah + blah + blah
            
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

            int openedBrackets = 0;
            for (i = 0; i < statement.tokens.Count; i++)
            {
                if (statement.tokens[i] == "(")
                {
                    openedBrackets++;
                }
                else if (statement.tokens[i] == ")")
                {
                    if (openedBrackets == 0 || openedBrackets == 1)
                    {
                        i++;
                        boolEnd = i;
                        break;
                    }
                    else
                        openedBrackets -= 1;
                }
            }

            boolEnd--;
            boolTokens = new string[boolEnd - boolStart];
            statement.tokens.CopyTo(boolStart, boolTokens, 0, boolEnd - boolStart);
        }

        void IfStatement(Statement statement, bool whileLoop)
        {
            //i -> index of {
            //Code for while statement and if statement merged because of similarity

            PrepareConditionalStatement(ref statement, out string[] boolTokens, out int i);

            int len = statement.tokens.Count - (i + 1); // token count

            statement.internalMethod = new Method(statement.tokens.GetRange(i + 1, len));
            statement.internalVariableHandler = new VariableHandler();
            statement.internalMethodHandler = new MethodHandler(globalVariables, statement.internalMethod);

            while (logicHandler.IntepreteBoolExpression(boolTokens))
            {
                statement.RunStatement();
                if(whileLoop == false) break; // is if statement
                // else continue as while statement
            }
        }

        void ForStatement(Statement statement)
        {
            //i -> index of {
            PrepareConditionalStatement(ref statement, out string[] forRunTokens, out int i);

            List<List<string>> tT = new List<List<string>>();
            List<string> current = new List<string>();

            for (int j = 0; j < forRunTokens.Length; j++)
            {
                if(forRunTokens[j] == "|" || j == forRunTokens.Length - 1)
                {
                    if(j == forRunTokens.Length - 1) 
                        current.Add(forRunTokens[j]);

                    if (forRunTokens[j] == "|")
                    {
                        forRunTokens[j] = ";";
                        if (tT.Count != 1) current.Add(forRunTokens[j]);
                    }
                    tT.Add(current);
                    current = new List<string>();
                }
                else current.Add(forRunTokens[j]);
            }

            //Setup statement
            int len = statement.tokens.Count - (i + 1); // token count
            statement.internalMethod = new Method(statement.tokens.GetRange(i + 1, len));
            statement.internalVariableHandler = new VariableHandler();
            statement.internalMethodHandler = new MethodHandler(globalVariables, statement.internalMethod);


            //Get the three parts of for statement
            string[] boolTokens = tT[1].ToArray();
            statement.internalMethodHandler.IntepreteCommand(new Statement(tT[0])); //Run first part.. eg var i = 0

            while (statement.internalMethodHandler.logicHandler.IntepreteBoolExpression(boolTokens))
            {
                statement.RunStatement();
                statement.internalMethodHandler.IntepreteCommand(new Statement(tT[2])); //Run last part.. eg i++
            }
        }

    }
}
