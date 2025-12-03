using AdventOfCode.Utils;

namespace AdventOfCode.Solvers
{
    public class B_LobbyQuestionSolver: ISolver<int[,], long>
    {
        public long Solve(int[,] input)
        {
            int n = input.GetLength(0);
            int m = input.GetLength(1);
            long ans = 0;
            for(int i = 0; i < n; i++)
            {
                long[,] dp = new long[m, 13];
                for(int j = m - 1; j >= 0; j--) {
                    dp[j, 1] = (long)input[i, j];
                    if(j + 1 < m)
                        dp[j, 1] = Math.Max(dp[j, 1], dp[j + 1, 1]);
                }

                for(int d = 2; d <= 12; d++) {
                    for(int j = m - d; j >= 0; j--) {
                        dp[j, d] = input[i, j] * MathUtils.Pow(10, d - 1) + dp[j + 1, d - 1];
                        dp[j, d] = Math.Max(dp[j, d], dp[j + 1, d]);
                    }
                }

                ans += dp[0, 12];

            }
            return ans;
        }
    }
}