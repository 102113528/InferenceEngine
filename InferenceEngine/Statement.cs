using System;
using System.Collections.Generic;

namespace InferenceEngine
{
    public class Statement
    {
        public readonly string Raw;
        public readonly string Symbol;
        public readonly List<string> Children = new List<string>();

        public Statement(string raw)
        {
            Raw = raw;

            string statement = raw.Replace(" ", string.Empty);

            if (statement.Contains(Operator.Implication))
            {
                string[] tokens = statement.Split(Operator.Implication);
                Symbol = tokens[1];

                if (tokens[0].Contains(Operator.Conjunction))
                {
                    tokens = tokens[0].Split(Operator.Conjunction);
                    foreach (string symbol in tokens) Children.Add(symbol);
                }
                else Children.Add(tokens[0]);
            }
            else Symbol = statement;

#if DEBUG
            Console.Write($"Statement: {Raw}, Symbol: {Symbol}");

            if (Children.Count > 0)
            {
                Console.Write($", Children: {Children[0]}");
                for (int i = 1; i < Children.Count; i++) Console.Write($", {Children[i]}");
            }

            Console.WriteLine();
#endif
        }
    }
}