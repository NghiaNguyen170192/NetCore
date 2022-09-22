using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCore.Shared.Extensions;

public static class Sort
{
    public static IEnumerable<TNode> TopologicalSort<TNode>(this IEnumerable<TNode> nodes, Func<TNode, IEnumerable<Type>> dependencySelector)
    {
        var elements = nodes.ToDictionary(node => new KeyValuePair<Type, TNode>(node.GetType(), node), node => new HashSet<Type>(dependencySelector(node)));

        while (elements.Count > 0)
        {
            var element = elements.FirstOrDefault(x => x.Value.Count == 0);

            if (element.Key.Value == null)
            {
                throw new ArgumentException($"Circular dependency: {element.Key.Key.Name}");
            }

            elements.Remove(element.Key);

            foreach (var sortedElement in elements)
            {
                sortedElement.Value.Remove(element.Key.Key);
            }

            yield return element.Key.Value;
        }
    }
}
