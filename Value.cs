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
}
