using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class LogicHandler
    {
        ValueHandler valueHanlder;
        // (value == 34 || value == 15)
        //Operators:
        //  ! -> NOT
        // && -> AND
        // || -> OR

        bool GetBoolEquivalent(string token)
        {
            if(token == "1") return true;
            return false;
        }
        bool IntepreteBoolExpression(List<string> tokens)
        {
            string ope ="";
            for (int i = 1; i < tokens.Count - 1; i++)
            {

            }
            return false;
        }

    }
}
