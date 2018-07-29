/*
 * Author Troy Frazier
 * Last Updated: 5/14/2018
 * Class: Extends ListGraph and applies Markov chain applications on it.
 */
using System;
using System.Collections.Generic;
using System.Text;
using Graph;

namespace MarkovChain {
    /* Note this class is immutable as all adding/removing methods do nothing
     * This was a design decision to lower the amount of checks required to adhere to the rules of Markov Chains*/
    class ListMarkovChain : ListGraph {
        
        private double[,] matrixList;
        private bool probabilityFlag = true; //Controls whether or not it is a probability Markov or an Unweighted Markov
        
        /// <summary>
        /// Calls the ListGraph method and creates a generic graph and then checks to ensure it is
        /// upholding the rules of Markov chains.
        /// </summary>
        /// <param name="path">path of the txt file that holds the graph in adjacency list format</param>
        public ListMarkovChain(string path) : base(path) {
            for (int count = 0; count < nodeName.Count; count++) {
                List<Node> line = adList[count];
                for (int i = 0; i < line.Count; i++) {
                    if (line[i].weight < 0 || line[i].weight > 1) {
                        throw new ArgumentOutOfRangeException("Weight: " + line[i].weight + " is not " +
                            "between bounds of 0 < x < 1. Markov Chain cannot be applied...");
                    }
                }
            }
            matrixList = toArray();

            double rowSum = 0;
            /*Ensures matrix fits the rules of Markov Chain */
            for (int col = 0; col < matrixList.GetLength(1); col++) {
                for (int row = 0; row < matrixList.GetLength(1); row++) {
                    if (probabilityFlag && matrixList[col, row] < 1)
                        rowSum += matrixList[col, row];
                    else if (matrixList[col, row] == 1) {
                        probabilityFlag = false;
                        rowSum = 1;
                    }
                    else {
                        throw new ArgumentOutOfRangeException("Data does not fit the format of a Markov Chain." +
                            "Row data must sum up to 1 or be in unweighted boolean format");
                    }
                }
                if(rowSum != 1 && probabilityFlag)
                    throw new ArgumentOutOfRangeException("Row data must sum up to 1 or be in unweighted boolean format");
                rowSum = 0;
            }
        }

        /// <summary>
        /// Calculates the probability of moving to each index in the matrix. used to calculate 
        /// probability of moving to a node after an X amount of moves
        /// Formula: P^n * p = p^(n+1)
        /// </summary>
        /// <param name="turns">total move made in the graph</param>
        /// <returns>the probability of each index after an X amount of moves.</returns>
        public double[,] move(int turns) {
            double[,] next = new double[matrixList.GetLength(1), matrixList.GetLength(1)]; //p^n
            double[,] result = new double[matrixList.GetLength(1), matrixList.GetLength(1)]; // p^(n+1)

            /* Copies matrixList onto next and sets result to 0*/
            for (int row = 0; row < matrixList.GetLength(1); row++) 
                for (int col = 0; col < matrixList.GetLength(1); col++) {
                    next[row, col] = matrixList[row, col];
                    result[row, col] = 0;
                }

            /* Move Calculation. Uses P^N*P=P^(N+1) */
            for (int count = 0; count < turns; count++) {
                for (int row = 0; row < matrixList.GetLength(1); row++)
                    for (int col = 0; col < matrixList.GetLength(1); col++)
                        result[row, col] = rowColMultiplication(row, col, next, matrixList);

                /*sets next equal to result to prepare for next calculation or to return */
                for (int col = 0; col < next.GetLength(1); col++)
                    for (int row = 0; row < next.GetLength(1); row++)
                        next[col, row] = result[col, row];
            }
            return next;
        }
        /// <summary>
        /// May remove
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="leftMatrix"></param>
        /// <param name="rightMatrix"></param>
        /// <returns></returns>
        private double rowColMultiplication(int row,int col, double[,] leftMatrix, double[,] rightMatrix) {
            double result = 0;
           
                for (int count = 0; count < leftMatrix.GetLength(1) && result < 1; count++)                   
                    result += leftMatrix[row, count] * rightMatrix[count, col];              
            return result;
        }
        /// <summary>
        /// Calculates the probability of ending at a node given some inital probability of starting at 
        /// each node
        /// </summary>
        /// <param name="pStart">Inital probabilities of starting at a particular node</param>
        /// <param name="moves">amount of moves made in the graph before calculating</param>
        /// <returns>the probability of ending at a node</returns>
        public double[] calculateProbability(double[] pStart, int moves) {
            if (!probabilityFlag)
                throw new Exception("Markov chain is an unweighted Markov chain. Cannot calculate probability...");

            double sum = 0;
            double[] result = new double[pStart.Length];
            double[,] movement = matrixList;

          /*Makes movements in the graph before calculating */ 
            if (moves > 0)
                movement = move(moves);

            /* ensures matrix is of correct dimensions*/
            if (pStart.Length != matrixList.GetLength(1)) {
                throw new ArgumentOutOfRangeException("Inital array does not fit the dimensions of the graph."
                    + pStart.Length + " != " + matrixList.GetLength(1));
            }

            /*Ensures rows of inital start = 100% */
            for (int count = 0; count < pStart.Length; count++) 
                sum += pStart[count];
            if (sum != 1)
                throw new ArgumentOutOfRangeException("Inital array does not sum to a probability of 100%");

            /* Sets all indexes of result to 0*/
             for (int count = 0; count < result.Length; count++) 
                 result[count] = 0;
             /* Matrix Multiplication */
            sum = 0;
            for (int row = 0; row < pStart.Length; row++) {
                for (int col = 0; col < pStart.Length; col++) 
                    sum += pStart[col] * movement[col, row];
                result[row] = sum;
                sum = 0;
            }
            return result;
        }
        
        /*This ensures that the class is immutable */
        /// <summary>Does Nothing </summary> 
        public new void addEdge(string a, string b, int c) {/*Do Nothing */ }
        /// <summary> Does Nothing</summary>
        public new void removeEdge(string a, string b) { /*Do Nothing */}
        /// <summary> Does Nothing</summary>
        public new void addNode(string a) { /*Do Nothing */}
        /// <summary> Does Nothing</summary>
        public new void removeNode(string a) {/*Do Nothing*/ }
        /// <summary> Does Nothing</summary>
        public new void removeAllEdges(string a) {/*Do Nothing */ }
    }
}
