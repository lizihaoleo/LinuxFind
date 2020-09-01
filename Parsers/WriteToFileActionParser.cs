using LinuxFind.Actions;
using LinuxFind.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinuxFind.Parsers
{
    public class WriteToFileActionParser : ParserBase
    {
        public override string getName()
        {
            return "writetofile";
        }

        public override PlanNode parse(Stack<string> args)
        {
            return new WriteToFileAction(args.Pop());
        }
    }
}
