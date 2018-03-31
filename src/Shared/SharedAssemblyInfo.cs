using System.Reflection;

[assembly: AssemblyProduct("Core.DesignByContract")]

[assembly: AssemblyCompany("CustomCode")]
[assembly: AssemblyCopyright("Copyright © 2018")]
[assembly: AssemblyTrademark("C# design by contract")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif