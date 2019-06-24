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
        MathEngine mathEngine = new MathEngine();
        VariableHandler globalVariables;
        Method method;

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

            return ParseNumberExpression(expression,rt);
            //var value = blah + blah + blah
        }

        public float ParseFloatExpression(List<string> tokens)
        {
            tokens.RemoveRange(0, 3);
            tokens.RemoveAt(tokens.Count - 1);

            string expression = string.Join("", tokens);
            var rt = MathEngine.ReturnType.Float;

            return ParseNumberExpression(expression, rt);
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

            return ParseNumberExpression(expression, rt);
            //var value = blah + blah + blah
        }

        public string ParseStringExpression(List<string> tokens)
        {
            tokens.RemoveRange(0, 3);
            tokens.RemoveAt(tokens.Count - 1);

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
                    output += GetValue(tokens[i]);
                }
            }

            return output;
            //var value = blah + blah + blah
        }

        public dynamic GetValue(string value)
        {
            if (Lexer.IsString(value))
            {
                return value.Substring(1, value.Length - 2);
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
