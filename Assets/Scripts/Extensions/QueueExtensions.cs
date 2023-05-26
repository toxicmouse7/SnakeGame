using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class QueueExtensions
    {
        public static void ForEach<T>(this Queue<T> queue, Action<T> result)
        {
            foreach (var element in queue)
            {
                result(element);
            }
        }
    }
}