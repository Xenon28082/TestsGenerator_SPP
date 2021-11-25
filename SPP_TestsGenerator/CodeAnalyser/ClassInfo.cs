using System.Collections.Generic;

namespace SPP_TestsGenerator.CodeAnalyser
{
    public class ClassInfo
    {
        public string Name { get; }
        public List<MethodInfo> Methods { get; }

        public ClassInfo(string name, IEnumerable<MethodInfo> methods)
        {
            Name = name;
            Methods = new List<MethodInfo>(methods);
        }
    }
}