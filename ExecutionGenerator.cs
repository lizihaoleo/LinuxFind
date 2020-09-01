using LinuxFind.Bases;
using LinuxFind.Parsers;
using LinuxFind.Parsers.LogicHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LinuxFind
{
    public class ExecutionGenerator
    {
        private static Dictionary<string, ParserBase> optionParserRegistry = new Dictionary<string, ParserBase>();
        private Stack<String> tokens = new Stack<string>();

        public ExecutionGenerator()
        {
            register(new FileNameFilterParser());
            register(new FileSizeFilterParser());
            register(new MaxDepthOptionParser());
            register(new WriteToFileActionParser());
        }

        private static void register(ParserBase parser)
        {
            optionParserRegistry.Add(parser.getName(), parser);
        }

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
            
            var root = new DirectoryInfo(tokens.Pop());

            List<FilterBase> filters = new List<FilterBase>(); // list of logfical helper
            List<OptionBase> options = new List<OptionBase>();
            List<ActionBase> actions = new List<ActionBase>();

            while (tokens.Count > 0)
            {
                // analysize next keyword (name, size, maxdepth) and parameter (*.txt, -1mb, 10)
                // looking for instance of respective node from static dictionary in the class
                PlanNode node = parseOr();
                switch (node.getKind())
                {
                    case PlanNodeKind.OPTION:
                        options.Add((OptionBase)node);
                        break;
                    case PlanNodeKind.FILTER:
                        filters.Add((FilterBase)node);
                        break;
                    case PlanNodeKind.ACTION:
                        actions.Add((ActionBase)node);
                        break;
                    default:
                        throw new Exception("Unsupport enum value " + node.getKind().ToString());
                }
            }

            tokens = null;
            return new Executor(root,filters, options, actions);
        }


        #region LL Parser
        // LL Parse (递归下降解析器) https://zhuanlan.zhihu.com/p/31271879

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

        // parse expression inside "(...)" or base class for Filter
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
            ParserBase parser = optionParserRegistry.GetValueOrDefault(name);
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

        #endregion

    }
}