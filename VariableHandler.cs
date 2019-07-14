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
            //< 1 (false), > 0 (true)

            Value val;

            if (Value.IsDouble(value)) val = new Value(double.Parse(value), Value.Value_Type.Double);
            else if (Value.IsFloat(value)) val = new Value(float.Parse(value), Value.Value_Type.Float);
            else if (Value.IsInt(value)) val = new Value(int.Parse(value), Value.Value_Type.Int);
            else if (Value.IsLong(value)) val = new Value(long.Parse(value), Value.Value_Type.Long);
            else if (Value.IsBool(value)) val = new Value(bool.Parse(value), Value.Value_Type.Bool);
            else if (Value.IsString(value)) val = new Value(value.Substring(0, value.Length - 1), Value.Value_Type.String);
            else val = new Value(value);

            return val;
        }

        public void AddArrayVariable(string name, string value, string type, int maxIndex)
        {
            //Add static array (Native Array)
            //variables.Add(name, new Array(type,maxIndex));
            //To be implemented
        }

        private void AddVariable(string name, Variable v)
        {
            //If variables contains variable with "name"
            //      edit the variable
            //else 
            //      add variable

            if (variables.ContainsKey(name))
                variables[name] = v;
            else
                variables.Add(name, v);
        }

        public void AddVariable(string name, string value)
        {
            var v = new Variable(ProcessedValue(value));
            AddVariable(name,v);
        }

        public void AddVariable(string name, dynamic value, Value.Value_Type valueType)
        {
            var v = new Variable(new Value(value, valueType));
            AddVariable(name, v);
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
            variables.Add("true", new Variable(new Value(true)));
            variables.Add("false", new Variable(new Value(false)));
        }
    }
}
