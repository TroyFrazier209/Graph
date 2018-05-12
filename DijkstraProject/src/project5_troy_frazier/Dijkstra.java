/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package project5_troy_frazier;

import java.nio.file.Path;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.PriorityQueue;
import java.util.Queue;
import java.util.Scanner;

/**
 *
 * @author Troy Frazier
 */
public class Dijkstra{
    /**
     * adList creates a list of lists that mimics a 2d array that is parallel to the list nodeName that holds the connection and weight of a set of vertexes
     * nodeName holds the names of every vertex which will be in a parallel format of all other lists
     * isFound marks a vertex to ensure no repeat checks of a vertex
     * dv is the distance from a target vertex
     * pv is the previous vertex which will chain to a starting vertex
     */
    private ArrayList<ArrayList<Node>>adList;
    private ArrayList<String>nodeName;
    private ArrayList<Boolean>isFound;
    private ArrayList<Integer>dv;
    private ArrayList<String>pv;
    
    /**
     * @param tar path of an adjacency list in vertex connection weight ... format
     * 
     * Checks to see if the file targeted by a path exists, if not then exception is thrown.
     * Afterwards, it formats the file line by line and creating the adjacency list and creates the list of names for nodeName
     * dv,pv, abd isFound will be set to their default state
     * Default states:
     * dv = MAX INTEGER
     * pv = itself
     * isFound = false
     */
    public Dijkstra(Path tar){
        
        adList = new ArrayList();
        nodeName = new ArrayList();
        isFound = new ArrayList();
        dv = new ArrayList();
        pv = new ArrayList();
            
        try(Scanner read = new Scanner(tar)){
            while(read.hasNext()){
                ArrayList<Node>line = new ArrayList(); //new list for the adList
                String[] hold = read.nextLine().replaceAll("[\\s]{2,}", " ").split(" "); // eats all white space leaving one whitespace between each element and splitting it there
                nodeName.add(hold[0]);
                pv.add(hold[0]);
                for(int count = 1; count < hold.length; count+=2){
                    isFound.add(false);
                    dv.add(Integer.MAX_VALUE);
                    try{
                    int ele = Integer.parseInt(hold[count+1]);
                    line.add(new Node(hold[count],ele));
                    }
                    catch(Exception e){
                        System.out.println("Error weight value for " + hold[count] + " is not an int or integer convertable.");
                    }
                }
                adList.add(line);
            }   
        }
        catch(Exception e){
            System.out.println("Error file at path " + tar.toAbsolutePath() + " does not exit");
            System.exit(0);
        }
        
    }
    
    /**
     * 
     * @param start the target node where the shortest path to all nodes will start at
     * @param end the ending vertex of the path
     * Sets up the shortest path algorithm and clears the dv,pv, and isFound
     * then calls the recursive function generatePath
     */
    public void enactDijkstra(String start,String end){
        clearPath();
        int iniIndex = getNameIndex(start);
        int endIndex = getNameIndex(end);
        if(iniIndex < 0 || endIndex < 0)
            System.out.println("Starting vertex or ending vertex does not exist");
        else{
            dv.set(iniIndex, 0);
            generatePaths(iniIndex);
            getPathList();
            route(end);
        }
    }
    /**
     * Sets the parallel lists dv,pv, and isFound back to its default values
     * default value information is above Dijkstra(Path tar)
     */
    public void clearPath(){
        for(int count = 0; count < nodeName.size(); count++){
            pv.set(count, nodeName.get(count));
            dv.set(count,Integer.MAX_VALUE);
            isFound.set(count,false);
        }
    }
    /**
     * 
     * @param curNode The current node that will have its connections checked to see if there is a shorter path the previously recorded
     * Sets a vertex's isFound to true and checks all the connections. Afterwards all the connections are placed into a Min Heap and the root gets send to a new call
     * function will continue until there is no more false isFound variables
     * The minHeap is determined by a Node's weight
     */
    private void generatePaths(int curNode) {
        Comparator<Node> comparator = new NodeComparator();
        ArrayList<Node> cNodes = adList.get(curNode);
        Queue<Node>queue = new PriorityQueue(comparator);
        
        isFound.set(curNode,true);
        
        for(int count = 0; count < cNodes.size(); count++){
            int totWeight = cNodes.get(count).weight;
            int dvIndex = getNameIndex(cNodes.get(count).name);
            if(dv.get(curNode) < Integer.MAX_VALUE)
                totWeight += dv.get(curNode);
            if(totWeight < dv.get(dvIndex)){
                dv.set(dvIndex,totWeight);
                pv.set(dvIndex,nodeName.get(curNode));
            }
        }
        
        for(int count = 0; count < cNodes.size(); count++) //Min Heap creation
            queue.add(cNodes.get(count));
        
        while(!queue.isEmpty()){ //Creates the next calls. function ends when there are no more false isFound variables
            Node next = queue.remove();
            int index = getNameIndex(next.name);
            if(!isFound.get(index))
                generatePaths(getNameIndex(next.name));
        }
    }
    
    
    
