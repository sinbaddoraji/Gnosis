using System;
using System.Collections.Generic;
using System.Linq;

namespace Gnosis
{
    class MethodHandler
    {
        readonly VariableHandler OuterVariables;
        public VariableHandler InnerVariables => method.lexer.Variables;
        public VariableHandler GlobalVariables => EntryPoint.globalVariableHandler;

        public readonly LogicHandler logicHandler;
        readonly ValueHandler valueHanlder;
        Method method; 

        public dynamic returned = null;
        bool IsVariable(string variableName)
        {
            return  GlobalVariables.IsVariable(variableName) || (OuterVariables != null && OuterVariables.IsVariable(variableName)) || InnerVariables.IsVariable(variableName);
        }

        bool IsArray(string variableName)
        {
            return GlobalVariables.IsArray(variableName) || (OuterVariables != null && OuterVariables.IsArray(variableName)) || InnerVariables.IsArray(variableName);
        }

        bool IsArrayPointer(string value) => IsArray(value.Split('[')[0]);

        int NextIndexOfBracket(Statement statement, int currentIndex)
        {
            //currentIndex => index of '['

            int isOpen = 0; //bracket is open

            for (int i = currentIndex; i < statement.tokens.Count; i++)
            {
                if (statement.tokens[i] == "[") isOpen++;
                if (statement.tokens[i] == "]") isOpen--;
                if (isOpen == 0) return i;
            }
            return 0;
        }


        public void DoFunction(Method method)
        {
            this.method = method;

            //Run through every statement
            for (int i = 0; i < this.method.lot.Count; i++)
            {
                if(returned != null)break;
                var statement = this.method.lot[i];
                IntepreteCommand(statement);
            }
        }

        private void CleanForArray(Statement statement)
        {
            while (statement.tokens.Contains("[") && statement.tokens.Contains("]"))
            {
                int start, end;

                for (int i = 0; i < statement.tokens.Count; i++)
                {
                    //elements [ 0 ]
                    if (IsArray(statement.tokens[i]))
                    {
                        //i+1,i+3
                        start = statement.tokens.IndexOf("[",i);
                        end = NextIndexOfBracket(statement,start);

                        for (int k = start; k <= end; k++)
                            statement.tokens[i] += statement.tokens[k];

                        statement.tokens.RemoveRange(start, end - start + 1);
                        break;
                    }
                }
            }
        }


        public void IntepreteCommand(Statement statement)
        {
            CleanForArray(statement);

            if (statement.tokens.Count == 0) return;
            else if (statement.tokens[0] == "print") Print(statement);
            else if (statement.tokens[0] == "input") Input(statement);
            else if (statement.tokens[0] == "var") VarDeclaration(statement);
            else if (statement.tokens[0] == "if") IfStatement(statement, false);
            else if (statement.tokens[0] == "while") IfStatement(statement, true);
            else if (statement.tokens[0] == "for") ForStatement(statement);
            else if (statement.tokens[0] == "return")
            {
                var valueType = valueHanlder.ValueType(statement.tokens[1]);
                int lem = statement.tokens.Count;

                returned = ExpressStatement(statement.tokens.GetRange(1,lem - 2),valueType, false);
                return;
            }
            else if (IsVariable(statement.tokens[0])) ShortHandStatement(statement);
            else if (IsArrayPointer(statement.tokens[0])) ArrayValueChangeStatement(statement);
            else if (statement.tokens.Count == 2)
            {
                //If single statement like "pause"
                string command = statement.tokens[0];
                if (command == "cls")
                {
                    Console.Clear();
                }
                if (command == "pause")
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else if (EntryPoint.Methods.ContainsKey(command))
                {
                    Method m = EntryPoint.Methods[command];
                    MethodHandler mh = new MethodHandler(OuterVariables,m);
                    mh.DoFunction(m);
                    
                }
            }
        }

        void Print(Statement statement)
        {
            string output = "";

            List<string> internalTokens = new List<string>();

            
            for (int i = 1; i < statement.tokens.Count; i++)
            {
                string token = statement.tokens[i];

                if (token == "<<" || token == ";")
                {
                    output += valueHanlder.ParseStringExpression(internalTokens, false);
                    internalTokens.Clear();
                }
                else internalTokens.Add(token);
            }

            Console.Write(output);
        }

