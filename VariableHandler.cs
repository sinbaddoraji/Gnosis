﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class VariableHandler
    {
        public VariableHandler OuterVariables;

        private readonly Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        public bool IsLocalVariable(string variable)
        {
            return variables.ContainsKey(variable);
        }

        public bool IsVariable(string variable)
        {
            VariableHandler cur = this;
            if(variables.ContainsKey(variable)) return true;

            while (true && cur.OuterVariables != null)
            {
                if (cur.OuterVariables.IsLocalVariable(variable)) return true;
                cur = cur.OuterVariables;
            }

           return false;
        }

        public VariableHandler GetVariableHandler(string variable)
        {
            VariableHandler cur = this;
            if (variables.ContainsKey(variable)) return cur;

            while (true && cur.OuterVariables != null)
            {
                if (cur.OuterVariables.IsLocalVariable(variable)) return cur;
                cur = cur.OuterVariables;
            }

            return null;
        }

        public bool IsArray(string variable)
        {
            try
            {
                return variables[variable].IsArray;
            }
            catch
            {
                return false;
            }
        }

        public static Value ProcessedValue(string value)
        {
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

        public void RawAddVariable(string name, Variable v)
        {
            if (variables.ContainsKey(name))
                variables[name] = v;
            else
                variables.Add(name, v);
        }

        public void AddVariable(string name, Variable v)
        {
            VariableHandler vh = GetVariableHandler(name);
            if(vh != null) vh.RawAddVariable(name, v);
            else RawAddVariable(name, v);
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

        public Variable GetLocalVariable(string name)
        {
            return variables[name];
        }

        public Variable GetVariable(string name)
        {
            VariableHandler cur = this;

            while (true && cur.OuterVariables != null)
            {
                if (cur.OuterVariables.IsLocalVariable(name)) 
                    return cur.OuterVariables.GetLocalVariable(name);
                cur = cur.OuterVariables;
            }
            return variables[name];
        }

        public dynamic GetArray(string name, int index)
        {
            Array array = (Array)GetVariable(name);

            return array.GetValue(index);
        }

        public VariableHandler(bool isGlobal = false)
        {
            if(isGlobal == false) return;
            variables.Add("endl", new Variable("\n", true));
            variables.Add("true", new Variable(new Value(true)));
            variables.Add("false", new Variable(new Value(false)));
            variables.Add("args", new Variable(new Value(Environment.GetCommandLineArgs())));
        }
    }
}
