using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    class Lexer
    {
        /*
         *   in depth tokenization will be done individually
         */
        string code;

        int codeIndex = 0;
        int line = 0;

        int main = 0;

        public List<string> currentTree = new List<string>();
        public List<List<string>> Trees = new List<List<string>>();

        public Dictionary<string,Method> Methods = new Dictionary<string, Method>();
        public VariableHandler Variables = new VariableHandler();
        //Add concept of global variables and handle them

        string[] keywords = new string[]
        {
            "print", "input", "string", "char", "int", "double", "long", "float", "bool","pause"
        };

        string[] conditionalKeywords = new string[]
        {
            "while", "switch", "for", "foreach", "if", "case"
        };

        string[] operators = new string[]
        {
            "+", "-","*", "/", "=", ">", "<", "!", "%", "[", "]", "{", "}","(",")",",",";"
        };

        string[] doubleOperators = new string[]
        {
            "++","--","+=","-=","*=","/=","!=","==", "<<",">>","||","&&"
        };

        public Method MainMethod()
        {
            return Methods["main"];
        }

        public bool isVariable(string val)
        {
            return Variables.IsVariable(val);
        }

        public static bool IsString(string str)
        {
            if(!str.StartsWith("\"") || !str.EndsWith("\"")) return false;
            else return true;
        }

        bool IsSymbol(string str)
        {
            return operators.Contains(str) || doubleOperators.Contains(str);
        }

        bool IsKeyword(string str)
        {
            return keywords.Contains(str) || conditionalKeywords.Contains(str);
        }

        public bool Eof()
        {
            return codeIndex >= code.Length;
        }

        string NextBit()
        {
            string output = code[codeIndex].ToString();
            codeIndex++;

            if(codeIndex == code.Length) return null;

            //If string
            if (output == "\"")
            {
                while(true)
                {
                    output += code[codeIndex];
                    if (code[codeIndex] == '"' && code[codeIndex - 1] != '\\')
                    {
                        codeIndex++;
                        return output;
                    }
                    codeIndex++;
                }
            }
            else 
            {
                string output2 = output + code[codeIndex].ToString();
                if(doubleOperators.Contains(output2))
                {
                    codeIndex++;
                    return output2;
                }
            }

            return output.Trim();
        }

        string NextKeyword()
        {
            string cur = "";

            while(true)
            {
                string next = NextBit();

                if (IsSymbol(next))
                {
                    statementOutput.Add(cur);
                    cur = "";
                }

                if (next == null) return null;
                else cur += next;
                
                
                 if(isVariable(cur) || IsString(cur) || IsSymbol(cur) || IsKeyword(cur)) 
                    return cur;
               
            }
        }

        List<string> statementOutput;
        public Statement NextStatement()
        {
            statementOutput = new List<string>();

            string cur = NextKeyword();

            while(cur != ";" && cur != null)
            {
                if(cur.Trim() != "")
                     statementOutput.Add(cur);
                cur = NextKeyword();

                if(cur == ";") statementOutput.Add(";");
            }

            return new Statement(statementOutput.Where(x => x != "").ToList());
        }

        Dictionary<string,Method> ExtractMethods()
        {
            int internalIndex = code.IndexOf('<'); //Use (for method extraction) instead of codeIndex(For in depth lexing)

            Dictionary<string, Method> methods = new Dictionary<string, Method>();

            while(internalIndex < code.Length && internalIndex != -1)
            {
                    //Get raw string (untokinized code of method)
                    //Locate the start and end
                    //Copy from start to end
                    if (internalIndex == code.Length - 1) break;

                    string methodName = code.Substring(internalIndex, code.IndexOf('>', internalIndex));
                    internalIndex = code.IndexOf(methodName.Insert(1, "/"));
                    string methodEnding = code.Substring(internalIndex, methodName.Length + 1);

                    //Skip method end signifier 
                    internalIndex += methodEnding.Length + 1;

                    int start = code.IndexOf(methodName) + methodName.Length + 2;//Exclude <method name>
                    int end = code.IndexOf(methodEnding);

                    string methodCode = code.Substring(start, end);
                    methods.Add(methodName.Trim('<', '>'), new Method(methodCode));

                    internalIndex = code.IndexOf('<', internalIndex);
            }
            return methods;
        }

        public Lexer (string codeText, bool initialRun = true)
        {
            code = codeText;

            if(initialRun)
            {
                 Methods = ExtractMethods(); // Extract methods
            }
        }

        
    }
}
