using LinuxFind.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinuxFind.Options
{
    public class MaxDepthOption : OptionBase
    {
        private int maxDepth;
        public MaxDepthOption(int maxDepth)
        {
            this.maxDepth = maxDepth;
        }
        public override void setup(ExcutionContext context)
        {
            context.setMaxDepth(this.maxDepth);
        }
    }
}
