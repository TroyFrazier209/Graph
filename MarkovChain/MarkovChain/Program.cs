using System;
using Graph;

namespace MarkovChain
{
    class Program
    {
        static void Main(string[] args)
        {
            ListMarkovChain graph = new ListMarkovChain("Diagonal.txt");
            Console.WriteLine(graph);
            double[,] moveResult = graph.move(3);
            double[] inital = { .5, .5};
            double[] result = graph.calculateProbability(inital, 3);

            for (int row = 0; row < moveResult.GetLength(1); row++) {
                for (int col = 0; col < moveResult.GetLength(1); col++)
                    Console.Write(moveResult[row,col] + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
            for(int count = 0; count < result.Length; count++)
                Console.Write(result[count] + " ");
            Console.WriteLine();
            


        }
    }
}