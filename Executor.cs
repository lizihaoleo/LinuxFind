using java.nio.file;
using LinuxFind.Bases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LinuxFind
{
    public class Executor
    {
        private DirectoryInfo filePath;
        private List<FilterBase> predicates;

        public void Execute()
        {
            var files = filePath.GetFiles("*", SearchOption.AllDirectories).ToList();
            foreach (var predicate in predicates)
            {
                files = files.Where(f => predicate.evaluate(f)).ToList();
            }
            files.ForEach(f => Console.WriteLine($@"{f.FullName}\{f.Name}"));
        }

        public Executor(DirectoryInfo filePath, List<FilterBase> predicates)
        {
            this.filePath = filePath;
            this.predicates = predicates;
        }
    }
}