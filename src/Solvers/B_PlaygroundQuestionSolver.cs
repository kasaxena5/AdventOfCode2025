namespace AdventOfCode.Solvers
{
    using AdventOfCode.Utils;
    public class B_PlaygroundQuestionSolver: ISolver<long[,], long>
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
            int cnt = 0;
            Edge lastEdge = edges[0];
            for(int i = 0; i < edges.Count; i++)
            {
                Edge e = edges[i];
                if(dsu.Union(e.Src.Idx, e.Dst.Idx))
                {
                    cnt++;
                    if(cnt == n - 1)
                    {
                        lastEdge = e;
                        break;
                    }
                }
            }

            return lastEdge.Src.Position[0] * lastEdge.Dst.Position[0];
        }

    }
}