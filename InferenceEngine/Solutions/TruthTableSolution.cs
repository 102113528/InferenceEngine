using System.Collections.Generic;

namespace InferenceEngine.Solutions
{
    public class TruthTableSolution : ISolution
    {
        private int _entailedCount;

        public string Solve(List<Statement> statements, string goal)
        {
            List<string> symbols = GetAllSymbols(statements);

            Dictionary<string, bool> model = new Dictionary<string, bool>();
            foreach (string symbol in symbols) model.Add(symbol, false);

            bool result = CheckAll(statements, goal, symbols, model);
            return result ? $"YES: {_entailedCount}" : "NO";
        }

        /// <summary>
        /// Checks each model and determines if the query / goal used with the model evaluates to true.
        /// </summary>
        /// <param name="statements">The knowledge base (a list of Statement objects).</param>
        /// <param name="goal">The goal / query (a single atomic symbol).</param>
        /// <param name="symbols">A list of all unique symbols.</param>
        /// <param name="model">The current model of the truth table.</param>
        /// <returns>Returns true or false depending on what the current model used with the query / goal evaluates to.</returns>
        private bool CheckAll(List<Statement> statements, string goal, List<string> symbols,
            Dictionary<string, bool> model)
        {
            if (symbols.Count == 0)
            {
                if (PlTrue(statements, model))
                {
                    _entailedCount++;
                    return model.ContainsKey(goal) && model[goal];
                }

                return true;
            }

            string symbol = symbols[0];
            symbols = symbols.GetRange(1, symbols.Count - 1);

            model[symbol] = true;
            bool left = CheckAll(statements, goal, symbols, model);

            model[symbol] = false;
            bool right = CheckAll(statements, goal, symbols, model);

            return left && right;
        }

        /// <summary>
        /// Checks for any false statements in the knowledge base.
        /// </summary>
        /// <param name="statements">The knowledge base (a list of Statement objects).</param>
        /// <param name="model">The current model of the truth table.</param>
        /// <returns>Returns true or false depending on the current state of the statements in the knowledge base.</returns>
        private bool PlTrue(List<Statement> statements, Dictionary<string, bool> model)
        {
            foreach (Statement statement in statements)
            {
                bool result = true;
                foreach (string child in statement.Children) result = result && model[child];
                result = !result || model[statement.Symbol];

                if (!result) return false;
            }

            return true;
        }

        /// <summary>
        /// Calculates all unique symbols in a given knowledge base.
        /// </summary>
        /// <param name="statements">The knowledge base (a list of Statement objects).</param>
        /// <returns>Returns a list of all unique symbols in the knowledge base.</returns>
        private List<string> GetAllSymbols(List<Statement> statements)
        {
            List<string> result = new List<string>();

            foreach (Statement statement in statements)
            {
                if (!result.Contains(statement.Symbol)) result.Add(statement.Symbol);

                foreach (string child in statement.Children)
                {
                    if (!result.Contains(child)) result.Add(child);
                }
            }

            return result;
        }
    }
}