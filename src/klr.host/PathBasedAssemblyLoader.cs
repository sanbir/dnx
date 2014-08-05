// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Framework.Runtime;

namespace klr.host
{
    public class PathBasedAssemblyLoader : IAssemblyLoader
    {
        private static readonly string[] _extensions = new string[] { ".dll", ".exe" };

        private readonly string[] _searchPaths;

        public PathBasedAssemblyLoader(string[] searchPaths)
        {
            _searchPaths = searchPaths;
        }

        public Assembly Load(IAssemblyLoadContext loadContext, string name)
        {
            foreach (var path in _searchPaths)
            {
                foreach (var extension in _extensions)
                {
                    var filePath = Path.Combine(path, name + extension);

                    if (File.Exists(filePath))
                    {
                        return loadContext.LoadFile(filePath);
                    }
                }
            }

            return null;
        }
    }
}
