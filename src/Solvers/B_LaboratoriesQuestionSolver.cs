using AdventOfCode.Utils;

namespace AdventOfCode.Solvers
{
    public class B_LaboratoriesQuestionSolver: ISolver<char[,], long>
    {
        const char SOURCE = 'S';
        const char BEAM = '|';
        const char EMPTY = '.';
        const char SPLITER = '^';

        void Dfs(int i, int j, char[,] input, long[,] dp, ref int count)
        {
            int n = input.GetLength(0);
            int m = input.GetLength(1);

            if(i >= n || i < 0 || j < 0 || j >= m)
                return;

            if(input[i, j] == EMPTY) {
                input[i, j] = BEAM;
                Dfs(i + 1, j, input, dp, ref count);
                dp[i, j] += (i + 1 >= n) ? 1 : dp[i + 1, j];
            }
            else if(input[i, j] == SPLITER)
            {
                count++;
                Dfs(i, j + 1, input, dp, ref count);
                Dfs(i, j - 1, input, dp, ref count);
                dp[i, j] += dp[i, j + 1];
                dp[i, j] += dp[i, j - 1];
            }
        }

        public long Solve(char[,] input)
        {
            int n = input.GetLength(0);
            int m = input.GetLength(1);
            long[,] dp = new long[n, m];
            int cnt = 0;
            long ans = 0;
            for(int j = 0; j < m; j++)
            {
                if(input[0, j] == SOURCE)
                {
                    input[0, j] = EMPTY;
                    Dfs(0, j, input, dp, ref cnt);
                    ans = dp[0, j];
                }
            }
            return ans;
        }

    }
}