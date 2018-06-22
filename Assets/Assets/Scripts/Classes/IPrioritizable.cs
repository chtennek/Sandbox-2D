using System;
using System.Collections.Generic;

public interface IPrioritizable
{
    int Priority { get; }
}

public class PriorityComparer : IComparer<IPrioritizable>
{
    public int Compare(IPrioritizable x, IPrioritizable y)
    {
        return x.Priority.CompareTo(y.Priority);
    }
}