using System.Reflection;

[assembly: AssemblyProduct("Core.ExceptionHandling")]

[assembly: AssemblyCompany("CustomCode")]
[assembly: AssemblyCopyright("Copyright © 2018")]
[assembly: AssemblyTrademark("C# exception handling component")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif