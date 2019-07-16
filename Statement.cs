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

        public Statement(List<string> statement) => tokens = statement;

        public void RunStatement() => internalMethodHandler.DoFunction(internalMethod);

    }
}
