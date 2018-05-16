using System;
using Graph;
namespace BipartiteGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Graph that represents a square*/
            ListGraph graph = new ListGraph("Graphs/SquareGraph.txt");
            Console.WriteLine("Square Graph");
            int[] partition = BipartiteGraph.getBipartition(graph);

            printPartition(partition, graph);

            if (BipartiteGraph.isBipartite(graph))
                Console.WriteLine("Graph is bipartite.");
            else
                Console.WriteLine("Graph is not bipartite");
            Console.WriteLine();

            /*Graph that represents a prism */
            graph = new ListGraph("Graphs/Prism.txt");
            Console.WriteLine("Prism Graph");
            partition = BipartiteGraph.getBipartition(graph);
           
            printPartition(partition, graph);

            if (BipartiteGraph.isBipartite(graph))
                Console.WriteLine("Graph is bipartite.");
            else
                Console.WriteLine("Graph is not bipartite");
            Console.WriteLine();

            /*Graph that represents a cube */
            graph = new ListGraph("Graphs/Cube.txt");
            Console.WriteLine("Cube Graph");
            partition = BipartiteGraph.getBipartition(graph);
            
            printPartition(partition, graph);

            if (BipartiteGraph.isBipartite(graph))
                Console.WriteLine("Graph is bipartite.");
            else
                Console.WriteLine("Graph is not bipartite");
            Console.WriteLine();

            /*Graph that represents a star */
            graph = new ListGraph("Graphs/Star.txt");
            partition = BipartiteGraph.getBipartition(graph);

            Console.WriteLine("Star Graph");
            printPartition(partition, graph);

            if (BipartiteGraph.isBipartite(graph))
                Console.WriteLine("Graph is bipartite.");
            else
                Console.WriteLine("Graph is not bipartite");
            Console.WriteLine();

            /*Graph that represents a directional graph that returns to start at the end point */
            graph = new ListGraph("Graphs/LineGraph.txt");
            Console.WriteLine("Directional Line Graph");
            partition = BipartiteGraph.getBipartition(graph);

            printPartition(partition, graph);

            if (BipartiteGraph.isBipartite(graph))
                Console.WriteLine("Graph is bipartite.");
            else
                Console.WriteLine("Graph is not bipartite");
            Console.WriteLine();

            /*Graph that represents 2 connected squares */
            graph = new ListGraph("Graphs/ConnectedSquares.txt");
            Console.WriteLine("2 Connected Squares Graph");
            partition = BipartiteGraph.getBipartition(graph);

            printPartition(partition, graph);

            if (BipartiteGraph.isBipartite(graph))
                Console.WriteLine("Graph is bipartite.");
            else
                Console.WriteLine("Graph is not bipartite");
            Console.WriteLine();

            /*Graph that represents Binary Tree */
            graph = new ListGraph("Graphs/BinaryTree.txt");
            Console.WriteLine("Binary Tree");
            partition = BipartiteGraph.getBipartition(graph);

            printPartition(partition, graph);

            if (BipartiteGraph.isBipartite(graph))
                Console.WriteLine("Graph is bipartite.");
            else
                Console.WriteLine("Graph is not bipartite");
            Console.WriteLine();

            /*Graph that represents a directional hourglass */
            graph = new ListGraph("Graphs/DirHourglass.txt");
            Console.WriteLine("Directional Hourglass Graph");
            partition = BipartiteGraph.getBipartition(graph);

            printPartition(partition, graph);

            if (BipartiteGraph.isBipartite(graph))
                Console.WriteLine("Graph is bipartite.");
            else
                Console.WriteLine("Graph is not bipartite");
            Console.WriteLine();

            /*Graph that represents an empty graph */
            graph = new ListGraph("Graphs/SingleLoop.txt");
            Console.WriteLine("One node that loops Graph");
            partition = BipartiteGraph.getBipartition(graph);

            printPartition(partition, graph);

            if (BipartiteGraph.isBipartite(graph))
                Console.WriteLine("Graph is bipartite.");
            else
                Console.WriteLine("Graph is not bipartite");
            Console.WriteLine();

            /*Graph that represents an empty graph */
            graph = new ListGraph();
            Console.WriteLine("Empty Graph");
            partition = BipartiteGraph.getBipartition(graph);

            printPartition(partition, graph);

            if (BipartiteGraph.isBipartite(graph))
                Console.WriteLine("Graph is bipartite.");
            else
                Console.WriteLine("Graph is not bipartite");
            Console.WriteLine();

        }
        public static void printPartition(int[] partition,ListGraph graph) {
            Console.Write("Partition 1: ");
            for (int count = 0; count < partition.Length; count++)
                if (partition[count] == 1)
                    Console.Write(graph[count] + " ");
            Console.Write("\nPartition 2: ");
            for (int count = 0; count < partition.Length; count++)
                if (partition[count] == 2)
                    Console.Write(graph[count] + " ");
            Console.WriteLine();
        }
    }
}