using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class LogicHandler
    {
        ValueHandler valueHanlder;
        // (value == 34 || value == 15)
        //Operators:
        //  ! -> NOT
        // && -> AND
        // || -> OR
        string[] operators = new[] { "!", "&&", "||" }; // there may be more in the future
        string[] comparer = new[] { "==", "!=" }; // there may be more in the future


        bool IsOperator(string op) => operators.Contains(op);

        bool GetBoolEquivalent(string token)
        {
            if(token == "1") return true;
            return false;
        }

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

        bool IntepreteBoolExpression(List<string> tokens)
        {
            List<string> values = new List<string>();

            //value == 15 && value == 16
            for (int i = 0; i < tokens.Count; i++)
            {
               var value = valueHanlder.GetValue(tokens[i]);

                if (value != null)
                    tokens[i] = Convert.ToString(value);
                else if (comparer.Contains(tokens[i]))
                {
                    if(i + 1 != tokens.Count - 1)
                    {
                        var nextValue = valueHanlder.GetValue(tokens[i + 1]);
                        if (nextValue != null) tokens[i+1] = Convert.ToString(nextValue);

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

            return false;
        }

        public LogicHandler(ValueHandler vH)
        {
            valueHanlder = vH;
        }

    }
}
