﻿using Common;
using Platform;
using System;
using System.Collections.Generic;

namespace LangPython
{
    public class PlatformImpl : AbstractPlatform
    {
        public override string Name { get { return "lang-python"; } }
        public override string InheritsFrom { get { return null; } }
        public override string NL { get { return "\n"; } }

        public PlatformImpl()
            : base("PYTHON")
        { }

        public override IDictionary<string, object> GetConstantFlags()
        {
            return new Dictionary<string, object>();
        }

        public override void ExportStandaloneVm(
            Dictionary<string, FileOutput> output,
            IList<LibraryForExport> everyLibrary)
        {
            throw new NotImplementedException();
        }

        public override void ExportProject(
            Dictionary<string, FileOutput> output,
            string byteCode,
            IList<LibraryForExport> libraries,
            Build.ResourceDatabase resourceDatabase,
            Options options)
        {
            throw new InvalidOperationException("This platform does not support direct export.");
        }

        public override Dictionary<string, string> GenerateReplacementDictionary(
            Options options,
            Build.ResourceDatabase resDb)
        {
            return AbstractPlatform.GenerateGeneralReplacementsDictionary(options);
        }
    }
}
