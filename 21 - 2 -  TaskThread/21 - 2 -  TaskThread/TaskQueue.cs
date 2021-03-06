﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TaskThread2
{
    internal class TaskQueue : IJobExecutor
    {
        private ConcurrentQueue<Action> queueTasks = new ConcurrentQueue<Action>();
        private CancellationTokenSource cancellationToken;
        private CancellationToken token;
        private Semaphore semaphore;
        private Task totalTask;

        public int Amount { get { return queueTasks.Count; } }

        public void Start(int maxConcurrent)
        {
            if (cancellationToken == null || token.IsCancellationRequested)
            {
                cancellationToken = new CancellationTokenSource();
                token = cancellationToken.Token;
                // Ограничиваем количество одновременно работающих потоков числом maxConcurrent
                semaphore = new Semaphore(maxConcurrent, maxConcurrent);

                totalTask = new Task(Run);
                totalTask.Start();
            }
            else
                Console.WriteLine("Программа уже запущена.");
        }

        private void Run()
        {
            while (!token.IsCancellationRequested)
            {
                if (Amount > 0)
                {
                    Task task = new Task( () => 
                    {
                        semaphore.WaitOne();
                        if (Amount > 0 && !token.IsCancellationRequested)
                        {
                            Action action;
                            queueTasks.TryDequeue(out action);
                            action?.Invoke();
                        }
                        semaphore.Release();
                    });
                    task.Start();
                }
                else
                    Console.WriteLine("Задачи в очереди отсутсвуют.");
            }
        }

        public void Stop()
        {
            if (cancellationToken == null || cancellationToken.IsCancellationRequested)
                Console.WriteLine("Программа на данный момент не запущена или уже остановлена!");
            else
            {
                cancellationToken.Cancel();
                token = cancellationToken.Token;
                Console.WriteLine("Очередь остановлена.");
            }
        }

        public void Add(Action action)
        {
            queueTasks.Enqueue(action);
            Console.WriteLine($"В очередь добавлен поток под номером: {Amount}");
        }

        public void Clear()
        {
            queueTasks = new ConcurrentQueue<Action>(); // При использовании .Net Core 3.0+: queueTasks.Clear();
            Console.WriteLine("Очередь очищена.");
        }
    }
}
