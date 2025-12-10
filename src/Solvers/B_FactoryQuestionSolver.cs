using System.Diagnostics.Metrics;
using System.Numerics;
using AdventOfCode.Utils;

namespace AdventOfCode.Solvers
{
    public class B_FactoryQuestionSolver: ISolver<string[], long>
    {
        const char LIGHTS_ON = '#';
        const char LIGHTS_OFF = '.';

        private string Hash(long[] nums)
        {
            return string.Join(",", nums);
        }

        private bool CanSubtract(long[] counters, long button)
        {
            int n = counters.Length;
            for(int i = 0; i < n; i++)
            {
                if(((1 << i) & button) != 0 && counters[i] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private long[] Subtract(long[] counters, long button)
        {
            int n = counters.Length;
            long[] newCounters = new long[n];
            for(int i = 0; i < n; i++)
            {
                if(((1 << i) & button) != 0)
                    newCounters[i] = counters[i] - 1;
                else
                    newCounters[i] = counters[i];
            }

            return newCounters;
        }

        private long MinimumCounter(long[] counters, long[] buttons, Dictionary<string, long> dp)
        {
            int n = counters.Length;
            if(Hash(counters) == Hash(new long[n]))
            {
                return 0;
            }

            if(dp.ContainsKey(Hash(counters)))
                return dp[Hash(counters)];

            long ans = int.MaxValue;
            foreach(var button in buttons)
            {
                if(CanSubtract(counters, button))
                {
                    var newCounters = Subtract(counters, button);
                    var val = MinimumCounter(newCounters, buttons, dp);
                    ans = Math.Min(ans, 1 + val);
                }
            }

            dp[Hash(counters)] = ans;
            return ans;
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
            
            long[][] joltages = input
                .Select(row =>
                {
                    var elements = row.Split(" ");
                    string str = elements[elements.Length - 1];
                    string joltageStr = str.Substring(1, str.Length - 2);
                    return joltageStr
                    .Split(",")
                    .Select(long.Parse)
                    .ToArray();
                })
                .ToArray();

            
            long[][] buttons = input
                .Select(row =>
                {
                    var elements = row.Split(" ");
                    var buttonElements = elements
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

                    Array.Sort(buttonElements, Comparer<long>.Create((a, b) => BitOperations.PopCount((ulong)b).CompareTo(BitOperations.PopCount((ulong)a))));

                    return buttonElements;
                })
                .ToArray();
            
            long ans = 0;
            int n = joltages.Length;
            for(int i = 0; i < n; i++)
            {
                ans += MinimumCounter(joltages[i], buttons[i], new Dictionary<string, long>());
            }

            return ans;
        }

    }
}