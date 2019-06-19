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

    public Method(string code)
    {
        lexer = new Lexer(code, false);

        while (!lexer.Eof())
        {
            Statement nextStatement = lexer.NextStatement();
            lot.Add(nextStatement);
        }
    }

}
}
