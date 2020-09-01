using System;
using System.Collections.Generic;
using System.Text;

namespace LinuxFind.Bases
{
    public abstract class OptionBase : PlanNode
    {
        public abstract void setup(ExcutionContext context);
        public override PlanNodeKind getKind()
        {
            return PlanNodeKind.OPTION;
        }
    }
}
