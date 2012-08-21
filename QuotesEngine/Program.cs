using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Swinton.QuotesEngine.Interface;
using Ninject;

namespace Swinton.QuotesEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                IKernel kernel = new StandardKernel(new QuoteEngineModule());
                
                IRunnable engine = kernel.Get<IRunnable>();
                engine.Run();

                //BeginAggregate();

                //Aggregate();
                AggregateSum();
                //AggregateWithResult();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while running the Quotes Engine: {0}", e.Message);
            }
        }

        private static void BeginAggregate()
        {
            int[] nos = new int[] { 2, 3, 4, 6, 7, 8 };

            var evenNos = nos.Count(n => n % 2 == 0);

            Console.WriteLine(3 / 2);
            Console.WriteLine(3 % 2);
            Console.WriteLine(evenNos);
        }

        private static void Aggregate()
        {
            string sentence = "the quick brown fox jumps over the lazy dog";

            // Split the string into individual words.
            string[] words = sentence.Split(' ');

            string reversed = words.Aggregate((currentSentence, next) => next + " " + currentSentence);

            Console.WriteLine(reversed);
        }

        private static void AggregateSum()
        {
            int[] nos = new int[] { 2, 3, 4, 6, 7, 8 };
            int sum = nos.Aggregate(0, (total, next) => total += next);

            Console.WriteLine("Aggregate Sum of the integers is {0}", sum);
            Console.WriteLine("Sum of the integers is {0}", nos.Sum());
        }

        private static void AggregateWithResult()
        {
            string[] fruits = { "apple", "mango", "orange", "passionfruit", "grape" };

            string longestString = fruits.Aggregate("banana",
                (currentFruit, next) => currentFruit.Length > next.Length ? currentFruit : next,
                (fruit) => fruit.ToUpper());

            Console.WriteLine("Fruits containing \"o\"");
            foreach (string fruit in fruits.Where(fruit => fruit.Contains('o')))
                Console.WriteLine(fruit);

            Console.WriteLine("Longest string: {0}", longestString);
        }
    }

}
