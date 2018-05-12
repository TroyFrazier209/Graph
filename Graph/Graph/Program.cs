using System;
using System.Collections.Generic;
namespace Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            ListGraph ports = new ListGraph("test.txt");
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

        }
    }
}