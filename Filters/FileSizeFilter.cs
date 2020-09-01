using com.sun.org.apache.bcel.@internal.util;
using LinuxFind.Bases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinuxFind.Filters
{
    public enum Comparator
    {
        eq,gt,lt
    }
    public class FileSizeFilter : FilterBase
    {
        private long size;
        private Comparator comparator;
        public FileSizeFilter(Comparator com, long size)
        {
            this.comparator = com;
            this.size = size;
        }
        public override bool evaluate(FileInfo file)
        {
            if (comparator.Equals(Comparator.eq))
            {
                return file.Length == size;
            }
            else if (comparator.Equals(Comparator.gt))
            {
                return file.Length >= size;
            }
            return file.Length <= size;

        }
    }
}
