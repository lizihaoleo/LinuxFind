using LinuxFind.Bases;
using LinuxFind.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinuxFind.Parsers
{
    public class MaxDepthOptionParser : ParserBase
    {
        public override string getName()
        {
            return "maxdepth";
        }

        public override PlanNode parse(Stack<string> args)
        {
            return new MaxDepthOption(int.Parse(args.Pop()));
        }
    }
}
