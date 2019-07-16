using System.Collections.Generic;
using System.Linq;

namespace Gnosis
{
    internal class Lexer
    {
        /*
         *   in depth tokenization will be done individually
         */
        private readonly string code;
        private int codeIndex = 0;

        //private readonly int line = 0;
        //private readonly int main = 0;

        private readonly bool initialRun = true;
        private readonly List<string> rawTokens;

        public List<string> currentTree = new List<string>();
        public List<List<string>> Trees = new List<List<string>>();

        public Dictionary<string, Method> Methods = new Dictionary<string, Method>();
        public VariableHandler Variables = new VariableHandler();

        //Add concept of global variables and handle them

        private readonly string[] keywords = new string[]
        {
            "print", "input", "var","pause", "let"
        };

        private readonly string[] conditionalKeywords = new string[]
        {
            "while", "switch", "for", "foreach", "if", "case"
        };
        private readonly string[] operators = new string[]
        {
            "+", "-","*", "/", "=", ">", "<", "!", "%", "[", "]", "{", "}","(",")",",",";", "|"
        };
        private readonly string[] doubleOperators = new string[]
        {
            "++","--","+=","-=","*=","/=","!=","==", "<<",">>","||","&&"
        };

        public Method MainMethod() => Methods["main"];

        public bool IsVariable(string val) => Variables.IsVariable(val);

        public bool IsArray(string val) => Variables.IsArray(val);

        public static bool IsString(string str)
        {
            if (str == null || !str.StartsWith("\"") || !str.EndsWith("\"")) return false;
            else return true;
        }

        private bool IsSymbol(string str) => operators.Contains(str) || doubleOperators.Contains(str);

        private bool IsKeyword(string str) => keywords.Contains(str) || conditionalKeywords.Contains(str);

        public bool Eof
        {
            //End of file
            get
            {
                if (initialRun == false && rawTokens != null) return codeIndex >= rawTokens.Count;
                else return codeIndex >= code.Length;
            }
        }

        private string NextBit()
        {
            string output;

            if(Eof) return "";

            if(initialRun == false && rawTokens != null)
            {
                output = rawTokens[codeIndex];
                codeIndex++;

                return output;
            }

            output = code[codeIndex].ToString();
            codeIndex++;

            if (codeIndex == code.Length) return null;

            //If string
            if (output == "\"")
            {
                while (true)
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
                if (doubleOperators.Contains(output2))
                {
                    codeIndex++;
                    return output2;
                }
            }

            return output.Trim();
        }

        private string NextKeyword()
        {
            string cur = "";

            while (true)
            {
                if(Eof) return null;

                string next = NextBit();

                if (IsSymbol(next))
                {
                    statementOutput.Add(cur);
                    cur = "";
                }

                if (next == null) return null;
                else cur += next;

                if (IsVariable(cur) || IsString(cur) || IsSymbol(cur) || IsKeyword(cur)) return cur;
            }
        }

        private List<string> statementOutput;
        public Statement NextStatement()
        {
            //Do not add ; after } in token list
            // }; is used in code to make lexer find it as a statement (regular design)
            statementOutput = new List<string>();

            bool bracket = false;
            int bracketsOpened = 0;

            string cur = NextKeyword();

            while (true)
            {
                if (Eof) break;

                if (cur != "")
                {
                    statementOutput.Add(cur);
                }

                if(cur == "{")
                {
                    bracket = true;
                    bracketsOpened++;
                }
                else if (cur == "}")
                {
                    bracketsOpened--;
                }

                cur = NextKeyword();

                if (bracket)
                {
                    if (bracketsOpened == 0) break;
                }
                else if (cur ==  ";" || cur == null)
                {
                    if (cur == ";")
                        statementOutput.Add(";");

                    break;
                }
            }

            return new Statement(statementOutput.Where(x => x != "").ToList());
        }

        private Dictionary<string, Method> ExtractMethods()
        {
            int internalIndex = code.IndexOf('<'); //Use (for method extraction) instead of codeIndex(For in depth lexing)

            Dictionary<string, Method> methods = new Dictionary<string, Method>();

            while (internalIndex < code.Length && internalIndex != -1)
            {
                //Get raw string (untokinized code of method)
                //Locate the start and end
                //Copy from start to end
                if (internalIndex == code.Length - 1)
                {
                    break;
                }

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

        public Lexer(string codeText, bool _ = true)
        {
            code = codeText;

            if (_)
            {
                Methods = ExtractMethods(); // Extract methods
            }
            else _ = false;
        }

        public Lexer(List<string> statement)
        {
            rawTokens = statement;
            initialRun = false;
        }

    }
}
