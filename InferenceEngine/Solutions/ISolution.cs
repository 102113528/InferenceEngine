using System.Collections.Generic;

namespace InferenceEngine.Solutions
{
    public interface ISolution
    {
        /// <summary>
        /// The calling method for solving the query with the given knowledge base.
        /// </summary>
        /// <param name="statements">The knowledge base (a list of Statement objects).</param>
        /// <param name="goal">The goal / query (a single atomic symbol).</param>
        /// <returns>Returns "YES" or "NO" with additional information specific to the solution.</returns>
        string Solve(List<Statement> statements, string goal);
    }
}