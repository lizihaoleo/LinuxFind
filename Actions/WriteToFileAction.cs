using LinuxFind.Bases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinuxFind.Actions
{
    public class WriteToFileAction : ActionBase
    {
        private string fileName;
        private StreamWriter writer;

        public WriteToFileAction(string fileName)
        {
            this.fileName = fileName;
        }

        public override void initialize()
        {
            try
            {
                writer = new StreamWriter(fileName);
            }
            catch (FileNotFoundException e)
            {
                throw new Exception(e.Message);
            }
        }


        public override void finalize()
        {
            writer.Close();
        }
        public override void invoke(ExcutionContext context)
        {
            initialize();
            context.files.ForEach(f => writer.WriteLine($@"{f.FullName}\{f.Name}  size: {f.Length / (1024)} KB"));
            finalize();
        }
    }
}
