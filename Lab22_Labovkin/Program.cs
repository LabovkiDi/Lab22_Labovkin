using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab22_Labovkin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество элементов массива");
            int n = Convert.ToInt32(Console.ReadLine());

            //делегат, принимает object и возвращающий результат.В качестве аргумента метод GetArray
            Func<object, int[]> func = new Func<object, int[]>(GetArray);
            //параметризированный Task,возвращающий массив целых чисел
            Task<int[]> task1 = new Task<int[]>(func, n);
            //делегат, параметризированный Task,не возвращающий значение.В качестве аргумента метод SumArray
            Action<Task<int[]>> action1 = new Action<Task<int[]>>(SumArray);
            //не обобщенный Task с делегатом action1
            Task task2 = task1.ContinueWith(action1);
            //делегат, параметризированный Task,не возвращающий значение.В качестве аргумента метод MaxArray
            Action<Task<int[]>> action2 = new Action<Task<int[]>>(MaxArray);
            //не обобщенный Task с делегатом action2
            Task task3 = task1.ContinueWith(action2);

            //запуск
            task1.Start();
            Console.ReadKey();
        }
        //метод форрмирования массива случайных чисел
        static int[] GetArray(object a)
        {
            //приводим object a к int a
            int n = (int)a;
            int[] array = new int[n];
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                array[i] = random.Next(0, 100);
            }
            return array;
        }
        //метод, вычисляющий сумму элементов массива  
        static void SumArray(Task<int[]> task)
        {
            //получаем массив через вызов Result
            int[] array = task.Result;
            int s = 0;
            for (int i = 0; i < array.Count(); i++) /*array.Count вместо n(количество элемментов массива)*/
            {
                s += array[i];
            }
            Console.WriteLine($"Сумма элементов массива={s}");
        }
        //метод, нахождения максимального элеммента массива
        static void MaxArray(Task<int[]> task)
        {
            int[] array = task.Result;
            int max = array[0];
            foreach (int a in array)
            {
                if (a > max)
                    max = a;
            }
            Console.WriteLine($"Максимальное число массива={max}");
        }
    }
}
