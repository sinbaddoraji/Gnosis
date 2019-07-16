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
        private VariableHandler globalVariables;
        public LogicHandler logicHandler;
        private readonly Method method;

        public bool IsVariable(string value)
        {
            return method.lexer.IsVariable(value) || globalVariables.IsVariable(value);
        }

        public bool IsArray(string value)
        {
            return method.lexer.IsArray(value) || globalVariables.IsArray(value);
        }


        public Value.Value_Type ValueType(string value)
        {
            //Find out if checking variable
            //if checking variable return variable value type
            //else return variable type
            if (value == "{")
            {
                return Value.Value_Type.Array;
            }
            if (method.lexer.IsVariable(value))
            {
                return method.lexer.Variables.GetVariable(value).ValueType();
            }
            else if (globalVariables.IsVariable(value))
            {
                return globalVariables.GetVariable(value).ValueType();
            }
            else return Value.ValueType(value);
        }

        public dynamic ParseNumberExpression(string expression, MathEngine.ReturnType rt)
        {
            return mathEngine.Express(expression,rt);
        }

        public double ParseDoubleExpression(List<string> tokens)
        {
            string expression = string.Join("", tokens.GetRange(3, tokens.Count - 4));
            var rt = MathEngine.ReturnType.Double;

            return (double)ParseNumberExpression(expression,rt);
            //var value = blah + blah + blah
        }

        public float ParseFloatExpression(List<string> tokens)
        {
            string expression = string.Join("", tokens.GetRange(3, tokens.Count - 4));
            var rt = MathEngine.ReturnType.Float;

            return (float)ParseNumberExpression(expression, rt);
            //var value = blah + blah + blah
        }

        public int ParseIntExpression(List<string> tokens)
        {
            string expression = string.Join("", tokens.GetRange(3, tokens.Count - 4));
            var rt = MathEngine.ReturnType.Int;

            return (int)ParseNumberExpression(expression, rt);
            //var value = blah + blah + blah
        }

        public long ParseLongExpression(List<string> tokens)
        {
            tokens.RemoveRange(0, 3);
            tokens.RemoveAt(tokens.Count - 1);

            string expression = string.Join("", tokens.GetRange(3, tokens.Count - 4));
            var rt = MathEngine.ReturnType.Long;

            return (long)ParseNumberExpression(expression, rt);
            //var value = blah + blah + blah
        }

        public string ParseStringExpression(List<string> t, bool fromVariable = true)
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
            if (method.lexer.IsArray(name))
            {
                return method.lexer.Variables.GetArray(name,index);
            }
            else if (globalVariables.IsArray(name))
            {
                return globalVariables.GetArray(name,index);
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
                    if (method.lexer.IsArray(val))
                    {
                        _ = (List<object>)method.lexer.Variables.GetVariable(val).value.value;
                    }
                    else if (globalVariables.IsArray(val))
                    {
                        _ = (List<object>)globalVariables.GetVariable(val).value.value;
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
            else if (method.lexer.IsVariable(value))
            {
                return method.lexer.Variables.GetVariable(value).value.value;
            }
            else if (globalVariables.IsVariable(value))
            {
                return globalVariables.GetVariable(value).value.value;
            }
            else if(Value.IsNumber(value))
            {
                return value;
            }
            else return null;
        }

        public ValueHandler(VariableHandler gV, Method m)
        {
            globalVariables = gV;
            method = m;

            // Variable handler method for the math evaluator (mathengine)
            mathEngine.externalGet = GetValue; 
        }
    }
}
