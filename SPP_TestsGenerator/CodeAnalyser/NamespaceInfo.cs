namespace SPP_TestsGenerator.CodeAnalyser
{
    public class NamespaceInfo
    {
        public string Name { get; }
        public ClassInfo ClassName { get; }

        public NamespaceInfo(string name, ClassInfo className)
        {
            Name = name;
            ClassName = className;
        }
    }
}