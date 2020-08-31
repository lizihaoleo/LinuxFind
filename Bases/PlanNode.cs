using System;
using System.Collections.Generic;
using System.Text;

namespace LinuxFind.Bases
{
    public enum PlanNodeKind
    {
        OPTION, FILTER, ACTION
    }

    // Option/Filter/Action的基类
    public abstract class PlanNode
    {
        public abstract PlanNodeKind getKind();
    }
}
