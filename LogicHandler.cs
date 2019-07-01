using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class LogicHandler
    {
        public ValueHandler valueHanlder;
        // (value == 34 || value == 15)
        //Operators:
        //  ! -> NOT
        // && -> AND
        // || -> OR
        string[] operators = new[] { "!", "&&", "||" }; // there may be more in the future
        string[] comparer = new[] { "==", "!=" }; // there may be more in the future


        bool IsOperator(string op) => operators.Contains(op);

        string GetInvariantBool(string str)
        {
            try
            {
                return (!bool.Parse(str)).ToString();
            }
            catch
            {
                return "False";
            }
        }

        public bool IntepreteBoolExpression(string[] tokens)
        {
            if(tokens.Length == 1)
            {
                var value = valueHanlder.GetValue(tokens[0]);

                return bool.Parse(value);
            }

            List<string> values = new List<string>();

            //value == 15 && value == 16
            for (int i = 0; i < tokens.Length; i++)
            {
               var value = valueHanlder.GetValue(tokens[i]);

                if (value != null)
                    tokens[i] = Convert.ToString(value);
                else if (comparer.Contains(tokens[i]))
                {
                    var nextValue = valueHanlder.GetValue(tokens[i + 1]);
                    if (nextValue != null) tokens[i + 1] = Convert.ToString(nextValue);

                    if (tokens[i] == "==")
                    {
                        values.Add((tokens[i - 1] == tokens[i + 1]).ToString());
                    }
                    else if (tokens[i] == "!=")
                    {
                        values.Add((tokens[i - 1] != tokens[i + 1]).ToString());
                    }
                    i++; // Skip next comparison (no need)
                }
                else if (operators.Contains(tokens[i]))
                {
                    if (tokens[i] == "!")
                    {
                        tokens[i + 1] = GetInvariantBool(tokens[i + 1]);
                        values.Add(tokens[i + 1]);
                        i++;
                    }
                    else values.Add(tokens[i]);
                }
                
            }

            bool output = bool.Parse(values[0]);

            for (int i = 1; i < values.Count; i++)
            {
                if(values[i] == "&&")
                {
                    i++;
                    output &= bool.Parse(values[i]);
                }
                else if (values[i] == "||")
                {
                    i++;
                    output = output || bool.Parse(values[i]);
                }
            }

            return output;
        }

        public LogicHandler(ValueHandler vH)
        {
            valueHanlder = vH;
        }

    }
}
