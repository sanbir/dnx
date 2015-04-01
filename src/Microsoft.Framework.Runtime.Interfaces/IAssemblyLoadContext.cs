using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Framework.Runtime
{
    public interface IAssemblyLoadContext : IDisposable
    {
        // LOUDO: obsolete for finding usage
        //[Obsolete("Use IAssemblyLoadContext.Load(AssemblyName)")]
        Assembly Load(string name);
        Assembly Load(AssemblyName name);
        Assembly LoadFile(string path);
        Assembly LoadStream(Stream assemblyStream, Stream assemblySymbols);
    }
}