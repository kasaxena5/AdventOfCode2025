namespace AdventOfCode.Utils
{
    public static class PrintUtils<T>
    {
        public static void Print(T[,] grid)
        {
            int n = grid.GetLength(0);
            int m = grid.GetLength(1);
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    Console.Write($"{grid[i, j]} ");
                }
                Console.WriteLine();
            }
        }
    }
}