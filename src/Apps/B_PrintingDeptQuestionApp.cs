namespace AdventOfCode.Apps
{
    using AdventOfCode.InputParsers;
    using AdventOfCode.OutputParsers;
    using AdventOfCode.Readers;
    using AdventOfCode.Solvers;
    using AdventOfCode.Writers;

    public class B_PrintingDeptQuestionApp : IApp<char[,], long>
    {
        public IInputParser<char[,]> inputParser => new GridInputParser<char>(Environment.NewLine);
        public IOutputParser<long> outputParser => new StandardOutputParser<long>();
        public ISolver<char[,], long> solver => new B_PrintingDeptQuestionSolver();
        public IOutputWriter writer => writerInternal;
        public IInputReader reader => readerInternal;

        private IOutputWriter writerInternal;
        private IInputReader readerInternal;

        public B_PrintingDeptQuestionApp(IInputReader reader, IOutputWriter writer)
        {
            this.writerInternal = writer;
            this.readerInternal = reader;
        }
    }
}