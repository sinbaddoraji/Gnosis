using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class Statement
    {
        public List<string> tokens = new List<string>();

        bool isConditional = false; // Is a statement with {}

        public Statement(List<string> statement)
        {
            tokens = statement;
        }
    }
}
