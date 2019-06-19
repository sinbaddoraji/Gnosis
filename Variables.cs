using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class Value
    {
        public string type;
        public dynamic value;

        private string[] vType = new[] { "double", "float", "int", "bool", "string" };

        public static bool IsString(string val) => val.StartsWith("\"") && val.EndsWith("\"");

        public static bool IsDouble(string val) => double.TryParse(val, out double d);

        public static bool IsFloat(string val) => float.TryParse(val, out float f);

        public static bool IsInt(string val) => int.TryParse(val, out int i);

        public static bool IsBool(string val) => bool.TryParse(val, out bool b);

        // value types : double, float, int, bool, string
        // double -> 0
        //  float -> 1
        //    int -> 2
        //   bool -> 3
        // string -> 4

        public Value(dynamic value)
        {
            this.value = value;
        }

        public Value(dynamic value, int valueType) 
        {
            this.value = value;
            type = vType[valueType];
        }

    }
    class VariableHandler
    {
        Dictionary<string,Variable> variables = new Dictionary<string, Variable>();

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

            Value val = null;

            if (Value.IsDouble(value)) val = new Value(double.Parse(value), 0);
            else if (Value.IsFloat(value)) val = new Value(float.Parse(value), 1);
            else if (Value.IsInt(value)) val = new Value(int.Parse(value), 2);
            else if (Value.IsBool(value)) val = new Value(bool.Parse(value), 3);
            else if (Value.IsString(value)) val = new Value(value.Substring(0, value.Length - 1), 4);
            
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

            if(variables.ContainsKey(name))
                variables[name] = v;
            else
                variables.Add(name, v);
        }

        public Variable GetVariable(string name,string type)
        {
            return null; //To be implemented
        }
        public Variable GetVariable(string name)
        {
           return variables[name];
        }

        public dynamic GetArray(string name, string type, int index)
        {
            Array array = (Array)GetVariable(name,type);

            return array.GetValue(index);
        }

        public VariableHandler()
        {
            variables.Add("endl", new Variable("\n",true));
        }
    }
      
    class Variable
    {
        string[] primitiveTypes = new[] { "string", "int", "double", "long", "float", "bool" };

        public Value value;

        bool IsPrimitive() => primitiveTypes.Contains(value.type);
        public bool IsArray = false;

        public Variable(Value value)
        {
            this.value = value;
        }

        public Variable(string value, bool rawString = false)
        {
            this.value = rawString ? new Value(value,4) 
                : VariableHandler.ProcessedValue(value);
        }


    }

    class Array : Variable
    {
        //if array length specified then array becomes static
        //else it acts as a list

        public bool isStaticArray = false;
        public Array(string type) : base(type)
        {
            IsArray = true;
        }

        public Array(string type,int maxLength) : this(type)
        {
            isStaticArray = true;
        }

        public dynamic GetValue(int index)
        {
            return null;
        }
    }
}
