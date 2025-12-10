namespace AdventOfCode.Solvers
{
    public class A_FactoryQuestionSolver: ISolver<string[], long>
    {
        const char LIGHTS_ON = '#';
        const char LIGHTS_OFF = '.';

        private long MinimumSwitches(long light, long[] buttons)
        {
            Queue<long> q = new Queue<long>();
            HashSet<long> visited = new HashSet<long>();

            q.Enqueue(0);
            visited.Add(0);

            long level = 0;
            while(q.Count > 0)
            {
                int sz = q.Count;
                for(int i = 0; i < sz; i++)
                {
                    long u = q.Dequeue();
                    foreach(long button in buttons)
                    {
                        long v = u ^ button;
                        if(v == light)
                            return level + 1;
                        
                        if(!visited.Contains(v))
                        {
                            q.Enqueue(v);
                            visited.Add(v);
                        }
                    }
                }
                
                level++;
            }

            return -1;
        }

        public long Solve(string[] input)
        {
            long[] lights = input
                .Select(row =>
                {
                    var elements = row.Split(" ");
                    return elements[0];
                })
                .Select(element =>
                {
                    long bitmap = 0;
                    string diagram = element.Substring(1, element.Length - 2);
                    for(int i = 0; i < diagram.Length; i++)
                    {
                        if(diagram[i] == LIGHTS_ON) {
                            bitmap += 1 << i;
                        }
                    }
                    return bitmap;
                })
                .ToArray();
            
            long[][] buttons = input
                .Select(row =>
                {
                    var elements = row.Split(" ");
                    return elements
                    .Where(str => str.Contains("("))
                    .Select(str =>
                    {
                        long ans = 0;
                        string buttonStr = str.Substring(1, str.Length - 2);
                        var lightsSwitched = buttonStr.Split(",");
                        foreach(var lightSwitched in lightsSwitched)
                        {
                            ans += 1 << int.Parse(lightSwitched);
                        }
                        return ans;
                    })
                    .ToArray();
                })
                .ToArray();

            int n = lights.Length;
            long ans = 0;
            for(int i = 0; i < n; i++)
            {
                ans += MinimumSwitches(lights[i], buttons[i]);
            }

            return ans;
        }

    }
}