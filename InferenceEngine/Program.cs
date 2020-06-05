using System;
using System.Collections.Generic;
using System.IO;
using InferenceEngine.Solutions;

namespace InferenceEngine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid number of arguments!");
                Console.WriteLine("Usage: iengine <method> <filename>");

                return;
            }

            string method = args[0].ToUpper(), path = args[1];

            List<Statement> statements = new List<Statement>();
            string goal = string.Empty;

            string[] lines = File.ReadAllLines(path);
            int state = 0; // 0 - None, 1 - Tell, 2 - Ask

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                switch (line)
                {
                    case "TELL":
                        state = 1;
                        continue;
                    case "ASK":
                        state = 2;
                        continue;
                }

                if (state == 1) // Tell
                {
                    string[] tokens = line.Split(';');

                    for (int j = 0; j < tokens.Length; j++)
                    {
                        string token = tokens[j].Trim();
                        if (string.IsNullOrWhiteSpace(token)) continue;

                        Statement statement = new Statement(token);
                        statements.Add(statement);
                    }
                }
                else if (state == 2) // Ask
                {
                    goal = line;
                    break;
                }
            }

            if (statements.Count == 0 || string.IsNullOrWhiteSpace(goal))
            {
                Console.WriteLine("The knowledge base and / or the query is / are empty!");
                return;
            }

            ISolution solution = method switch
            {
                "TT" => new TruthTableSolution(),
                "FC" => new ForwardChainingSolution(),
                "BC" => new BackwardChainingSolution(),
                _ => null
            };

            if (solution == null)
            {
                Console.WriteLine("Invalid method!");

                Console.WriteLine("Methods:");
                Console.WriteLine("- TT: Truth table solution");
                Console.WriteLine("- FC: Forward chaining solution");
                Console.WriteLine("- BC: Backward chaining solution");

                return;
            }

            string result = solution.Solve(statements, goal);

            if (result == null)
            {
                Console.WriteLine("Unable to find a solution!");
                return;
            }

            Console.WriteLine(result);
        }
    }
}