        void Input(Statement statement)
        {
            //input >> variable;
            //input public >> variable;
            //input "Display string" >> variable
            //input public "Display string" >> variable;

            string variableName;
            string value;
            bool isPublic = false;

            if (statement.tokens.Count == 4)
            {
                //if input does not have display message
                variableName = statement.tokens[2];
            }
            else if (statement.tokens.Count == 5)
            {
                //if input has a display message
                if(statement.tokens[1] == "public")
                {
                    variableName = statement.tokens[3];
                    isPublic = true;
                }
                else
                {
                    string display = statement.tokens[1];
                    Console.Write(display.Substring(1, display.Length - 2));

                    variableName = statement.tokens[3];
                }
            }
            else if (statement.tokens.Count == 6)
            {
                //if input is public var
                isPublic = true;
                string display = statement.tokens[2];
                Console.Write(display.Substring(1, display.Length - 2));

                variableName = statement.tokens[4];
            }
            else
            {
                //Throw exception
                //To be implemented
                return;
            }

            value = Console.ReadLine();

            if (isPublic)
            {
                GlobalVariables.AddVariable(variableName,value);
            }
            else
            {
                InnerVariables.AddVariable(variableName,value);
            }
        }

        private void SetupArrayVariable(Statement statement, string variableName, bool isPublic)
        {
            List<dynamic> arr = new List<dynamic>();

            if (isPublic) statement.tokens.RemoveAt(1);

            var bList = new[] { "{", "}", ",", "+" };
            for (int i = 3; i < statement.tokens.Count; i++)
            {
                var cur = statement.tokens[i];
                if(!bList.Contains(cur))
                {
                    if (IsArray(cur)) arr.AddRange((List<object>)valueHanlder.GetValue(cur));
                    else arr.Add(valueHanlder.GetValue(cur));
                }
            }

            Array a = new Array(new Value(arr, VariableHandler.ProcessedValue(Convert.ToString(arr[0])).type));

            if(isPublic) GlobalVariables.AddVariable(variableName,a);
            else if (OuterVariables != null && OuterVariables.IsVariable(variableName)) OuterVariables.AddVariable(variableName, a);
            else InnerVariables.AddVariable(variableName, a);
        }

        private dynamic ExpressStatement(List<string> tokens, Value.Value_Type valueType, bool fromVariable = true)
        {
            dynamic value;

            switch (valueType)
            {
                case Value.Value_Type.Double:
                    value = valueHanlder.ParseDoubleExpression(tokens, fromVariable);
                    break;
                case Value.Value_Type.Float:
                    value = valueHanlder.ParseFloatExpression(tokens, fromVariable);
                    break;
                case Value.Value_Type.Int:
                    value = valueHanlder.ParseIntExpression(tokens, fromVariable);
                    break;
                case Value.Value_Type.Long:
                    value = valueHanlder.ParseLongExpression(tokens, fromVariable);
                    break;
                case Value.Value_Type.Bool:
                    value = valueHanlder.ParseBoolExpression(tokens, fromVariable);
                    //To be implemented (Bool)
                    break;

                case Value.Value_Type.String:
                    value = valueHanlder.ParseStringExpression(tokens, fromVariable);
                    break;

                case Value.Value_Type.Other:
                    //Throw exception
                    //To be implemented
                    value = null;
                    break;
                default:
                    //Throw exception
                    //To be implemented
                    value = null;
                    break;
            }

            return value;
        }

        private void SetupNormalVariable(Statement statement, string variableName, Value.Value_Type valueType, bool isPublic)
        {
            dynamic value = ExpressStatement(statement.tokens, valueType);


            if (isPublic) GlobalVariables.AddVariable(variableName, value, valueType);
            else if (OuterVariables != null && OuterVariables.IsVariable(variableName)) OuterVariables.AddVariable(variableName, value, valueType);
            else InnerVariables.AddVariable(variableName, value, valueType);
        }

