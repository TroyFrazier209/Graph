/*
 * Author Troy Frazier
 * Last Updated: 5/16/2018
 * Class: Checks a graph from the class ListGraph to see if it is Bipartite
 * Bipatite definition: two groups that are only connected to the other group and not themselves
 */
using System;
using System.Collections.Generic;
using System.Text;
using Graph;
namespace BipartiteGraph
{
    static class BipartiteGraph
    {
        /* 
         * NIL = Colorless
         * Blue = Group 1
         * Orange = Group 2
         */
        enum color { NIL = 0, BLUE, ORANGE};

        /// <summary>
        /// Checks graph to see if it is Bipartite and creates two seperate groups in an array with the integer 
        /// values of 1 = Group 1 and 2 = Group 2. If not bipartite then entire array will be in group 1
        /// </summary>
        /// <param name="graph">Graph to partition</param>
        /// <returns>int[] that is parallel to graph[] which links those nodes into two groups of 1 or 2</returns>
        public static int[] getBipartition(ListGraph graph) {
            color[] colorFlags = new color[graph.length];
            int[] partition = new int[graph.length];
            /* Sets all nodes to colorless and sets the entire partition to 1 */
            for (int count = 0; count < colorFlags.Length; count++) {
                colorFlags[count] = color.NIL;
                partition[count] = 1;
            }

            if (!isBipartite(graph, colorFlags))  //Not bipartite
                Console.WriteLine("Graph is not bipartite, Grouping all nodes.");
            else  //Bipartite, setting all nodes to their respective groups
                for(int count = 0; count < colorFlags.Length; count++) 
                    partition[count] = (colorFlags[count] == color.BLUE) ? 1 : 2;
 
            return partition;
        }
        /// <summary>
        /// Checks to see if graph is bipartite. That is, if the graph can be seperated into two groups of connections. 
        /// Checks in a breadth-first search style
        /// </summary>
        /// <param name="graph">Graph to check if it is bipartite</param>
        /// <returns>True = is bipartite. False = not bipartite</returns>
        public static bool isBipartite(ListGraph graph) {
            if (graph.length <= 0) //Edge case of empty graph
                return true;
            color[] colorFlags = new color[graph.length];
            Queue<int> bQueue = new Queue<int>();
            /* sets all nodes to colorless*/
            for (int count = 0; count < colorFlags.Length; count++)
                colorFlags[count] = color.NIL;
            bQueue.Enqueue(0);
            colorFlags[0] = color.BLUE;
            return isBipartite(graph, colorFlags, bQueue);
        }
        /// <summary>
        /// Used in getPartition to check to see if graph is bipartite and returns an array with set colors
        /// </summary>
        /// <param name="graph">Graph to check to see if it is bipartite</param>
        /// <param name="colorFlags">nodes to be colored</param>
        /// <returns>colored nodes that prove if graph is bipartite or not</returns>
        private static bool isBipartite(ListGraph graph,color[] colorFlags) {
            if (graph.length <= 0) //Edge case of empty graph
                return true;
            Queue<int> bQueue = new Queue<int>();
            for (int count = 0; count < colorFlags.Length; count++)
                colorFlags[count] = color.NIL;
            bQueue.Enqueue(0);
            colorFlags[0] = color.BLUE;
            return isBipartite(graph, colorFlags, bQueue);
        }
        /// <summary>
        /// Recursive function that colors all connections of a node and then colors all the connection's connections
        /// If two connected nodes have the same color, then graph is not bipartite
        /// </summary>
        /// <param name="graph">graph to check if it is bipartite</param>
        /// <param name="colorFlags">nodes that will be colored</param>
        /// <param name="bQueue">keeps track of breadth-first search algorithim</param>
        /// <param name="colorTurn"></param>
        /// <returns>True = Bipartite. False = Not Bipartite</returns>
        private static bool isBipartite(ListGraph graph,color[] colorFlags, Queue<int> bQueue) {
            int index = bQueue.Dequeue();
            int[] connections = graph.getConnections(graph[index]);
            /* Colors and checks colors */
            for(int count = 0; count < connections.Length; count++) {
                if (colorFlags[connections[count]] == color.NIL) {
                    colorFlags[connections[count]] = (colorFlags[index] == color.ORANGE) ? color.BLUE : color.ORANGE;
                    bQueue.Enqueue(connections[count]);
                }
                else if(colorFlags[index] == colorFlags[connections[count]]) //Two nodes are the same color. Not bipartite
                    return false;
            }
            if (bQueue.Count > 0) //loops until all nodes have been checked
                return isBipartite(graph, colorFlags, bQueue);

            return true;
        }

    }
}