    /**
     * @param tar variable that will be searched for in nodeName
     * @return index position which is parallel to dv,pv,and isFound
     */
    public int getNameIndex(String tar){
        for(int count = 0; count < nodeName.size(); count++){
            if(tar.equals(nodeName.get(count)))
                return count;
        }
        return -1; //data not found
    }
    /**
     * @param end the vertex name of the place you want to stop at
     * prints out price then begins recursive call to print connections and the names of the vertexes from point A -> B
     */
    private void route(String end){
        int endIndex = getNameIndex(end);
        System.out.println("Price = " + dv.get(endIndex));
        route(endIndex,-1);
        System.out.println();
    }
    /**
     * @param index the index of the vertex currently at
     * @param connections total connections that have been traversed
     * Starting at the end point, this function moves all the way to the starting point and then prints the path from A -> B
     */
    private void route(int index,int connections){
        int next = getNameIndex(pv.get(index));
         
        if(nodeName.get(next).equals(nodeName.get(index))){ //Ends the recursive call
            System.out.print("Number of airports between: ");
            if(connections <= 0) //Accounts for edge case of A -> A
                System.out.print("0");
            else
                System.out.print(connections);
            System.out.print("\nRoute: ");
        }
        else
           route(next,connections+1);
        
        System.out.print(nodeName.get(index));
        
        if(connections >= 0) //If the call is at the last call then omit ->
            System.out.print("->");
    }
    /**
     * Prints the list in nodeName isFound weight pv format
     * used to show all the vertexes along with their shortest path to some starting point which will be marked at dv = 0 and pv = itself
     */
    public void getPathList(){
        System.out.println("Format: Vertex  Found  MinWeight  Previous Vertex\n");
        for(int count = 0; count < nodeName.size(); count++){
            System.out.println(nodeName.get(count) + "  " + isFound.get(count) + "  " + dv.get(count) + "  " + pv.get(count) + "  ");
        }
        System.out.println("\n");
    }
    /**
     * Creates a string that shows the adlist that was obtained from some file
     * @return string in adjacency list format
     */
    @Override public String toString(){
        StringBuilder sb = new StringBuilder();
        for(int count = 0; count < nodeName.size(); count++){
            sb.append(nodeName.get(count));
            sb.append(" ");
            for(int i = 0; i < adList.get(count).size(); i++){
                sb.append(adList.get(count).get(i).name);
                sb.append(" ");
                sb.append(adList.get(count).get(i).weight);
                sb.append(" ");
            }
            sb.append("\n");
        }
            
       return sb.toString();
    }
   
    /**
     * class holds a vertex name and its weight. unweighted vertexes are 1
     * No negative weights are allowed
     */
    private class Node{
        String name;
        int weight;
        Node(String tarName,int tarWeight){
            name = tarName;
            if(tarWeight <= 0) //Ensure no negative weights
                weight = 1;
            else
                weight = tarWeight;
        }
        Node(String tarName){ //Unweighted version
            name = tarName;
            weight = 1;
        }
        @Override public String toString(){
            System.out.println(name + " " + weight);
            return "";
        }
    }
    
    /**
     * used to put a set of nodes in a heap. This is judged by a node's weight
     */
    private class NodeComparator implements Comparator<Node>{
        @Override public int compare(Node x,Node y){
            if(x.weight < y.weight)
                return -1;
            else if(x.weight > y.weight)
                return 1;
            else
                return 0;
        }
}
    
}
