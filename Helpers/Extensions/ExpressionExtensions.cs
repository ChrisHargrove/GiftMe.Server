using System.Linq.Expressions;

namespace Helpers.Extensions;

/// <summary>
/// Extensions class for everything related to Expressions.
/// </summary>
public static class ExpressionExtensions {
    /// <summary>
    /// Gets the member name of a expression that is just a member expression or a unary expression.
    /// </summary>
    /// <param name="accessor">Expression that gives access to a member of a class.</param>
    /// <param name="defaultName">The name to return if no name could be accessed from the expression.</param>
    /// <typeparam name="T">Any Type</typeparam>
    /// <returns>
    /// Returns the name of the member specified by the accessor.
    /// </returns>
    public static string GetMemberName<T>(this Expression<T> accessor, string defaultName = "")
        => accessor.Body switch {
            MemberExpression m => m.Member.Name,
            UnaryExpression { Operand: MemberExpression m } => m.Member.Name,
            _ => defaultName
        };
}