﻿using System;
using System.Diagnostics;
using System.Threading;

namespace _000_ThreadPool
{
    class Program
    {
        private static int _count = 25;
        private static readonly TimeSpan DURATION = TimeSpan.FromSeconds(10);

        static void Main(string[] args)
        {
            for (int i = 0; i < 25; i++)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    Console.Write(">");
                    //ExecuteIoLike(DURATION);
                    ExecuteComputeLike(DURATION);
                    Console.Write("|");
                    Interlocked.Decrement(ref _count);
                }, i);
            }
            while (_count != 0)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }
            Console.ReadKey();
        }

        private static void ExecuteIoLike(TimeSpan duration)
        {
            Thread.Sleep(duration); // thread pool starvation
        }

        private static void ExecuteComputeLike(TimeSpan duration)
        {
            var sw = Stopwatch.StartNew();
            while (sw.Elapsed < duration)
            {

            }
        }
    }
}