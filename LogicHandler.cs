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
        string[] comparer = new[] { "==", "!=", "<", ">", "<=", ">=", "&&", "||" }; // there may be more in the future


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

        List<string> GetBoolValues(string[] tokens)
        {
            //Get Values from tokens
            // man == Bird || bird == bird
            // -> false || true -> true (Desired use)
            List<string> values = new List<string>();

            //Compare numbers using default number value type (double)

            for (int i = 1; i < tokens.Length; i += 2)
            {
                if (comparer.Contains(tokens[i]))
                {
                    var value = valueHanlder.GetValue(tokens[i - 1]);
                    if (value == null) value = tokens[i - 1];

                    var nextValue = valueHanlder.GetValue(tokens[i + 1]);
                    if (nextValue == null) nextValue = tokens[i + 1];

                    
                    switch (tokens[i])
                    {
                        case "==":
                            values.Add((Convert.ToString(value) == Convert.ToString(nextValue)).ToString());
                            break;

                        case "!=":
                            values.Add((Convert.ToString(value) != Convert.ToString(nextValue)).ToString());
                            break;

                        case "<":
                            values.Add((Convert.ToDouble(value) < Convert.ToDouble(nextValue)).ToString());
                            break;

                        case ">":
                            values.Add((Convert.ToDouble(value) > Convert.ToDouble(nextValue)).ToString());
                            break;

                        case "<=":
                            values.Add((Convert.ToDouble(value) <= Convert.ToDouble(nextValue)).ToString());
                            break;

                        case ">=":
                            values.Add((Convert.ToDouble(value) >= Convert.ToDouble(nextValue)).ToString());
                            break;

                        case "&&":
                        case "||":
                            values.Add(tokens[i]);
                            break;
                    }
                }
                else
                {
                    //Throw Error
                    //Not implemented
                    return null;
                }
            }

            return values;
        }

        public bool IntepreteBoolExpression(string[] tokens)
        {
            if(tokens.Length == 1)
            {
                var value = valueHanlder.GetValue(tokens[0]);

                return bool.Parse(value);
            }

            List<string> values = GetBoolValues(tokens);


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
