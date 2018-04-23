﻿using Common;
using Pastel.Transpilers;
using Platform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PythonApp
{
    public class PlatformImpl : AbstractPlatform
    {
        public override string Name { get { return "python-app"; } }
        public override string InheritsFrom { get { return "lang-python"; } }
        public override string NL { get { return "\n"; } }

        public PlatformImpl()
        {
            this.Translator = new PythonTranslator();
        }

        public override IDictionary<string, object> GetConstantFlags()
        {
            return new Dictionary<string, object>();
        }

        public override void ExportStandaloneVm(
            Dictionary<string, FileOutput> output,
            Pastel.PastelCompiler compiler,
            Pastel.PastelContext pastelContext,
            IList<LibraryForExport> everyLibrary,
            ILibraryNativeInvocationTranslatorProvider libraryNativeInvocationTranslatorProviderForPlatform)
        {
            throw new NotImplementedException();
        }

        public override void ExportProject(
            Dictionary<string, FileOutput> output,
            Pastel.PastelCompiler compiler,
            Pastel.PastelContext pastelContext,
            IList<LibraryForExport> libraries,
            ResourceDatabase resourceDatabase,
            Options options,
            ILibraryNativeInvocationTranslatorProvider libraryNativeInvocationTranslatorProviderForPlatform)
        {
            Dictionary<string, string> replacements = this.GenerateReplacementDictionary(options, resourceDatabase);

            TranspilerContext ctx = new TranspilerContext();

            output["code/vm.py"] = new FileOutput()
            {
                Type = FileOutputType.Text,
                TextContent = string.Join(this.Translator.NewLine, new string[] {
                    this.LoadTextResource("Resources/header.txt" , replacements),
                    this.LoadTextResource("Resources/TranslationHelper.txt", replacements),
                    compiler.GetGlobalsCodeTEMP(this.Translator, ctx, ""),
                    this.LoadTextResource("Resources/LibraryRegistry.txt", replacements),
                    this.LoadTextResource("Resources/ResourceReader.txt", replacements),
                    compiler.GetFunctionCodeTEMP(this.Translator, ctx, ""),
                }),
            };

            foreach (LibraryForExport library in libraries)
            {
                ctx.CurrentLibraryFunctionTranslator = libraryNativeInvocationTranslatorProviderForPlatform.GetTranslator(library.Name);
                string libraryName = library.Name;
                List<string> libraryLines = new List<string>();
                if (library.HasPastelCode)
                {
                    libraryLines.Add("import math");
                    libraryLines.Add("import os");
                    libraryLines.Add("import random");
                    libraryLines.Add("import sys");
                    libraryLines.Add("import time");
                    libraryLines.Add("import inspect");
                    libraryLines.Add("from code.vm import *");
                    libraryLines.Add("");
                    libraryLines.Add(library.PastelContext.CompilerDEPRECATED.GetFunctionCodeForSpecificFunctionAndPopItFromFutureSerializationTEMP(
                        "lib_manifest_RegisterFunctions",
                        null,
                        this.Translator,
                        ctx,
                        ""));
                    libraryLines.Add(library.PastelContext.CompilerDEPRECATED.GetFunctionCodeTEMP(this.Translator, ctx, ""));
                    libraryLines.Add("");
                    libraryLines.Add("_moduleInfo = ('" + libraryName + "', dict(inspect.getmembers(sys.modules[__name__])))");
                    libraryLines.Add("");
                    libraryLines.AddRange(library.ExportEntities["EMBED_CODE"].Select(entity => entity.StringValue));

                    output["code/lib_" + libraryName.ToLower() + ".py"] = new FileOutput()
                    {
                        Type = FileOutputType.Text,
                        TextContent = string.Join(this.NL, libraryLines),
                    };
                }
            }

            output["main.py"] = new FileOutput()
            {
                Type = FileOutputType.Text,
                TextContent = this.LoadTextResource("Resources/main.txt", replacements),
            };

            output["code/__init__.py"] = new FileOutput()
            {
                Type = FileOutputType.Text,
                TextContent = "",
            };

            output["res/bytecode.txt"] = resourceDatabase.ByteCodeFile;
            output["res/resource_manifest.txt"] = resourceDatabase.ResourceManifestFile;
            if (resourceDatabase.ImageSheetManifestFile != null)
            {
                output["res/image_sheet_manifest.txt"] = resourceDatabase.ImageSheetManifestFile;
            }

            foreach (FileOutput image in resourceDatabase.ImageResources)
            {
                output["res/images/" + image.CanonicalFileName] = image;
            }

            foreach (string imageSheetFile in resourceDatabase.ImageSheetFiles.Keys)
            {
                output["res/images/" + imageSheetFile] = resourceDatabase.ImageSheetFiles[imageSheetFile];
            }

            foreach (FileOutput sound in resourceDatabase.AudioResources)
            {
                output["res/audio/" + sound.CanonicalFileName] = sound;
            }

            foreach (FileOutput textResource in resourceDatabase.TextResources)
            {
                output["res/text/" + textResource.CanonicalFileName] = textResource;
            }

            foreach (FileOutput fontResource in resourceDatabase.FontResources)
            {
                output["res/ttf/" + fontResource.CanonicalFileName] = fontResource;
            }
        }

        public override Dictionary<string, string> GenerateReplacementDictionary(Options options, ResourceDatabase resDb)
        {
            return this.ParentPlatform.GenerateReplacementDictionary(options, resDb);
        }
    }
}