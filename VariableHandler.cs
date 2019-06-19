using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class VariableHandler
    {
        Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        public bool IsVariable(string variable) => variables.ContainsKey(variable);

        public static Value ProcessedValue(string value)
        {
            // value types : double, float, int, bool, string
            // double -> 0
            //  float -> 1
            //    int -> 2
            //   bool -> 3
            // string -> 4

            //< 1 (false), > 0 (true)

            Value val;

            if (Value.IsDouble(value)) val = new Value(double.Parse(value), 0);
            else if (Value.IsFloat(value)) val = new Value(float.Parse(value), 1);
            else if (Value.IsInt(value)) val = new Value(int.Parse(value), 2);
            else if (Value.IsBool(value)) val = new Value(bool.Parse(value), 3);
            else if (Value.IsString(value)) val = new Value(value.Substring(0, value.Length - 1), 4);
            else val = new Value(value);

            return val;
        }

        public void AddArrayVariable(string name, string value, string type, int maxIndex)
        {
            //Add static array (Native Array)
            //variables.Add(name, new Array(type,maxIndex));
            //To be implemented
        }

        public void AddVariable(string name, string value)
        {
            var v = new Variable(ProcessedValue(value));

            //If variables contains variable with "name"
            //      edit the variable
            //else 
            //      add variable

            if (variables.ContainsKey(name))
                variables[name] = v;
            else
                variables.Add(name, v);
        }

        public Variable GetVariable(string name, string type)
        {
            return null; //To be implemented
        }
        public Variable GetVariable(string name)
        {
            return variables[name];
        }

        public dynamic GetArray(string name, string type, int index)
        {
            Array array = (Array)GetVariable(name, type);

            return array.GetValue(index);
        }

        public VariableHandler()
        {
            variables.Add("endl", new Variable("\n", true));
        }
    }
}
