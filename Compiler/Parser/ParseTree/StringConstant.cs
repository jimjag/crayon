﻿using System.Text;

namespace Parser.ParseTree
{
    public class StringConstant : Expression, IConstantValue
    {
        public override bool IsInlineCandidate { get { return true; } }

        public override bool CanAssignTo { get { return false; } }

        public string Value { get; private set; }
        public StringConstant(Token token, string value, Node owner)
            : base(token, owner)
        {
            this.Value = value;
        }

        public override bool IsLiteral { get { return true; } }

        public static string ParseOutRawValue(Token stringToken)
        {
            string rawValue = stringToken.Value;
            rawValue = rawValue.Substring(1, rawValue.Length - 2);
            StringBuilder sb = new StringBuilder();
            char c;
            for (int i = 0; i < rawValue.Length; ++i)
            {
                c = rawValue[i];
                if (c == '\\')
                {
                    c = rawValue[++i];
                    switch (c)
                    {
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case '0': sb.Append('\0'); break;
                        case 't': sb.Append('\t'); break;
                        case 'b': sb.Append('\b'); break;
                        case '\'': sb.Append("'"); break;
                        case '"': sb.Append("\""); break;
                        case '\\': sb.Append("\\"); break;
                        default: throw new ParserException(stringToken, "Invalid escape sequence: \\" + c);
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        internal override Expression Resolve(ParserContext parser)
        {
            return this;
        }

        internal override Expression ResolveEntityNames(ParserContext parser)
        {
            return this;
        }

        public Expression CloneValue(Token token, Node owner)
        {
            return new StringConstant(token, this.Value, owner);
        }

        internal override void PerformLocalIdAllocation(ParserContext parser, VariableScope varIds, VariableIdAllocPhase phase) { }
    }
}
