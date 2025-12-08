namespace AdventOfCode.Solvers
{
    using AdventOfCode.Utils;
    public class A_PlaygroundQuestionSolver: ISolver<long[,], long>
    {
        public class Node
        {
            public int Idx { get; set; }
            public long[] Position { get; set; }
            public Node(int i, long[] position)
            {
                this.Idx = i;
                this.Position = position;
            }

            public void Print()
            {
                Console.WriteLine($"Node{Idx}: {string.Join(",", Position)}");
            }
        }

        public class Edge
        {
            public Node Src { get; set; }

            public Node Dst { get; set; }

            public Edge(Node src, Node dst)
            {
                this.Src = src;
                this.Dst = dst;
            }

            public long Distance()
            {
                int n = Src.Position.Length;
                long dist = 0;
                for(int i = 0; i < n; i++)
                {
                    dist += (Src.Position[i] - Dst.Position[i]) * (Src.Position[i] - Dst.Position[i]);
                }
                return dist;
            }

            public void Print()
            {
                Console.WriteLine($"Src: ");
                Src.Print();
                Console.WriteLine($"Dst: ");
                Dst.Print();
            }
        }

        public long Solve(long[,] input)
        {
            int n = input.GetLength(0);
            Node[] nodes = new Node[n];
            for(int i = 0; i < n; i++)
            {
                long[] position = new long[3];
                for(int k = 0; k < 3; k++)
                    position[k] = input[i, k];
                nodes[i] = new Node(i, position);
            }

            List<Edge> edges = new List<Edge>();
            for(int i = 0; i < n; i++)
            {
                for(int j = i + 1; j < n; j++)
                {
                    edges.Add(new Edge(nodes[i], nodes[j]));
                }
            }
            edges.Sort(Comparer<Edge>.Create((a, b) => a.Distance().CompareTo(b.Distance())));

            Dsu dsu = new Dsu(n);
            for(int i = 0; i < 1000; i++)
            {
                Edge e = edges[i];
                dsu.Union(e.Src.Idx, e.Dst.Idx);
            }

            SortedDictionary<long, long> size = new SortedDictionary<long, long>();
            for(int i = 0; i < n; i++)
            {
                int p = dsu.Find(i);
                if(!size.ContainsKey(p))
                {
                    size[p] = 0;
                }
                size[p]++;
            }

            List<long> biggest = new List<long>();
            foreach(var kvp in size)
            {
                biggest.Add(kvp.Value);
            }
            biggest.Sort(Comparer<long>.Create((a, b) => b.CompareTo(a)));

            return biggest[0] * biggest[1] * biggest[2];
        }

    }
}