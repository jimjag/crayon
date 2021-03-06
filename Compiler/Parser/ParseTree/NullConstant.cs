﻿using Parser.Resolver;
using System.Collections.Generic;

namespace Parser.ParseTree
{
    internal class NullConstant : Expression, IConstantValue
    {
        public override bool IsInlineCandidate { get { return true; } }

        internal override IEnumerable<Expression> Descendants { get { return Expression.NO_DESCENDANTS; } }

        public NullConstant(Token token, Node owner)
            : base(token, owner)
        {
            this.ResolvedType = ResolvedType.NULL;
        }

        public override bool IsLiteral { get { return true; } }

        internal override Expression Resolve(ParserContext parser)
        {
            return this;
        }

        internal override Expression ResolveEntityNames(ParserContext parser)
        {
            return this;
        }

        internal override Expression ResolveTypes(ParserContext parser, TypeResolver typeResolver)
        {
            return this;
        }

        public Expression CloneValue(Token token, Node owner)
        {
            return new NullConstant(token, owner);
        }

        internal override void ResolveVariableOrigins(ParserContext parser, VariableScope varIds, VariableIdAllocPhase phase) { }
    }
}
