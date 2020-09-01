using LinuxFind.Bases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LinuxFind
{
    public class Executor
    {
        private DirectoryInfo root;
        private List<FilterBase> filters;
        private List<OptionBase> options;
        private List<ActionBase> actions;
        public void Execute()
        {
            var context = new ExcutionContext();
            var files = new List<FileInfo>();
            
            //Options: exscute before find files
            foreach (var option in options)
            {
                option.setup(context);
            }
            files = getFiles(context);
            
            //Filter: addtional filters to files
            foreach (var filter in filters)
            {
                files = files.Where(f => filter.evaluate(f)).ToList();
            }
            // store filted files into context for further Actions
            context.files = files;

            //Action: execute Action at the end
            foreach (var action in actions)
            {
                action.invoke(context);
            }

            files.ForEach(f => Console.WriteLine($@"{f.FullName}\{f.Name}  size: {f.Length/(1024)} KB"));
        }
        // BFS from root to all sub directories
        private List<FileInfo> getFiles(ExcutionContext context)
        {
            var files = new List<FileInfo>();
            
            var curDepth = 0;
            var queue = new Queue<DirectoryInfo>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                if (context.getMaxDepth() == curDepth)
                {
                    return files;
                }
                var curDirectory = queue.Dequeue();
                foreach (var subDirectory in curDirectory.GetDirectories())
                {
                    queue.Enqueue(subDirectory);
                }
                foreach (var file in curDirectory.GetFiles())
                {
                    files.Add(file);  
                }
                curDepth++;
            }
            return files;
        }

        private bool isSymbolic(FileInfo file)
        {
            return file.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }

        public Executor(DirectoryInfo root, List<FilterBase> filters, List<OptionBase> options, List<ActionBase> actions)
        {
            this.filters = filters;
            this.options = options;
            this.actions = actions;
            this.root = root;
        }
    }
}