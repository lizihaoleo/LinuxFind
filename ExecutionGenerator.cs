using java.nio.file;
using javax.imageio.spi;
using LinuxFind.Bases;
using LinuxFind.Parsers;
using LinuxFind.Parsers.LogicHelpers;
using sun.tools.tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Path = java.nio.file.Path;

namespace LinuxFind
{
    public class ExecutionGenerator
    {
        private static Dictionary<string, OptionParser> optionParserRegistry = new Dictionary<string, OptionParser>();
        private Stack<String> tokens = new Stack<string>();
        public Executor generateExecutor(string[] args)
        {
            for (int i = args.Count() - 1; i >= 0; --i)
            {
                this.tokens.Push(args[i]);
            }

            if (tokens.Count == 0)
            {
                throw new Exception("Requires at least one path argument");
            }
            var filePath = new DirectoryInfo(tokens.Pop());

            List<FilterBase> logicalFilters = new List<FilterBase>();


            while (tokens.Count > 0)
            {
                PlanNode logicalNode = parseOr();
                switch (logicalNode.getKind())
                {
                    case PlanNodeKind.FILTER:
                        logicalFilters.Add((FilterBase)logicalNode);
                        break;
                    default:
                        throw new Exception("Unsupport enum value " + logicalNode.getKind().ToString());
                }
            }

            tokens = null;
            return new Executor(filePath, logicalFilters);
        }

        // parse "... -or ..." input
        private PlanNode parseOr()
        {
            List<PlanNode> operands = new List<PlanNode> ();
            operands.Add(parseAnd());
            while (nextTokenIs("-or") || nextTokenIs("-o"))
            {
                tokens.Pop();
                operands.Add(parseAnd());
            }
            if (operands.Count() == 1)
            {
                return operands.ElementAt(0);
            }
            List<FilterBase> predicates = new List<FilterBase>();
            foreach (PlanNode node in operands)
            {
                if (node.getKind() != PlanNodeKind.FILTER)
                {
                    throw new Exception("Logical OR can only be applied to predicates");
                }
                predicates.Add((FilterBase)node);
            }
            return new LogicalOr(predicates);
        }

        // parse "... -and ..." input
        private PlanNode parseAnd()
        {
            List<PlanNode> operands = new List<PlanNode>();
            operands.Add(parseNot());
            while (nextTokenIs("-and") || nextTokenIs("-a"))
            {
                tokens.Pop();
                operands.Add(parseNot());
            }
            if (operands.Count() == 1)
            {
                return operands.ElementAt(0);
            }
            List<FilterBase> predicates = new List<FilterBase>();
            foreach (PlanNode node in operands)
            {
                if (node.getKind() != PlanNodeKind.FILTER)
                {
                    throw new Exception("Logical AND can only be applied to predicates");
                }
                predicates.Add((FilterBase)node);
            }
            return new LogicalAnd(predicates);
        }

        // parse "-not ..." input
        private PlanNode parseNot()
        {
            bool negate = false;
            if (nextTokenIs("-not") || nextTokenIs("-n"))
            {
                tokens.Pop();
                negate = true;
            }
            PlanNode operand = parseAtom();
            if (!negate)
            {
                return operand;
            }
            if (operand.getKind() != PlanNodeKind.FILTER)
            {
                throw new Exception("Logical NOT can only be applied to a predicate");
            }
            return new LogicalNot((FilterBase)operand);
        }

        // parse expression inside "(...)" or base class for Option / Filter / Action
        private PlanNode parseAtom()
        {
            if (nextTokenIs("("))
            {
                tokens.Pop();
                PlanNode node = parseOr();
                if (!nextTokenIs(")"))
                {
                    throw new Exception("Unmatched parenthesis");
                }
                tokens.Pop();
                return node;
            }
            if (tokens.Count == 0)
            {
                throw new Exception("Unexpected end of input stream");
            }
            if (!tokens.Peek().StartsWith("-"))
            {
                throw new Exception("Unexpected token " + tokens.Peek());
            }
            // 这个name就是参数名，例如-type的"type"，-size的"size"
            String name = tokens.Pop().Substring(1);
            // 在registry中找到与参数名对应的parser
            OptionParser parser = optionParserRegistry.GetValueOrDefault(name);
            if (parser == null)
            {
                throw new Exception("Unrecognized option " + name);
            }
            // Parser各自的parse()方法用于parse参数的arguments
            // 例如 -size +1M，那么"size"所对应的parser应当知道如何parse"+1M"
            return parser.parse(tokens);
        }

        private bool nextTokenIs(string value)
        {
            return tokens.Count > 0 && value.Equals(tokens.Peek());
        }

        public ExecutionGenerator()
        {
            Register(new FileNameFilterParser());
        }

        private static void Register(OptionParser parser)
        {
            optionParserRegistry.Add(parser.getName(), parser);
        }
    }
}