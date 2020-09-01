using jdk.nashorn.@internal.ir;
using LinuxFind.Bases;
using LinuxFind.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinuxFind.Parsers
{
    public class FileSizeFilterParser : ParserBase
    {
        public override string getName()
        {
            return "size";
        }

        // +10M, 20kb
        public override PlanNode parse(Stack<string> args)
        {
            var param = args.Pop();
            var op = Comparator.eq;
            int startPos = 0;
            defineComparator(param, ref op, ref startPos);
            int endPos = startPos;
            while (endPos < param.Length && char.IsDigit(param[endPos]))
            {
                endPos++;
            }
            if (endPos == startPos) throw new Exception("Invalid file size specification: " + param);

            var fileSize = long.Parse(param.Substring(startPos, endPos - startPos));
            string sizeFormat = param.Substring(endPos).ToLower();
            fileSize = calculateSize(fileSize, sizeFormat);
            return new FileSizeFilter(op, fileSize);

        }

        private static void defineComparator(string param, ref Comparator op, ref int startPos)
        {
            if (param.StartsWith("+"))
            {
                op = Comparator.gt;
                startPos++;
            }
            else if (param.StartsWith("-"))
            {
                op = Comparator.lt;
                startPos++;
            }
        }

        private static long calculateSize(long fileSize, string sizeFormat)
        {
            if (sizeFormat.Equals("kb") || sizeFormat.Equals("k"))
            {
                fileSize *= 1024;
            }
            else if (sizeFormat.Equals("mb") || sizeFormat.Equals("m"))
            {
                fileSize *= 1024 * 1024;
            }
            else if (sizeFormat.Equals("gb") || sizeFormat.Equals("g"))
            {
                fileSize *= 1024 * 1024 * 1024;
            }
            else
            {
                throw new Exception($"Unsupport size format {sizeFormat}");
            }

            return fileSize;
        }
    }
}
