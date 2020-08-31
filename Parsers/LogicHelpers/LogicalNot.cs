using LinuxFind.Bases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinuxFind.Parsers.LogicHelpers
{
    public class LogicalNot : FilterBase
    {
        private FilterBase operand;

        public LogicalNot(FilterBase operand)
        {
            this.operand = operand;
        }

        public override bool evaluate(FileInfo file)
        {
            return !operand.evaluate(file);
        }
    }
}
