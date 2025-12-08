namespace AdventOfCode.Apps
{
    using AdventOfCode.InputParsers;
    using AdventOfCode.OutputParsers;
    using AdventOfCode.Readers;
    using AdventOfCode.Solvers;
    using AdventOfCode.Writers;

    public class B_PlaygroundQuestionApp : IApp<long[,], long>
    {
        public IInputParser<long[,]> inputParser => new GridInputParser<long>(Environment.NewLine, ",");
        public IOutputParser<long> outputParser => new StandardOutputParser<long>();
        public ISolver<long[,], long> solver => new B_PlaygroundQuestionSolver();
        public IOutputWriter writer => writerInternal;
        public IInputReader reader => readerInternal;

        private IOutputWriter writerInternal;
        private IInputReader readerInternal;

        public B_PlaygroundQuestionApp(IInputReader reader, IOutputWriter writer)
        {
            this.writerInternal = writer;
            this.readerInternal = reader;
        }
    }
}