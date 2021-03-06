﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TaskEvents
{
    public class ProgramEvents
    {
        public static void Main()
        {
            Console.WriteLine(@"Задача: 
1. Реализовать интерфейс INotifyPropertyChanged на произвольном классе, продемонстрировать его работу
2. Реализовать очередь которая генерирует событие когда кол-во объектов в ней превышает n и событие когда становится пустой
3. Реализовать класс анализирующий поток чисел и если число отличается более чем x процентов выбрасывает событие");

            // Генерация события при работе с очередью
            CreateQueue();
            Console.ReadKey();

            // Проверка ряда чисел на отклонение от заданного числа
            CreateStreamOfNumbers();

            Console.WriteLine("\nКонец задачи.\n");
            Console.ReadKey();
        }

        // Создание ряда чисел с последующей проверкой его элементов на заданное отклонение
        private static void CreateStreamOfNumbers()
        {
            Random rng = new Random();
            // Генерация ряда чисел
            List<double> volteges = new List<double>();
            for (int i = 0; i < 20; i++)
                volteges.Add(rng.Next(180,260)+rng.NextDouble());

            // Проверка элементов ряда чисел, на заданное отклонение
            Voltmeter vol = new Voltmeter(volteges,10.00);
            Console.WriteLine("\nЗадача 2.\nПроверим ряд чисел на допустимость отклонения от заданно числа - 220 V: ");
            foreach (double val in volteges)
                Console.Write($"{Math.Round(val,2)} ");
            Console.WriteLine("");

            // Вызываем метод CheckVolteges , проверяющий ряд чисел, из класса Voltmeter и передаем в него метод реагирующий на события
            vol.CheckVolteges(EventResponseVoltages);
        }

        private static void EventResponseVoltages(double val, string message) 
        {
            Console.WriteLine($"{message} - {Math.Round(val,2)}");
        }


        // Создание очереди продуктов
        private static void CreateQueue()
        {
            Console.WriteLine("Задача 1: ");
            Console.WriteLine("Введите элементы в очередь Products(string):");
            Products<string> qProducts = new Products<string>(5);
            // Добавление к событию  PropertyChanged метода PrintEvent
            qProducts.PropertyChanged += PrintEvent;

            for (int i = 0; i < 9; i++)
                qProducts.Enqueue(Console.ReadLine());

            while (qProducts.Count() != 0)
                Console.Write($"{qProducts.Dequeue()} ");

            Console.WriteLine("\nEnd.\n");
        }

        private static void PrintEvent(object obj, PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
        }
    }
}
