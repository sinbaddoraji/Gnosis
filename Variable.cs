using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class Variable
    {
        private readonly string[] primitiveTypes = new[] { "string", "int", "double", "long", "float", "bool" };
        // value types : double, float, int, long, bool, string
        // double -> 0
        //  float -> 1
        //    int -> 2
        //   long -> 3
        //   bool -> 4
        // string -> 5
        //  other -> 6;

        public int ValueType()
        {
            for (int i = 0; i < primitiveTypes.Length; i++)
            {
                if(value.type ==  primitiveTypes[i]) return i;
            }

            return 6;
        }

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
