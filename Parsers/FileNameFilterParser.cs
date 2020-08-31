using LinuxFind.Bases;
using LinuxFind.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinuxFind.Parsers
{
    public class FileNameFilterParser : OptionParser
    {
        public override string getName()
        {
            return "name";
        }

        public override PlanNode parse(Stack<string> args)
        {
            return new FileNameFilter(args.Pop());
        }
    }
}