        void VarDeclaration(Statement statement)
        {
            //Variable type will be based on the first variable in statement (tokens[3])
            string variableName;
            Value.Value_Type valueType; 

            bool isPublic = false;
            if(statement.tokens[1] == "public")
            {
                isPublic = true;
                //var public ma = ndjnfr;
                variableName = statement.tokens[2];
                valueType = valueHanlder.ValueType(statement.tokens[4]);
            }
            else
            {
                variableName = statement.tokens[1];
                valueType = valueHanlder.ValueType(statement.tokens[3]);
            }
            

            //if given "other" value type.. find out if is actually numeical or boolean value
            if (valueType == Value.Value_Type.Other)
            {
                if (statement.tokens.Contains("!=")) valueType = Value.Value_Type.Bool;
                else if (statement.tokens.Contains("==")) valueType = Value.Value_Type.Bool;
                else if (statement.tokens[3] == "(")
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


            if (valueType == Value.Value_Type.Array) 
                SetupArrayVariable(statement, variableName, isPublic);
            else 
                SetupNormalVariable(statement, variableName, valueType, isPublic);

        }
        
        void ArrayValueChangeStatement(Statement statement)
        {
            //elements[$elements-1] = "Nothing else";
            string variableName = statement.tokens[0].Split('[')[0];

            int indexOfVName = statement.tokens[0].IndexOf(variableName) + variableName.Length + 1;
            int indexOfLastBrack = statement.tokens[0].LastIndexOf(']');

            string pointerIndex = statement.tokens[0].Substring(indexOfVName,indexOfLastBrack - indexOfVName);
            int pointerIndexInt = (int)valueHanlder.ParseNumberExpression(pointerIndex,Math.MathEngine.ReturnType.Int);

            Array arr = null;
            if (InnerVariables.IsArray(variableName))
            {
                arr = (Array)InnerVariables.GetVariable(variableName);
               
            }
            else if (OuterVariables.IsArray(variableName))
            {
                arr = (Array)OuterVariables.GetVariable(variableName);
            }
            else if (GlobalVariables.IsArray(variableName))
            {
                arr = (Array)GlobalVariables.GetVariable(variableName);
            }
            else
            {
                //Throw
                //To be implemented
            }
            
            List<string> internalTokens = new List<string>();
            for (int i = 2; i < statement.tokens.Count -1; i++)
            {
                internalTokens.Add(statement.tokens[i]);
            }

            dynamic value = valueHanlder.ParseStringExpression(internalTokens, false);
            arr.SetValue(value,pointerIndexInt);
        }

        void ShortHandStatement(Statement statement)
        {
            //variable = blahblah + blah
            //var variable = variable + blahblah + blah

            statement.tokens.Insert(0, "var");
            string variableName = statement.tokens[1];

            if(GlobalVariables.IsVariable(variableName))
                 statement.tokens.Insert(1, "public");

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
                    statement.tokens.InsertRange(3, new List<string>() { variableName, statement.tokens[2][0].ToString() });
                    break;

                case "++":
                case "--":
                    statement.tokens.InsertRange(3, new List<string>() { variableName, statement.tokens[2][0].ToString(), "1", ";" });
                    break;
            }

            statement.tokens[2] = "=";
            IntepreteCommand(statement);
        }

        #region Statement


        void PrepareConditionalStatement(ref Statement statement, out string[] boolTokens, out int i)
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
            statement.internalMethodHandler = new MethodHandler(InnerVariables, statement.internalMethod);

            while (logicHandler.IntepreteBoolExpression(boolTokens))
            {
                statement.RunStatement();
                if (whileLoop == false) break; // is if statement
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
                if (forRunTokens[j] == "|" || j == forRunTokens.Length - 1)
                {
                    if (j == forRunTokens.Length - 1)
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
            statement.internalMethodHandler = new MethodHandler(InnerVariables, statement.internalMethod);


            //Get the three parts of for statement
            string[] boolTokens = tT[1].ToArray();
            statement.internalMethodHandler.IntepreteCommand(new Statement(tT[0])); //Run first part.. eg var i = 0

            while (statement.internalMethodHandler.logicHandler.IntepreteBoolExpression(boolTokens))
            {
                statement.RunStatement();
                statement.internalMethodHandler.IntepreteCommand(new Statement(tT[2])); //Run last part.. eg i++
            }
        }


        #endregion

        public MethodHandler(VariableHandler outerVariables, Method m)
        {
            method = m;
            OuterVariables = outerVariables;

            valueHanlder = new ValueHandler(OuterVariables, method);
            logicHandler = new LogicHandler(valueHanlder);

            valueHanlder.logicHandler = logicHandler;

            InnerVariables.OuterVariables = outerVariables;
        }
    }
}
