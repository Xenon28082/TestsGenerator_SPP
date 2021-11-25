namespace SPP_TestsGenerator
{
    public class PipeFrame
    {
        public string path;
        public string code;

        public PipeFrame(string path)
        {
            this.path = path;
        }

        public PipeFrame(string path, string code)
        {
            this.path = path;
            this.code = code;
        }
    }
}