using System.Runtime.InteropServices.Marshalling;

namespace AdventOfCode.Solvers
{
    public class B_ReactorQuestionSolver: ISolver<string[], long>
    {
        private class Graph
        {
            Dictionary<string, List<string>> adj;
            int n;

            public Graph(int n)
            {
                this.n = n;
                adj = new Dictionary<string, List<string>>();
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

            public long CountPaths(string u, string target, Dictionary<string, long> paths, Dictionary<string, int> color)
            {
                color[u] = 1;
                if(adj.ContainsKey(u)) {
                    foreach (var v in adj[u])
                    {
                        if (!color.ContainsKey(v))
                            CountPaths(v, target, paths, color);
                        else
                        {
                            if(color[v] == 1)
                            {
                                Console.WriteLine("BackEdge detected");
                            }
                        }
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

                return paths[u];
            }
        }

        const string SRC = "svr";
        const string STOP_1 = "fft";
        const string STOP_2 = "dac";
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

            long ans = 0;
            ans += (
                g.CountPaths(SRC, STOP_1, new Dictionary<string, long>(), new Dictionary<string, int>()) * 
                g.CountPaths(STOP_1, STOP_2, new Dictionary<string, long>(), new Dictionary<string, int>()) *
                g.CountPaths(STOP_2, DST, new Dictionary<string, long>(), new Dictionary<string, int>()));

            ans += (
                g.CountPaths(SRC, STOP_2, new Dictionary<string, long>(), new Dictionary<string, int>()) *
                g.CountPaths(STOP_2, STOP_1, new Dictionary<string, long>(), new Dictionary<string, int>()) *
                g.CountPaths(STOP_1, DST, new Dictionary<string, long>(), new Dictionary<string, int>()));

            return ans;
        }

    }
}