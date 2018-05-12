package mazegeneration;

import java.util.Scanner;

/**
 *
 * @author Troy Frazier
 * This Class creates a maze with all cells have one path to it from the start of the maze.
 * Uses Disjoint sets with union by size to create the maze
 * Has two outputs. Console Output and JFrame output
 * toString() is the console output and
 *
 */
public class MazeGeneration {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        Maze maze;
        Scanner in = new Scanner(System.in);
        boolean userFlag = false;
        int[] parsedInput = new int[2];
        
        /*  Gets input with validation */
        /* Command line args*/
        if(args.length == 2){
                try{
                    parsedInput[0] = Integer.parseInt(args[0]);
                    parsedInput[1] = Integer.parseInt(args[1]);
                    userFlag = true;
                }
                catch(Exception e){
                    System.out.println("Error: args values was not a set of integers integers");
                }
            }
        /* User input */
        while(!userFlag){
            System.out.println("Enter the width and height of the maze with a space between the two(Example, 25 25): ");
            String[] userIn = in.nextLine().split(" ");
            if(userIn.length != 2)
                System.out.println("Incorrect format");
            else{
                try{
                    parsedInput[0] = Integer.parseInt(userIn[0]);
                    parsedInput[1] = Integer.parseInt(userIn[1]);
                    userFlag = true;
                }
                catch(Exception e){
                    System.out.println("Error: input was not a set of integers");
                }
            }
        }
        
        /* Maze generation begins here  */
        maze = new Maze(parsedInput[0],parsedInput[1]);  
        maze.generateMaze();
        System.out.println(maze + "\n\n"); //Prints console form of maze
        maze.draw(); //Prints a GUI form of maze
        
      
    }
    
}
