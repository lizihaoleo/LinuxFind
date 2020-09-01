using System;
using System.Collections.Generic;
using System.Text;

namespace LinuxFind.Bases
{
    public abstract class ActionBase : PlanNode
    {
        public abstract void invoke(ExcutionContext context);

        public abstract void initialize();
        public abstract void finalize();
        public override PlanNodeKind getKind()
        {
            return PlanNodeKind.ACTION;
        }
    }
}
