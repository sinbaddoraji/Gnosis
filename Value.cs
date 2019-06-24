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

        private readonly string[] vType = new[] { "double", "float", "int", "long", "bool", "string" };
        

        public static bool IsDouble(string val) => double.TryParse(val, out double d);

        public static bool IsFloat(string val) => float.TryParse(val, out float f);

        public static bool IsInt(string val) => int.TryParse(val, out int i);

        public static bool IsLong(string val) => long.TryParse(val, out long l);

        public static bool IsBool(string val) => bool.TryParse(val, out bool b);

        public static bool IsString(string val) => val.StartsWith("\"") && val.EndsWith("\"");

        public static bool IsNumber(string val)
        {
            return IsDouble(val) || IsFloat(val) ||IsInt(val) || IsLong(val);
        }


        public static int ValueType(string val)
        {
            if (IsDouble(val)) return 0;
            else if (IsFloat(val)) return 1;
            else if (IsInt(val)) return 2;
            else if (IsLong(val)) return 3;
            else if (IsBool(val)) return 4;
            else if (IsString(val)) return 5;
            else return 6; //Other (Array?)
        }


        // value types : double, float, int, long, bool, string
        // double -> 0
        //  float -> 1
        //    int -> 2
        //   long -> 3
        //   bool -> 4
        // string -> 5
        //  other -> 6;

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

}
