using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class Variable
    {
        public Value.Value_Type ValueType() => value.type;

        public Value value;

        public bool IsArray = false;

        public Variable(Value value) => this.value = value;

        public Variable(string value, bool rawString = false) => 
            this.value = rawString ? new Value(value, Value.Value_Type.String)
                : VariableHandler.ProcessedValue(value);
    }

    class Array : Variable
    {
        public Array(Value value) : base(value)
        {
            IsArray = true;
        }

        public dynamic GetValue(int index) => ((List<object>)value.value)[index];

        public void SetValue(object val, int index) => ((List<object>)value.value)[index] = val;
    }
}
