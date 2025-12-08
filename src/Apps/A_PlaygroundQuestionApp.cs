namespace AdventOfCode.Apps
{
    using AdventOfCode.InputParsers;
    using AdventOfCode.OutputParsers;
    using AdventOfCode.Readers;
    using AdventOfCode.Solvers;
    using AdventOfCode.Writers;

    public class A_PlaygroundQuestionApp : IApp<long[,], long>
    {
        public IInputParser<long[,]> inputParser => new GridInputParser<long>(Environment.NewLine, ",");
        public IOutputParser<long> outputParser => new StandardOutputParser<long>();
        public ISolver<long[,], long> solver => new A_PlaygroundQuestionSolver();
        public IOutputWriter writer => writerInternal;
        public IInputReader reader => readerInternal;

        private IOutputWriter writerInternal;
        private IInputReader readerInternal;

        public A_PlaygroundQuestionApp(IInputReader reader, IOutputWriter writer)
        {
            this.writerInternal = writer;
            this.readerInternal = reader;
        }
    }
}