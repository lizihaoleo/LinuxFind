using LinuxFind.Bases;
using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinuxFind.Filters
{
    public class FileNameFilter : FilterBase
    {
        private Matcher matcher = new Matcher();
        public FileNameFilter(string pattern)
        {
            matcher.AddInclude(pattern);
        }
        public override bool evaluate(FileInfo file)
        {
            return matcher.Match(file.Name).HasMatches;
        }
    }
}
