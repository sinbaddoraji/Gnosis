using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class Value
    {
        public Value_Type type;
        public dynamic value;

        public enum Value_Type { Double, Float, Int, Long, Bool, String, Character, Array, Other };

        public static bool IsDouble(string val) => double.TryParse(val, out double d);

        public static bool IsFloat(string val) => float.TryParse(val, out float f);

        public static bool IsInt(string val) => int.TryParse(val, out int i);

        public static bool IsLong(string val) => long.TryParse(val, out long l);

        public static bool IsBool(string val) => bool.TryParse(val, out bool b);

        public static bool IsString(string val)
        {
            if (val == null) return false;
            return val.StartsWith("\"") && val.EndsWith("\"");
        }

        public static bool IsNumber(string val)
        {
            return IsDouble(val) || IsFloat(val) ||IsInt(val) || IsLong(val);
        }


        public static Value_Type ValueType(string val)
        {
            if (IsDouble(val)) return Value_Type.Double;
            else if (IsFloat(val)) return Value_Type.Float;
            else if (IsInt(val)) return Value_Type.Int;
            else if (IsLong(val)) return Value_Type.Long;
            else if (IsBool(val)) return Value_Type.Bool;
            else if (IsString(val)) return Value_Type.String;
            else return Value_Type.Other; //Other (Array?)
        }

        public Value(dynamic value)
        {
            this.value = value;
        }

        public Value(dynamic value, Value_Type valueType)
        {
            this.value = value;
            type = valueType;
        }

    }

}
