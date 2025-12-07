using AdventOfCode.Utils;

namespace AdventOfCode.Solvers
{
    public class A_LaboratoriesQuestionSolver: ISolver<char[,], long>
    {
        const char SOURCE = 'S';
        const char BEAM = '|';
        const char EMPTY = '.';
        const char SPLITER = '^';

        void Dfs(int i, int j, char[,] input, ref int count)
        {
            // PrintUtils<char>.Print(input);
            // Console.WriteLine("--------------------------------------");
            int n = input.GetLength(0);
            int m = input.GetLength(1);
            if(i >= n || i < 0 || j < 0 || j >= m)
                return;
            if(input[i, j] == EMPTY) {
                input[i, j] = BEAM;
                Dfs(i + 1, j, input, ref count);
            }
            else if(input[i, j] == SPLITER)
            {
                count++;
                Dfs(i, j + 1, input, ref count);
                Dfs(i, j - 1, input, ref count);
            }
        }

        public long Solve(char[,] input)
        {
            int n = input.GetLength(0);
            int m = input.GetLength(1);
            int cnt = 0;
            for(int j = 0; j < m; j++)
            {
                if(input[0, j] == SOURCE)
                {
                    input[0, j] = EMPTY;
                    Dfs(0, j, input, ref cnt);
                }
            }
            return cnt;
        }

    }
}