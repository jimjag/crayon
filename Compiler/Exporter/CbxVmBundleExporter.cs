﻿using AssemblyResolver;
using Build;
using Common;
using CommonUtil.Disk;
using System;
using System.Collections.Generic;

namespace Exporter
{
    public static class CbxVmBundleExporter
    {
        public static ExportResponse Run(
            string platformId,
            string projectDirectory,
            string outputDirectory,
            string byteCode,
            ResourceDatabase resourceDatabase,
            IList<AssemblyMetadata> assemblies,
            ExportRequest exportRequest,
            Platform.IPlatformProvider platformProvider,
            bool isRelease)
        {
            if (isRelease)
            {
                try
                {
                    RunImpl(platformId, projectDirectory, outputDirectory, byteCode, resourceDatabase, assemblies, exportRequest, platformProvider);
                }
                catch (InvalidOperationException ioe)
                {
                    return new ExportResponse()
                    {
                        Errors = new Error[] { new Error() { Message = ioe.Message } },
                    };
                }
            }
            else
            {
                RunImpl(platformId, projectDirectory, outputDirectory, byteCode, resourceDatabase, assemblies, exportRequest, platformProvider);
            }
            return new ExportResponse();
        }

        public static void RunImpl(
            string platformId,
            string projectDirectory,
            string outputDirectory,
            string byteCode,
            ResourceDatabase resourceDatabase,
            IList<AssemblyMetadata> assemblies,
            ExportRequest exportRequest,
            Platform.IPlatformProvider platformProvider)
        {
            Platform.AbstractPlatform platform = platformProvider.GetPlatform(platformId);
            if (platform == null) throw new InvalidOperationException("Unrecognized platform. See usage.");

            if (!Path.IsAbsolute(outputDirectory))
            {
                outputDirectory = FileUtil.JoinPath(projectDirectory, outputDirectory);
            }
            outputDirectory = FileUtil.GetCanonicalizeUniversalPath(outputDirectory);
            FileOutputExporter exporter = new FileOutputExporter(outputDirectory);

            VmGenerator vmGenerator = new VmGenerator();
            Dictionary<string, FileOutput> result = new Dictionary<string, FileOutput>();
            vmGenerator.GenerateVmSourceCodeForPlatform(
                result,
                byteCode,
                platform,
                exportRequest,
                resourceDatabase,
                assemblies,
                outputDirectory,
                VmGenerationMode.EXPORT_SELF_CONTAINED_PROJECT_SOURCE);

            exporter.ExportFiles(result);
        }
    }
}