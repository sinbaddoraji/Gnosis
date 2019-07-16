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
        public Lexer lexer;
        //private method variables -> lexer.variables


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
    }
}
