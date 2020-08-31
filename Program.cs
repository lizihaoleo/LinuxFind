using System;

namespace LinuxFind
{
    class Program
    {
        static void Main(string[] args)
        {
            Executor exec = new ExecutionGenerator().generateExecutor(args);
            exec.Execute();
            Console.ReadKey();
        }
    }
}
