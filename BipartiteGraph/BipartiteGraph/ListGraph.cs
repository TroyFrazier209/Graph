/*
 * Author Troy Frazier
 * Last Updated: 5/15/2018
 * Class: Creates an adjacency list representation of a graph
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Graph
{
    
    class ListGraph
    {
        /// <summary>
        /// nodeName contains all the names of the main vertexes that are parrallel to adList. Dictionary is used
        /// to create a constant lookup for searching a particular node
        /// adList contains all the connections of a particular node that is defined in nodeName
        /// </summary>
        
        protected Dictionary<string, int> nodeName = new Dictionary<string, int>();
        protected List<List<Node>> adList = new List<List<Node>>();
        protected int _length = 0;
        public int length {
            get { return _length; }
        }

        /// <summary>
        /// Constructor for a file path contained by a string
        /// Checks the file and opens.
        /// Must be in Adjacency List format I.E A B 15 C 23 D 15
        /// </summary>
        /// <param name="path"> file path to Adjacency List file in a txt format</param>
        public ListGraph(string path) {
            try {
                StreamReader read = File.OpenText(path);
                for (int i = 0; !read.EndOfStream; i++) {
                    string line = read.ReadLine();
                    Regex rgx = new Regex(@"[\s]+");
                    string[] nodes = rgx.Split(line);
                    nodeName.Add(nodes[0],i);
                    _length++;
                    List<Node> connections = new List<Node>();
                    for (int count = 1; count < nodes.Length - 1; count += 2) {
                        try {
                            double x = Double.Parse(nodes[count + 1]);
                            connections.Add(new Node(nodes[0],nodes[count], x));
                        }
                        catch (Exception) {
                            throw new InvalidCastException("Not a convertable Integer. " +
                                "Ensure file is in name weight format (A 50)");
                        }
                    }
                    adList.Add(connections);
                }
            }
            catch (Exception) {
                throw new FileNotFoundException("File does not exist at " + Directory.GetCurrentDirectory()
                    + " Ensure file exists and is a .txt file");
            }
        }
        /// <summary>
        /// Default constructor that creates an empty graph
        /// </summary>
        public ListGraph() {
            nodeName = new Dictionary<string, int>();
            adList = new List<List<Node>>();
        }
        /// <summary>
        /// Turns adList into a 2d double array for the intentions of performing mathematical operations on it.
        /// Will still be parrallel to nodeName
        /// </summary>
        /// <returns>A 2D array representation of adList.</returns>
        public double[,] toArray() {
           
            double[,] list = new double[nodeName.Count,nodeName.Count];
            
            for (int count = 0; count < list.GetLength(1); count++)  //sets entire array to 0s
                for (int i = 0; i < list.GetLength(1); i++)
                    list[count, i] = 0;
            
            for(int count = 0; count < nodeName.Count; count++) { //puts weights at appropriate postions in array 
                List<Node> line = adList[count];
                for (int i = 0; i < line.Count; i++) {
                    int index = nodeName[line[i].name];
                    list[count, index] = line[i].weight;
                }
            }
            return list;
        }
        public int[] getConnections(string name) { //Not checked yet
            int[] connections = null;
            try {
                int index = nodeName[name];
                List<Node> line = adList[index];
                connections = new int[line.Count];
                for (int count = 0; count < line.Count; count++)
                    connections[count] = nodeName[line[count].name];
               
            }
            catch (Exception) {
                throw new KeyNotFoundException("Key not found in adjacency list");
            }
            return connections;
        }
        /// <summary>
        /// Adds a single directed edge to a vertex to an another already existing vertex
        /// </summary>
        /// <param name="node">Name of the node that will have an edge added</param>
        /// <param name="_name">Name of the new connection</param>
        /// <param name="_weight">weight of the new connection</param>
        public void addEdge(string node, string _name, int _weight) {
            if (nodeName.Count > 0) {
                try {
                    int index = nodeName[node];
                    List<Node> line = adList[index];
                    line.Add(new Node(node, _name, _weight));
                    adList[index] = line;
                }
                catch {
                    throw new KeyNotFoundException("Node does not exist, edge not added");
                }
            }
            else
                Console.WriteLine("Graph is empty");
        }
        /// <summary>
        /// Removes an edge between two already existing vertex
        /// </summary>
        /// <param name="nodeOne">First vertex connection</param>
        /// <param name="nodeTwo">Second vertex connection</param>
        public void removeEdge(string nodeOne, string nodeTwo) {
            if (nodeName.Count > 0) {
                try {
                    int index = nodeName[nodeOne];
                    List<Node> line = adList[index];
                    for (int count = 0; count < line.Count; count++) {
                        if (line[count].name.Equals(nodeTwo))
                            line.RemoveAt(count);
                    }
                    adList[index] = line;
                }
                catch {
                    throw new KeyNotFoundException("Node or edge does not exist");
                }
            }
            else
                Console.WriteLine("Graph is empty");
        }
        /// <summary>
        /// Remove all edges from a vertex
        /// </summary>
        /// <param name="node">Vertex that will have all the edges removed</param>
        public void removeAllEdges(string node) {
            if (nodeName.Count > 0) {
                try {
                    int index = nodeName[node];//nodeIndex(node);

                    List<Node> line = adList[index];
                    for (int count = line.Count - 1; count >= 0; count--) //ensures O(N)
                        line.RemoveAt(count);
                    adList[index] = line;
                }
                catch {
                    throw new KeyNotFoundException("Node does not exist");
                }
            }
            else
                Console.WriteLine("Graph is empty");

        }
        /// <summary>
        /// Adds a new node to the Adjacency List
        /// </summary>
        /// <param name="_name">Name of the new vertex</param>
        public void addNode(string _name) {
            try {
                nodeName.Add(_name, nodeName.Count);
                _length++;
                adList.Add(new List<Node>());
            }
            catch(Exception) {
                Console.WriteLine("Node already exists. No Node was added");
            }
        }
        /// <summary>
        /// Removes alls the edges of a vertex and then the vertex itself
        /// </summary>
        /// <param name="_name">Vertex to be removed</param>
        public void removeNode(string _name) {
            if (nodeName.Count > 0) {
                try {
                    removeAllEdges(_name);
                    adList.Remove(adList[nodeName[_name]]);
                    nodeName.Remove(_name);
                    _length--;

                    Dictionary<string, int>.KeyCollection keys = nodeName.Keys;
                    string[] names = new string[nodeName.Count];
                    keys.CopyTo(names, 0);

                    for(int count = 0; count < nodeName.Count; count++)  //adjusts new index levels of nodeNames
                        nodeName[names[count]] = count;

                    for(int count = 0; count < nodeName.Count; count++) {
                        List<Node> line = adList[count];
                        //Console.WriteLine(line[0].parent);
                        for (int i = 0; i < line.Count; i++) {
                            //Console.WriteLine(line[i].name);
                            if (line[i].name.Equals(_name)) {
                                removeEdge(line[i].parent, line[i].name);
                                i--;
                            }
                        }
                    }
                }
                catch {
                    throw new KeyNotFoundException("Node does not exist");
                }
            }
            else
                Console.WriteLine("Graph is empty");
        }
        /// <summary>
        /// Starting call for the recursive function. Prints each node then travels to the next node
        /// and returns if no connections are left until all nodes have been traversed
        /// </summary>
        /// <param name="start"></param>
        public void depthSearchPrint(string start) {
            if (nodeName.Count > 0) {
                try {
                    int index = nodeName[start];
                    bool[] visited = new bool[nodeName.Count];
                    for (int count = 0; count < visited.Length; count++)
                        visited[count] = false;
                    depthSearchPrint(visited, start);
                    Console.WriteLine();

                }
                catch {
                    Console.WriteLine("Node does not exist. Search not enacted");
                }
            }
            else
                Console.WriteLine("Graph is empty");

        }
        /// <summary>
        /// Recursive depth search print. starts with a user-specified node and traverses through the graph printing
        /// each node until all nodes have been printed
        /// </summary>
        /// <param name="visited">Array that is parrallel to nodeName and marks said node as visited</param>
        /// <param name="node">the node to be looked up</param>
        private void depthSearchPrint(bool[] visited,string node) { 
            Console.Write(node + " ");
            visited[nodeName[node]] = true;
            List<Node> line = adList[nodeName[node]];
            for (int count = 0; count < line.Count; count++) {
                if (!visited[nodeName[line[count].name]])
                    depthSearchPrint(visited, line[count].name);
            }
        }
        /// <summary>
        /// Starting call for the recursive function. Prints each node in a breath-search fashion
        /// </summary>
        /// <param name="start"></param>
        public void breathSearchPrint(string start) {
            try {
                Queue<string> bQueue = new Queue<string>();
                bool[] visited = new bool[nodeName.Count];
                for (int count = 0; count < visited.Length; count++)
                    visited[count] = false;
                bQueue.Enqueue(start);
                visited[nodeName[start]] = true;
                Console.Write(start + " ");
                breathSearchPrint(visited, bQueue);
                Console.WriteLine();
            }
            catch {
                Console.WriteLine("Node does not exist. Search not enacted");
            }
        }
        /// <summary>
        /// starting at the first node, all connections are added to a queue and printed. Then function
        /// calls itself and dequeues the front of the queue
        /// </summary>
        /// <param name="visited">Ensures no nodes are revisited</param>
        /// <param name="bQueue">holds the next node to be searched</param>
        private void breathSearchPrint(bool[]visited, Queue<string>bQueue) {
            string node = bQueue.Dequeue();
            List<Node>line = adList[nodeName[node]];
            for(int count = 0; count < line.Count; count++) {
                if (!visited[nodeName[line[count].name]]) {
                    Console.Write(line[count].name + " ");
                    visited[nodeName[line[count].name]] = true;
                    bQueue.Enqueue(line[count].name);
                }
            }
            if (bQueue.Count > 0)
                breathSearchPrint(visited, bQueue);
        }
        /// <summary>
        /// Prints the graph in Adjacency list form
        /// </summary>
        /// <returns>string of Adjacency list</returns>
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, int>.KeyCollection keys = nodeName.Keys;
            string[] names = new string[nodeName.Count];
            keys.CopyTo(names, 0);
            
            for(int count = 0; count < nodeName.Count; count++) {
                List<Node> line = adList[count];
                sb.Append(names[count] + " ");
                for(int i = 0; i < line.Count; i++)
                    sb.Append(line[i].name + " " + line[i].weight + "  ");
                sb.Append("\n");
            }

            return sb.ToString();
        }
        /// <summary>
        /// Finds the name of a node given the nodes index number in the adjacency list
        /// </summary>
        /// <param name="key">Index number of the node</param>
        /// <returns>Name of the node at that specific index</returns>
        public string this[int key] {
            get {
                Dictionary<string, int>.KeyCollection keys = nodeName.Keys;
                string[] names = new string[nodeName.Count];
                keys.CopyTo(names, 0);
                return names[key];
            }
        }

        internal class Node {
            /// <summary>
            /// Parent is the previous connection of this specific node. empty string implies no parent
            /// name is the name of this node
            /// weight is the cost of traveling from parent to node
            /// </summary>
            public string parent = "";
            public string name;
            public double weight;
            /// <summary>
            /// Creates a node with a given weight. 1 is determined to be unweighted. Negatives are converted to 1
            /// </summary>
            /// <param name="_parent">Name of parent connection</param>
            /// <param name="_name">name of node</param>
            /// <param name="_weight">value of weight</param>
            public Node(string _parent,string _name, double _weight) {
                parent = _parent;
                name = _name;
                if (weight <= 0)
                    weight = _weight;
                else
                    weight = 1;
            }
            /// <summary>
            /// Created an unweighted node
            /// </summary>
            /// <param name="_name">name of node</param>
            /// <param name="_parent">name of parent connection of node</param>
            public Node(string _name,string _parent) {
                name = _name;
                parent = _parent;
                weight = 1;
            }
        }
    }
}
