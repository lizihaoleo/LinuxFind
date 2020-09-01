using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinuxFind
{
    public class ExcutionContext
    {
        private int maxDepth = int.MaxValue;
        public List<FileInfo> files { get; set; } = new List<FileInfo>();
        // Getters and setters
        public void setMaxDepth(int maxDepth)
        {
            this.maxDepth = maxDepth;
        }

        public int getMaxDepth()
        {
            return maxDepth;
        }

        

    }
}
