using LinuxFind.Bases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinuxFind.Parsers.LogicHelpers
{
    public class LogicalAnd: FilterBase
    {
        private List<FilterBase> operands = new List<FilterBase>();
        public LogicalAnd(List<FilterBase> operands)
        {
            this.operands = operands;
        }
        public override bool evaluate(FileInfo file)
        {
            foreach (var operand in operands)
            {
                if (!operand.evaluate(file))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
