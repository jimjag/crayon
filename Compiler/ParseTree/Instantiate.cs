﻿using System.Collections.Generic;
using System.Linq;

namespace Crayon.ParseTree
{
	internal class Instantiate : Expression
	{
		public override bool CanAssignTo { get { return false; } }

		public Token NameToken { get; private set; }
		public string Name { get; private set; }
		public Expression[] Args { get; private set; }
		public ClassDefinition Class { get; set; }

		public Instantiate(Token firstToken, Token firstClassNameToken, string name, IList<Expression> args, Executable owner)
			: base(firstToken, owner)
		{
			this.NameToken = firstClassNameToken;
			this.Name = name;
			this.Args = args.ToArray();
		}

		internal override Expression Resolve(Parser parser)
		{
			string className = this.NameToken.Value;

			if (parser.IsTranslateMode)
			{
				StructDefinition structDefinition = parser.GetStructDefinition(className);

				if (structDefinition != null)
				{
					if (this.Args.Length != structDefinition.Fields.Length)
					{
						throw new ParserException(this.FirstToken, "Args length did not match struct field count for '" + structDefinition.Name.Value + "'.");
					}

					StructInstance si = new StructInstance(this.FirstToken, this.NameToken, this.Args, this.FunctionOrClassOwner);
					si = (StructInstance)si.Resolve(parser);
					return si;
				}
			}

			for (int i = 0; i < this.Args.Length; ++i)
			{
				this.Args[i] = this.Args[i].Resolve(parser);
			}

			ConstructorDefinition cons = this.Class.Constructor;
			if (this.Args.Length < cons.MinArgCount || this.Args.Length > cons.MaxArgCount)
			{
				// TODO: show the correct arg count.
				throw new ParserException(this.FirstToken, "This constructor has the wrong number of arguments.");
			}

			return this;
		}

		internal override void SetLocalIdPass(VariableIdAllocator varIds)
		{
			for (int i = 0; i < this.Args.Length; ++i)
			{
				this.Args[i].SetLocalIdPass(varIds);
			}
		}

		internal override Expression ResolveNames(Parser parser, Dictionary<string, Executable> lookup, string[] imports)
		{
			this.BatchExpressionNameResolver(parser, lookup, imports, this.Args);

			Executable ex = Expression.DoNameLookup(lookup, imports, this.Name);
			if (ex == null)
			{
				throw new ParserException(this.NameToken, "No class found called '" + this.Name + "'");
			}

			if (ex is ClassDefinition)
			{
				this.Class = (ClassDefinition)ex;
			}
			else
			{
				throw new ParserException(this.NameToken, "This is not a class.");
			}

			return this;
		}
	}
}
