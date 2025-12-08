using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Utils
{
    public class Dsu
    {
        public int n { get; private set; }
        public int[] parent { get; private set; }
        public int[] rank { get; private set; }

        public Dsu(int n)
        {
            this.n = n;
            this.parent = new int[n];
            this.rank = new int[n];
            for(int i = 0; i < n; i++)
                this.parent[i] = i;
            for(int i = 0; i < n; i++)
                this.rank[i] = 1;
        }

        public int Find(int i)
        {
            if(this.parent[i] != i)
            {
                this.parent[i] = Find(this.parent[i]);
            }
            return this.parent[i];
        }

        public bool Union(int u, int v)
        {
            int p_u = Find(u), p_v = Find(v);
            if(p_u == p_v)
                return false;
            if(this.rank[p_u] >= this.rank[p_v])
            {
                this.parent[p_v] = p_u;
                this.rank[p_u] += this.rank[p_v];
            }
            else
            {
                this.parent[p_u] = p_v;
                this.rank[p_v] += this.rank[p_u];
            }
            return true;
        }
    }
}