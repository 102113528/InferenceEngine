using System.Collections.Generic;
using System.Text;

namespace InferenceEngine.Solutions
{
    public class BackwardChainingSolution : ISolution
    {
        public string Solve(List<Statement> statements, string goal)
        {
            List<string> atomicSymbols = new List<string>();

            foreach (Statement statement in statements)
            {
                if (statement.Children.Count == 0) atomicSymbols.Add(statement.Symbol);
            }

            Stack<string> frontier = new Stack<string>();
            frontier.Push(goal);

            List<string> result = new List<string>();
            result.Add(goal);

            while (frontier.Count > 0)
            {
                string symbol = frontier.Pop();
                if (!result.Contains(symbol)) result.Add(symbol);

                if (atomicSymbols.Contains(symbol)) continue;

                int count = frontier.Count;

                foreach (Statement statement in statements)
                {
                    if (statement.Symbol != symbol) continue;

                    foreach (string child in statement.Children)
                    {
                        if (!result.Contains(child)) frontier.Push(child);
                    }
                }

                if (count == frontier.Count) return "NO";
            }

            result.Reverse();

            StringBuilder builder = new StringBuilder($"YES: {result[0]}");
            for (int i = 1; i < result.Count; i++) builder.Append($", {result[i]}");

            return builder.ToString();
        }
    }
}