using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math;

namespace Gnosis
{
    class ValueHandler
    {
        //This class handles variables and raw values for the method handler
        private readonly MathEngine mathEngine = new MathEngine();
        private VariableHandler OuterVariables;
        public VariableHandler InnerVariables => method.lexer.Variables;
        public VariableHandler GlobalVariables => EntryPoint.globalVariableHandler;

        public LogicHandler logicHandler;

        private readonly Method method;

        public bool IsVariable(string value)
        {
            return GlobalVariables.IsVariable(value) || InnerVariables.IsVariable(value) || OuterVariables.IsVariable(value);
        }

        public bool IsArray(string value)
        {
            return GlobalVariables.IsArray(value) || InnerVariables.IsArray(value) || OuterVariables.IsArray(value);
        }


        public Value.Value_Type ValueType(string value)
        {
            //Find out if checking variable
            //if checking variable return variable value type
            //else return variable type
            if (value == "{" || IsArray(value))
            {
                return Value.Value_Type.Array;
            }
            if (InnerVariables.IsVariable(value))
            {
                return InnerVariables.GetVariable(value).ValueType();
            }
            else if (OuterVariables != null && OuterVariables.IsVariable(value))
            {
                return OuterVariables.GetVariable(value).ValueType();
            }
            else if (GlobalVariables.IsVariable(value))
            {
                return GlobalVariables.GetVariable(value).ValueType();
            }
            else return Value.ValueType(value);
        }

        public dynamic ParseNumberExpression(string expression, MathEngine.ReturnType rt)
        {
            return mathEngine.Express(expression,rt);
        }

        public double ParseDoubleExpression(List<string> tokens, bool fromVariable = false)
        {
            var t = fromVariable ? tokens.GetRange(3, tokens.Count - 4) : tokens;

            string expression = string.Join("", t); var rt = MathEngine.ReturnType.Double;

            return (double)ParseNumberExpression(expression,rt);
            //var value = blah + blah + blah
        }

        public float ParseFloatExpression(List<string> tokens, bool fromVariable = false)
        {
            var t = fromVariable ? tokens.GetRange(3, tokens.Count - 4) : tokens;

            string expression = string.Join("", t); var rt = MathEngine.ReturnType.Float;

            return (float)ParseNumberExpression(expression, rt);
            //var value = blah + blah + blah
        }

        public int ParseIntExpression(List<string> tokens, bool fromVariable = false)
        {
            string expression = string.Join("", tokens.GetRange(3, tokens.Count - 4));
            var rt = MathEngine.ReturnType.Int;

            return (int)ParseNumberExpression(expression, rt);
            //var value = blah + blah + blah
        }

        public long ParseLongExpression(List<string> tokens, bool fromVariable = false)
        {
            var t = fromVariable ? tokens.GetRange(3, tokens.Count - 4) : tokens;

            string expression = string.Join("", t);
            var rt = MathEngine.ReturnType.Long;

            return (long)ParseNumberExpression(expression, rt);
            //var value = blah + blah + blah
        }

        public string ParseStringExpression(List<string> t, bool fromVariable = false)
        {
            List<string> tokens = fromVariable ? t.GetRange(3, t.Count - 4) : t;
            

            string output = "";

            for (int i = 0; i < tokens.Count; i++)
            {
                var next = GetValue(tokens[i]);

                if(next == null)
                {
                    //Throw exception
                    //To be implemented
                }
                else
                {
                    output += next;
                }
            }

            return output;
            //var value = blah + blah + blah
        }


        public string ParseBoolExpression(List<string> t, bool inStatement = false)
        {
            //Check here if having boolean issues
            List<string> tokens = new List<string>(t);

            if(inStatement)
            {
                tokens.RemoveRange(0, 4);
                tokens.RemoveRange(tokens.Count - 2,2);
            }
            else
            {
                tokens.RemoveRange(0, 3);
                tokens.RemoveAt(tokens.Count - 1);
            }

            return logicHandler.IntepreteBoolExpression(tokens.ToArray()).ToString();
            //var value = blah + blah + blah
        }

        public dynamic GetArray(string name, int index)
        {
            if (InnerVariables.IsArray(name))
            {
                return method.lexer.Variables.GetArray(name,index);
            }
            else if (OuterVariables.IsArray(name))
            {
                return OuterVariables.GetArray(name, index);
            }
            else if (GlobalVariables.IsArray(name))
            {
                return GlobalVariables.GetArray(name,index);
            }
            else if (ValueType(name) == Value.Value_Type.String)
            {
                string str = GetValue(name);
                return str[index];
            }
            else
            {
                //Throw error
                //To be implemented
                return -02030303;
            }
        }

        public dynamic GetValue(string value)
        {

            if (value.StartsWith("$"))
            {
                string val = value.Substring(1, value.Length - 1);

                if (IsArray(val))
                {
                    List<object> _ = new List<object>();
                    if (InnerVariables.IsArray(val))
                    {
                        _ = (List<object>)method.lexer.Variables.GetVariable(val).value.value;
                    }
                    else if (OuterVariables.IsArray(val))
                    {
                        _ = (List<object>)OuterVariables.GetVariable(val).value.value;
                    }
                    else if (GlobalVariables.IsArray(val))
                    {
                        _ = (List<object>)GlobalVariables.GetVariable(val).value.value;
                    }
                    
                    return _.Count;
                }
                else
                {
                    string rawValue = Convert.ToString(GetValue(val));
                    return rawValue.Length;
                }
            }
            else if(value.Contains("["))
            {
                //elements[0]
                var t = value.Split('[',']');
                var vName = t[0];
                var index = Convert.ToInt32(GetValue(t[1]));

                if(!IsArray(vName)) return GetValue(vName)[index];
                else return GetArray(vName,index);
            }
            else if (Lexer.IsString(value))
            {
                return value.Substring(1, value.Length - 2)
                    .Replace("\\a", "\a")
                    .Replace("\\b", "\b")
                    .Replace("\\f", "\f")
                    .Replace("\\n", "\n")
                    .Replace("\\r", "\r")
                    .Replace("\\t", "\t")
                    .Replace("\\v", "\v");
            }
            else if (InnerVariables.IsVariable(value))
            {
                return method.lexer.Variables.GetVariable(value).value.value;
            }
            else if (OuterVariables != null && OuterVariables.IsVariable(value))
            {
                return OuterVariables.GetVariable(value).value.value;
            }
            else if (GlobalVariables.IsVariable(value))
            {
                return GlobalVariables.GetVariable(value).value.value;
            }
            else if (EntryPoint.Methods.ContainsKey(value))
            {
                Method m = EntryPoint.Methods[value];
                MethodHandler mh = new MethodHandler(null, m);
                mh.DoFunction(m);

                return mh.returned;
            }
            else if(Value.IsNumber(value))
            {
                return value;
            }
            else
            {
                //To be implemented
                //Throw exception
                return null;
            }
        }

        public ValueHandler(VariableHandler oV, Method m)
        {
            OuterVariables = oV;
            method = m;

            // Variable handler method for the math evaluator (mathengine)
            mathEngine.externalGet = GetValue; 
        }
    }
}
