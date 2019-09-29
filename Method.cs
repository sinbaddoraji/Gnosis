using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class Method
    {
        public List<Statement> lot = new List<Statement>();
        public VariableHandler Variables => lexer.Variables;

        private Lexer lexer;


        private void GetStatements()
        {
            while (!lexer.Eof)
            {
                Statement nextStatement = lexer.NextStatement();
                lot.Add(nextStatement);
            }
        }

        public Method(string code)
        {
            lexer = new Lexer(code, false);
            GetStatements();
        }

        public Method(List<string> statement)
        {
            lexer = new Lexer(statement);
            GetStatements();
        }

        public dynamic GetArray(string name, int index)
        {
            return Variables.GetArray(name, index);
        }

        public Variable GetVariable(string name)
        {
            return Variables.GetVariable(name);
        }
    }
}
