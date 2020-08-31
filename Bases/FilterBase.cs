using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinuxFind.Bases
{
    public abstract class FilterBase : PlanNode
    {
        public abstract bool evaluate(FileInfo file);
        public override PlanNodeKind getKind()
        {
            return PlanNodeKind.FILTER;
        }
    }
}
