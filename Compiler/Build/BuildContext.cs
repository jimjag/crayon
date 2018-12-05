﻿using Build.BuildParseNodes;
using Common;
using Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Build
{
    public class BuildContext
    {
        public BuildContext()
        {
            this.TopLevelAssembly = new AssemblyContext(this);
        }

        public AssemblyContext TopLevelAssembly { get; set; }
        public string ProjectID { get; set; }
        public string ProjectDirectory { get; set; }
        public string OutputFolder { get; set; }
        public string JsFilePrefix { get; set; }
        public string Platform { get; set; }
        public bool Minified { get; set; }
        public bool ReadableByteCode { get; set; }
        public string GuidSeed { get; set; }
        public string LaunchScreenPath { get; set; }
        public string DefaultTitle { get; set; }
        public string Orientation { get; set; }
        public string CrayonPath { get; set; }
        public string IosBundlePrefix { get; set; }
        public string JavaPackage { get; private set; }
        public bool JsFullPage { get; set; }
        public int? WindowWidth { get; set; }
        public int? WindowHeight { get; set; }
        public Locale CompilerLocale { get; set; }
        public string IconFilePath { get; set; }

        private static Target FindTarget(string targetName, IList<Target> targets)
        {
            foreach (Target target in targets)
            {
                if (target.Name == null) throw new InvalidOperationException("A target in the build file is missing a name.");

                // CBX targets don't have a platform specified.
                if (target.Name != "cbx" && target.Platform == null) throw new InvalidOperationException("A target in the build file is missing a platform.");

                if (target.Name == targetName)
                {
                    return target;
                }
            }
            return null;
        }

        private static BuildRoot GetBuildRoot(string buildFile)
        {
            try
            {
                return JsonParserForBuild.Parse(buildFile);
            }
            catch (JsonParser.JsonParserException jpe)
            {
                throw new InvalidOperationException("Build file JSON syntax error: " + jpe.Message);
            }
        }

        public static BuildContext Parse(string projectDir, string buildFile, string nullableTargetName)
        {
            BuildRoot buildInput = GetBuildRoot(buildFile);
            BuildRoot flattened = buildInput;
            string platform = null;
            Dictionary<string, BuildVarCanonicalized> varLookup;
            string targetName = nullableTargetName;
            Target desiredTarget = null;
            if (nullableTargetName != null)
            {
                desiredTarget = FindTarget(targetName, buildInput.Targets);

                if (desiredTarget == null)
                {
                    throw new InvalidOperationException("Build target does not exist in build file: '" + targetName + "'.");
                }

                platform = desiredTarget.Platform;
            }
            else
            {
                targetName = "cbx";
                desiredTarget = FindTarget(targetName, buildInput.Targets) ?? new Target();
            }

            Dictionary<string, string> replacements = new Dictionary<string, string>() {
                { "TARGET_NAME", targetName }
            };
            varLookup = BuildVarParser.GenerateBuildVars(buildInput, desiredTarget, replacements);

            flattened.Sources = desiredTarget.SourcesNonNull.Union<SourceItem>(flattened.SourcesNonNull).ToArray();
            flattened.Output = FileUtil.GetCanonicalizeUniversalPath(DoReplacement(targetName, desiredTarget.Output ?? flattened.Output));
            flattened.ProjectName = DoReplacement(targetName, desiredTarget.ProjectName ?? flattened.ProjectName);
            flattened.JsFilePrefix = DoReplacement(targetName, desiredTarget.JsFilePrefix ?? flattened.JsFilePrefix);
            flattened.JsFullPageRaw = desiredTarget.JsFullPageRaw ?? flattened.JsFullPageRaw;
            flattened.ImageSheets = MergeImageSheets(desiredTarget.ImageSheets, flattened.ImageSheets);
            flattened.MinifiedRaw = desiredTarget.MinifiedRaw ?? flattened.MinifiedRaw;
            flattened.ExportDebugByteCodeRaw = desiredTarget.ExportDebugByteCodeRaw ?? flattened.ExportDebugByteCodeRaw;
            flattened.GuidSeed = DoReplacement(targetName, desiredTarget.GuidSeed ?? flattened.GuidSeed);
            flattened.IconFilePath = DoReplacement(targetName, desiredTarget.IconFilePath ?? flattened.IconFilePath);
            flattened.LaunchScreen = DoReplacement(targetName, desiredTarget.LaunchScreen ?? flattened.LaunchScreen);
            flattened.DefaultTitle = DoReplacement(targetName, desiredTarget.DefaultTitle ?? flattened.DefaultTitle);
            flattened.Orientation = DoReplacement(targetName, desiredTarget.Orientation ?? flattened.Orientation);
            flattened.CrayonPath = CombineAndFlattenStringArrays(desiredTarget.CrayonPath, flattened.CrayonPath).Select(s => DoReplacement(targetName, s)).ToArray();
            flattened.Description = DoReplacement(targetName, desiredTarget.Description ?? flattened.Description);
            flattened.Version = DoReplacement(targetName, desiredTarget.Version ?? flattened.Version);
            flattened.WindowSize = Size.Merge(desiredTarget.WindowSize, flattened.WindowSize) ?? new Size();
            flattened.CompilerLocale = desiredTarget.CompilerLocale ?? flattened.CompilerLocale;
            flattened.Orientation = desiredTarget.Orientation ?? flattened.Orientation;

            ImageSheet[] imageSheets = flattened.ImageSheets ?? new ImageSheet[0];

            string[] crayonPath = flattened.CrayonPath ?? new string[0];
            crayonPath = crayonPath.Select(t => EnvironmentVariableUtil.DoReplacementsInString(t)).ToArray();

            BuildContext buildContext = new BuildContext()
            {
                ProjectDirectory = projectDir,
                JsFilePrefix = flattened.JsFilePrefix,
                OutputFolder = flattened.Output,
                Platform = platform,
                ProjectID = flattened.ProjectName,
                Minified = flattened.Minified,
                ReadableByteCode = flattened.ExportDebugByteCode,
                GuidSeed = flattened.GuidSeed,
                IconFilePath = flattened.IconFilePath,
                LaunchScreenPath = flattened.LaunchScreen,
                DefaultTitle = flattened.DefaultTitle,
                Orientation = flattened.Orientation,
                CrayonPath = string.Join(";", crayonPath),
                IosBundlePrefix = flattened.IosBundlePrefix,
                JavaPackage = flattened.JavaPackage,
                JsFullPage = flattened.JsFullPage,
                WindowWidth = (flattened.WindowSize ?? new Size()).Width,
                WindowHeight = (flattened.WindowSize ?? new Size()).Height,
                CompilerLocale = Locale.Get((flattened.CompilerLocale ?? "en").Trim()),
            };

            ProgrammingLanguage? nullableLanguage = ProgrammingLanguageParser.Parse(flattened.ProgrammingLanguage ?? "Crayon");
            if (nullableLanguage == null)
            {
                throw new InvalidOperationException("Invalid programming language specified: '" + flattened.ProgrammingLanguage + "'");
            }

            buildContext.TopLevelAssembly = new AssemblyContext(buildContext)
            {
                Description = flattened.Description,
                Version = flattened.Version,
                SourceFolders = ToFilePaths(projectDir, flattened.Sources ?? new SourceItem[0]),
                ImageSheetPrefixesById = imageSheets.ToDictionary<ImageSheet, string, string[]>(s => s.Id, s => s.Prefixes),
                ImageSheetIds = imageSheets.Select<ImageSheet, string>(s => s.Id).ToArray(),
                BuildVariableLookup = varLookup,
                ProgrammingLanguage = nullableLanguage.Value,
            };

            return buildContext.ValidateValues();
        }

        private static string[] CombineAndFlattenStringArrays(string[] a, string[] b)
        {
            List<string> output = new List<string>();
            if (a != null) output.AddRange(a);
            if (b != null) output.AddRange(b);
            return output.ToArray();
        }

        private static string ThrowError(string message)
        {
            throw new InvalidOperationException(message);
        }

        public BuildContext ValidateValues()
        {
            if (this.ProjectID == null) throw new InvalidOperationException("There is no <projectname> for this build target.");


            if (this.TopLevelAssembly.SourceFolders.Length == 0) throw new InvalidOperationException("There are no <source> paths for this build target.");
            if (this.OutputFolder == null) throw new InvalidOperationException("There is no <output> path for this build target.");

            foreach (char c in this.ProjectID)
            {
                if (!((c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z') ||
                    (c >= '0' && c <= '9')))
                {
                    throw new InvalidOperationException("Project ID must be alphanumeric characters only (a-z, A-Z, 0-9)");
                }
            }

            string iconPathRaw = this.IconFilePath;
            if (iconPathRaw != null)
            {
                List<string> absoluteIconPaths = new List<string>();
                foreach (string iconFile in iconPathRaw.Split(','))
                {
                    string trimmedIconFile = iconFile.Trim();
                    if (!FileUtil.IsAbsolutePath(trimmedIconFile))
                    {
                        trimmedIconFile = FileUtil.JoinPath(this.ProjectDirectory, trimmedIconFile);
                    }
                    if (!FileUtil.FileExists(trimmedIconFile))
                    {
                        throw new InvalidOperationException("Icon file path does not exist: " + this.IconFilePath);
                    }
                    absoluteIconPaths.Add(trimmedIconFile);
                }
                this.IconFilePath = string.Join(",", absoluteIconPaths);
            }

            string launchScreenPath = this.LaunchScreenPath;
            if (launchScreenPath != null)
            {
                if (!FileUtil.IsAbsolutePath(launchScreenPath))
                {
                    launchScreenPath = FileUtil.JoinPath(this.ProjectDirectory, launchScreenPath);
                }
                if (!FileUtil.FileExists(launchScreenPath))
                {
                    throw new InvalidOperationException("Launch screen file path does not exist: " + this.LaunchScreenPath);
                }
            }

            return this;
        }

        private static FilePath[] ToFilePaths(string projectDir, SourceItem[] sourceDirs)
        {
            Dictionary<string, FilePath> paths = new Dictionary<string, FilePath>();

            foreach (SourceItem sourceDir in sourceDirs)
            {
                string sourceDirValue = EnvironmentVariableUtil.DoReplacementsInString(sourceDir.Value);
                string relative = FileUtil.GetCanonicalizeUniversalPath(sourceDirValue);
                FilePath filePath = new FilePath(relative, projectDir, sourceDir.Alias);
                paths[filePath.AbsolutePath] = filePath;
            }

            List<FilePath> output = new List<FilePath>();
            foreach (string key in paths.Keys.OrderBy<string, string>(k => k))
            {
                output.Add(paths[key]);
            }
            return output.ToArray();
        }

        private static string DoReplacement(string target, string value)
        {
            return value != null && value.Contains("%TARGET_NAME%")
                ? value.Replace("%TARGET_NAME%", target)
                : value;
        }

        private static ImageSheet[] MergeImageSheets(ImageSheet[] originalSheets, ImageSheet[] newSheets)
        {
            Dictionary<string, List<string>> prefixDirectLookup = new Dictionary<string, List<string>>();
            List<string> order = new List<string>();

            originalSheets = originalSheets ?? new ImageSheet[0];
            newSheets = newSheets ?? new ImageSheet[0];

            foreach (ImageSheet sheet in originalSheets.Concat<ImageSheet>(newSheets))
            {
                if (sheet.Id == null)
                {
                    throw new InvalidOperationException("Image sheet is missing an ID.");
                }

                if (!prefixDirectLookup.ContainsKey(sheet.Id))
                {
                    prefixDirectLookup.Add(sheet.Id, new List<string>());
                    order.Add(sheet.Id);
                }
                prefixDirectLookup[sheet.Id].AddRange(sheet.Prefixes);
            }

            return order
                .Select<string, ImageSheet>(
                    id => new ImageSheet()
                    {
                        Id = id,
                        Prefixes = prefixDirectLookup[id].ToArray()
                    })
                .ToArray();
        }


        public static string GetValidatedCanonicalBuildFilePath(string originalBuildFilePath)
        {
            string buildFilePath = originalBuildFilePath;
            buildFilePath = FileUtil.FinalizeTilde(buildFilePath);
            if (!buildFilePath.StartsWith("/") &&
                !(buildFilePath.Length > 1 && buildFilePath[1] == ':'))
            {
                // Build file will always be absolute. So make it absolute if it isn't already.
                buildFilePath = FileUtil.GetAbsolutePathFromRelativeOrAbsolutePath(buildFilePath);
            }

            if (!FileUtil.FileExists(buildFilePath))
            {
                throw new InvalidOperationException("Build file does not exist: " + originalBuildFilePath);
            }

            return buildFilePath;
        }
    }
}
