﻿using Localization;
using System;

namespace Parser.ParseTree
{
    public class ImportStatement : Node
    {
        public string ImportPath { get; set; }

        public ImportStatement(Token importToken, string path, FileScope fileScope)
            : base(importToken, null)
        {
            this.ImportPath = path;
            fileScope.Imports.Add(this);
        }
    }
}
