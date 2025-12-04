namespace AdventOfCode
{
    using AdventOfCode.Apps;
    using AdventOfCode.Readers;
    using AdventOfCode.Writers;

    internal class AppSelector
    {
        internal static IApp Select(int idx)
        {
            var reader = new InputReader(idx);
            var writer = new OutputWriter(idx);

            switch (idx)
            {
                case 0:
                    return new SampleQuestionApp(reader, writer);
                case 1:
                    return new A_SecretEntranceQuestionApp(reader, writer);
                case 2:
                    return new B_SecretEntranceQuestionApp(reader, writer);
                case 3:
                    return new A_GiftShopQuestionApp(reader, writer);
                case 4:
                    return new B_GiftShopQuestionApp(reader, writer);
                case 5:
                    return new A_LobbyQuestionApp(reader, writer);
                case 6:
                    return new B_LobbyQuestionApp(reader, writer);
                case 7:
                    return new A_PrintingDeptQuestionApp(reader, writer);
                case 8:
                    return new B_PrintingDeptQuestionApp(reader, writer);
                default:
                    return new SampleQuestionApp(reader, writer);
            }
        }
    }
}