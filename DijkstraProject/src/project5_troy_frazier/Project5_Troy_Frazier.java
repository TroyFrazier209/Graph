/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package project5_troy_frazier;

import java.nio.file.FileSystems;
import java.nio.file.Path;
import java.util.Scanner;

/**
 *
 * @author Troy Frazier
 */
public class Project5_Troy_Frazier {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        Path tar = FileSystems.getDefault().getPath("airport.txt"); //adjacency list
        Dijkstra graph = new Dijkstra(tar);
        Scanner read = new Scanner(System.in);
        String userIn;
        System.out.println(graph); //prints adjacency list
        
        do{
        System.out.print("Enter the starting point's name: "); //Gets starting point
        String start = read.next();
        System.out.print("Enter the ending point's name: "); //Gets ending point
        String end = read.next();
        graph.enactDijkstra(start,end); //Shortest path begins here
        System.out.print("Check another route? Y/N: "); // Repeat or not
        userIn = read.next();
        }while(userIn.equals("y") || userIn.equals("Y")); //All inputs besides y,Y will end loop
        
    }
    
}
