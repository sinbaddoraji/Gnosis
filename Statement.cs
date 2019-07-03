using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class Statement
    {
        public Method internalMethod;
        public MethodHandler internalMethodHandler;
        public VariableHandler internalVariableHandler;

        public List<string> tokens = new List<string>();

        bool isConditional = false; // Is a statement with {}

        public Statement(List<string> statement, bool isConditional = false)
        {
            tokens = statement;
            this.isConditional = isConditional;
        }

        public void RunStatement()
        {
            internalMethodHandler.DoFunction(internalMethod);
        }

    }
}
