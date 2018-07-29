using System;
using System.Collections.Generic;
using MarkovChain;
using Bipartitism;

namespace Graph
{
    
    class Program
    {
        const string FOLDER = "Graphs/"; //Path to the txt files that contain adjacency lists

        static void Main(string[] args) {
            string userIn;
            int data = -1;

            Console.WriteLine("Enter a number to test a class...\n" +
                "1. ListGraph \n" +
                "2. MarkovChain \n" +
                "3. BipartiteGraph \n");
            try {
                userIn = Console.ReadLine();
                data = Int32.Parse(userIn);
            }
            catch (Exception) {
                Console.WriteLine("Not a valid integer. Ending Program\n");
            }

            switch (data) {
                case 1: //ListGraph
            ListGraph ports = new ListGraph(FOLDER + "airport.txt");
            Console.WriteLine("Printing List...");
            Console.WriteLine(ports + "\n"); //Inital print
            ports.addNode("LON");
            ports.addEdge("LON", "MOB", 132);
            ports.addEdge("MOB", "LON", 132);
            Console.WriteLine("Printing Depth-Search and Breath-Search...");
            ports.depthSearchPrint("LON");
            ports.breathSearchPrint("LON"); //Print of the graph with a new node
            Console.WriteLine();
            ports.removeNode("DFW");
            Console.WriteLine("DFW Removed. Printing List...\n" + ports + "\n"); //New Print with DFW removed
            ports.depthSearchPrint("MOB"); //Follow two calls show that the list split into multiple graphs
            ports.breathSearchPrint("MOB");
            Console.WriteLine();
            ports.addEdge("MOB", "MSY", 111);
            ports.addEdge("MOB", "OKC", 222);
            ports.addEdge("MOB", "SHV", 333);
            ports.addEdge("MOB", "LIT", 444);
            ports.addEdge("MOB", "SAT", 555);
            ports.addEdge("MOB", "SFO", 777);
            Console.WriteLine("Printing Modified Graph in Depth-Seach, Breath-Search and Adj List...");
            ports.depthSearchPrint("MOB");
            ports.breathSearchPrint("MOB"); //Print of the graph reconnected into one graph
            ports.removeEdge("AUS", "SAT");
            Console.WriteLine(ports + "\n");
            /*Display nodes by traversing Array */
            for (int count = 0; count < ports.length; count++)
                Console.Write(ports[count] + " ");
            Console.WriteLine();
                    break;
                case 2:
                    ListMarkovChain graph = new ListMarkovChain(FOLDER + "Diagonal.txt");
                    Console.WriteLine(graph);
                    double[,] moveResult = graph.move(3);
                    double[] inital = { .5, .5 };
                    double[] result = graph.calculateProbability(inital, 3);

                    for (int row = 0; row < moveResult.GetLength(1); row++) {
                        for (int col = 0; col < moveResult.GetLength(1); col++)
                            Console.Write(moveResult[row, col] + " ");
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    for (int count = 0; count < result.Length; count++)
                        Console.Write(result[count] + " ");
                    Console.WriteLine();
                    break;

                case 3:
                    /* Graph that represents a square*/
                    ListGraph checkGraph = new ListGraph(FOLDER + "SquareGraph.txt");
                    Console.WriteLine("Square Graph");
                    int[] partition = Bipartitism.BipartiteGraph.getBipartition(checkGraph);
                    printPartition(partition, checkGraph);
                    if (Bipartitism.BipartiteGraph.isBipartite(checkGraph))
                        Console.WriteLine("Graph is bipartite.");
                    else
                        Console.WriteLine("Graph is not bipartite");
                    Console.WriteLine();

                    /*Graph that represents a prism */
                    checkGraph = new ListGraph(FOLDER + "Prism.txt");
                    Console.WriteLine("Prism Graph");
                    partition = Bipartitism.BipartiteGraph.getBipartition(checkGraph);

                    printPartition(partition, checkGraph);

                    if (Bipartitism.BipartiteGraph.isBipartite(checkGraph))
                        Console.WriteLine("Graph is bipartite.");
                    else
                        Console.WriteLine("Graph is not bipartite");
                    Console.WriteLine();

                    /*Graph that represents a cube */
                    checkGraph = new ListGraph(FOLDER + "Cube.txt");
                    Console.WriteLine("Cube Graph");
                    partition = Bipartitism.BipartiteGraph.getBipartition(checkGraph);

                    printPartition(partition, checkGraph);

                    if (Bipartitism.BipartiteGraph.isBipartite(checkGraph))
                        Console.WriteLine("Graph is bipartite.");
                    else
                        Console.WriteLine("Graph is not bipartite");
                    Console.WriteLine();

                    /*Graph that represents a star */
                    checkGraph = new ListGraph(FOLDER + "Star.txt");
                    partition = Bipartitism.BipartiteGraph.getBipartition(checkGraph);

                    Console.WriteLine("Star Graph");
                    printPartition(partition, checkGraph);

                    if (Bipartitism.BipartiteGraph.isBipartite(checkGraph))
                        Console.WriteLine("Graph is bipartite.");
                    else
                        Console.WriteLine("Graph is not bipartite");
                    Console.WriteLine();

                    /*Graph that represents a directional graph that returns to start at the end point */
                    checkGraph = new ListGraph(FOLDER + "LineGraph.txt");
                    Console.WriteLine("Directional Line Graph");
                    partition = Bipartitism.BipartiteGraph.getBipartition(checkGraph);

                    printPartition(partition, checkGraph);

                    if (Bipartitism.BipartiteGraph.isBipartite(checkGraph))
                        Console.WriteLine("Graph is bipartite.");
                    else
                        Console.WriteLine("Graph is not bipartite");
                    Console.WriteLine();

                    /*Graph that represents 2 connected squares */
                    checkGraph = new ListGraph(FOLDER + "ConnectedSquares.txt");
                    Console.WriteLine("2 Connected Squares Graph");
                    partition = Bipartitism.BipartiteGraph.getBipartition(checkGraph);

                    printPartition(partition, checkGraph);

                    if (Bipartitism.BipartiteGraph.isBipartite(checkGraph))
                        Console.WriteLine("Graph is bipartite.");
                    else
                        Console.WriteLine("Graph is not bipartite");
                    Console.WriteLine();

                    /*Graph that represents Binary Tree */
                    checkGraph = new ListGraph(FOLDER + "BinaryTree.txt");
                    Console.WriteLine("Binary Tree");
                    partition = Bipartitism.BipartiteGraph.getBipartition(checkGraph);

                    printPartition(partition, checkGraph);

                    if (Bipartitism.BipartiteGraph.isBipartite(checkGraph))
                        Console.WriteLine("Graph is bipartite.");
                    else
                        Console.WriteLine("Graph is not bipartite");
                    Console.WriteLine();

                    /*Graph that represents a directional hourglass */
                    checkGraph = new ListGraph(FOLDER + "DirHourglass.txt");
                    Console.WriteLine("Directional Hourglass Graph");
                    partition = Bipartitism.BipartiteGraph.getBipartition(checkGraph);

                    printPartition(partition, checkGraph);

                    if (Bipartitism.BipartiteGraph.isBipartite(checkGraph))
                        Console.WriteLine("Graph is bipartite.");
                    else
                        Console.WriteLine("Graph is not bipartite");
                    Console.WriteLine();

                    /*Graph that represents an empty graph */
                    checkGraph = new ListGraph(FOLDER + "SingleLoop.txt");
                    Console.WriteLine("One node that loops Graph");
                    partition = Bipartitism.BipartiteGraph.getBipartition(checkGraph);

                    printPartition(partition, checkGraph);

                    if (Bipartitism.BipartiteGraph.isBipartite(checkGraph))
                        Console.WriteLine("Graph is bipartite.");
                    else
                        Console.WriteLine("Graph is not bipartite");
                    Console.WriteLine();

                    /*Graph that represents an empty graph */
                    checkGraph = new ListGraph();
                    Console.WriteLine("Empty Graph");
                    partition = Bipartitism.BipartiteGraph.getBipartition(checkGraph);

                    printPartition(partition, checkGraph);

                    if (Bipartitism.BipartiteGraph.isBipartite(checkGraph))
                        Console.WriteLine("Graph is bipartite.");
                    else
                        Console.WriteLine("Graph is not bipartite");
                    Console.WriteLine();
                    break;
            }
        }
        public static void printPartition(int[] partition, ListGraph graph) {
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