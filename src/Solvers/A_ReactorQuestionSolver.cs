namespace AdventOfCode.Solvers
{
    public class A_ReactorQuestionSolver: ISolver<string[], long>
    {
        private class Graph
        {
            Dictionary<string, List<string>> adj;
            Dictionary<string, int> color;
            Dictionary<string, long> paths;
            int n;

            public Graph(int n)
            {
                this.n = n;
                adj = new Dictionary<string, List<string>>();
                color = new Dictionary<string, int>();
                paths = new Dictionary<string, long>();
            }

            public void Print()
            {
                foreach(var kvp in adj)
                {
                    Console.WriteLine($"{kvp.Key}: {string.Join(',', kvp.Value)}");
                }
            }

            public void AddEdge(string u, string v)
            {
                if(!adj.ContainsKey(u))
                    adj[u] = new List<string>();
                
                adj[u].Add(v);
            }

            public void CountPaths(string u, string target)
            {
                color[u] = 1;
                if(adj.ContainsKey(u)) {
                    foreach (var v in adj[u])
                    {
                        if (!color.ContainsKey(v))
                            CountPaths(v, target);
                    }
                }
                color[u] = 2;
                paths[u] = (u == target) ? 1 : 0;

                if(adj.ContainsKey(u)) {
                    foreach(var v in adj[u])
                    {
                        paths[u] += paths[v];
                    }
                }
            }

            public long GetPaths(string u)
            {
                return paths[u];
            }
        }
        const string SRC = "you";
        const string DST = "out";

        public long Solve(string[] input)
        {
            int n = input.Length;
            Graph g = new Graph(n);
            foreach(string line in input)
            {
                string[] parts = line.Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                string node = parts[0];
                string[] neighbours = parts[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                foreach(string neighbour in neighbours)
                {
                    g.AddEdge(node, neighbour);
                }
            }
            g.CountPaths(SRC, DST);
            return g.GetPaths(SRC);
        }

    }
}