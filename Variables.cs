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

        public Value(dynamic value)
        {
            this.value = value;
        }
    }
    class VariableHandler
    {
        Dictionary<string,Variable> variables = new Dictionary<string, Variable>();

        public bool IsVariable(string variable)
        {
            return variables.ContainsKey(variable);
        }
        public void AddVariable(string name, string type, bool isArray)
        {
            Variable newVariable = isArray ? new Variable(type) 
                                           : new Array(type);
            variables.Add(name,newVariable);
        }

        public void AddVariable(string name, string type, int maxIndex)
        {
            // Add static array (Native Array)
            variables.Add(name, new Array(type,maxIndex));
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
            variables.Add("endl", new Variable("string"){ value = new Value("\n") });
        }
    }
      
    class Variable
    {
        string[] primitiveTypes = new[] { "string", "int", "double", "long", "float", "bool" };

        string name;
        string type;

        public Value value;

        bool IsPrimitive() => primitiveTypes.Contains(name);
        public bool IsArray = false;

        public Variable(string type)
        {
            this.type = type;
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
