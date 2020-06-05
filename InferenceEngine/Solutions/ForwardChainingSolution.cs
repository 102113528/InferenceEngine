using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InferenceEngine.Solutions
{
    public class ForwardChainingSolution : ISolution
    {
        public string Solve(List<Statement> statements, string goal)
        {
            Dictionary<Statement, int> count = new Dictionary<Statement, int>();
            Queue<string> frontier = new Queue<string>();

            foreach (Statement statement in statements)
            {
                count.Add(statement, statement.Children.Count);
                if (statement.Children.Count == 0) frontier.Enqueue(statement.Symbol);
            }

            List<string> result = new List<string>();

            while (frontier.Count > 0)
            {
                string symbol = frontier.Dequeue();
                if (!result.Contains(symbol)) result.Add(symbol);

                foreach (Statement statement in statements)
                {
                    if (statement.Children.All(x => x != symbol)) continue;

                    count[statement]--;
                    if (count[statement] != 0) continue;

                    if (statement.Symbol == goal)
                    {
                        StringBuilder builder = new StringBuilder($"YES: {result[0]}");
                        for (int i = 1; i < result.Count; i++) builder.Append($", {result[i]}");
                        builder.Append($", {goal}");

                        return builder.ToString();
                    }

                    frontier.Enqueue(statement.Symbol);
                }
            }

            return "NO";
        }
    }
}