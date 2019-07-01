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
        private MathEngine mathEngine = new MathEngine();
        private VariableHandler globalVariables;
        public LogicHandler logicHandler;
        private Method method;

        public int ValueType(string value)
        {
            //Find out if checking variable
            //if checking variable return variable value type
            //else return variable type
            if (method.lexer.isVariable(value))
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
            tokens.RemoveRange(0,3);
            tokens.RemoveAt(tokens.Count - 1);

            string expression = string.Join("",tokens);
            var rt = MathEngine.ReturnType.Double;

            return (double)ParseNumberExpression(expression,rt);
            //var value = blah + blah + blah
        }

        public float ParseFloatExpression(List<string> tokens)
        {
            tokens.RemoveRange(0, 3);
            tokens.RemoveAt(tokens.Count - 1);

            string expression = string.Join("", tokens);
            var rt = MathEngine.ReturnType.Float;

            return (float)ParseNumberExpression(expression, rt);
            //var value = blah + blah + blah
        }

        public int ParseIntExpression(List<string> tokens)
        {
            tokens.RemoveRange(0, 3);
            tokens.RemoveAt(tokens.Count - 1);

            string expression = string.Join("", tokens);
            var rt = MathEngine.ReturnType.Int;

            return (int)ParseNumberExpression(expression, rt);
            //var value = blah + blah + blah
        }

        public long ParseLongExpression(List<string> tokens)
        {
            tokens.RemoveRange(0, 3);
            tokens.RemoveAt(tokens.Count - 1);

            string expression = string.Join("", tokens);
            var rt = MathEngine.ReturnType.Long;

            return (long)ParseNumberExpression(expression, rt);
            //var value = blah + blah + blah
        }

        public string ParseStringExpression(List<string> tokens, bool fromVariable = true)
        {
            if(fromVariable)
            {
                tokens.RemoveRange(0, 3);
                tokens.RemoveAt(tokens.Count - 1);
            }
            

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

        public string ParseBoolExpression(List<string> tokens, bool inStatement = false)
        {
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


        public dynamic GetValue(string value)
        {
            if (Lexer.IsString(value))
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
            else if (method.lexer.isVariable(value))
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

        public ValueHandler(ref VariableHandler gV, ref Method m)
        {
            globalVariables = gV;
            method = m;

            // Variable handler method for the math evaluator (mathengine)
            mathEngine.externalGet = GetValue; 
        }
    }
}
