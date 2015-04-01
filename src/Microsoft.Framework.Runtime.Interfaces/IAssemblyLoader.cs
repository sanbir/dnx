// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Microsoft.Framework.Runtime
{
    public interface IAssemblyLoader
    {
        //LOUDO: obsolete for finding usage
        //[Obsolete("Use IAssemblyLoader.Load(AssemblyName)")]
        Assembly Load(string name);
        Assembly Load(AssemblyName name);
    }
}
