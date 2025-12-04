namespace AdventOfCode.Solvers
{
    public class B_PrintingDeptQuestionSolver: ISolver<char[,], long>
    {
        const char ROLL = '@';
        const char REMOVED = 'X';

        private bool CanBeRemoved(int i, int j, char[,] grid)
        {
            int n = grid.GetLength(0);
            int m = grid.GetLength(1);
            if (i < 0 || i >= n || j >= m || j < 0 || grid[i, j] != ROLL)
            {
                return false;
            }
            int neigh = 0;
            for (int k = 0; k < 8; k++)
            {
                int ni = i + Constants.di[k];
                int nj = j + Constants.dj[k];
                if (ni >= 0 && ni < n && nj >= 0 && nj < m && grid[ni, nj] == ROLL)
                {
                    neigh++;
                }
            }

            return (neigh < 4);
        }

        public long Solve(char[,] input)
        {
            long cnt = 0;
            int n = input.GetLength(0);
            int m = input.GetLength(1);
            Queue<(int, int)> q = new Queue<(int, int)>();

            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    if(CanBeRemoved(i, j, input))
                    {
                        q.Enqueue((i, j));
                        input[i, j] = REMOVED;
                        cnt++;
                    }
                }
            }
            
            while(q.Count > 0)
            {
                int sz = q.Count;
                for(int s = 0; s < sz; s++)
                {
                    (int i, int j) = q.Dequeue();
                    for (int k = 0; k < 8; k++)
                    {
                        int ni = i + Constants.di[k];
                        int nj = j + Constants.dj[k];
                        if (CanBeRemoved(ni, nj, input))
                        {
                            q.Enqueue((ni, nj));
                            input[ni, nj] = REMOVED;
                            cnt++;
                        }
                    }
                }
            }
            return cnt;
        }

    }
}