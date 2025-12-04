namespace AdventOfCode.Solvers
{
    public class A_PrintingDeptQuestionSolver: ISolver<char[,], long>
    {
        const char ROLL = '@';
        public long Solve(char[,] input)
        {
            long cnt = 0;
            int n = input.GetLength(0);
            int m = input.GetLength(1);
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    if (input[i, j] == ROLL)
                    {
                        int neigh = 0;
                        for (int k = 0; k < 8; k++)
                        {
                            int ni = i + Constants.di[k];
                            int nj = j + Constants.dj[k];
                            if (ni >= 0 && ni < n && nj >= 0 && nj < m && input[ni, nj] == ROLL)
                            {
                                neigh++;
                            }
                        }

                        if (neigh < 4)
                            cnt++;
                    }
                }
            }
            return cnt;
        }

    }
}