package mazegeneration;

import java.awt.Color;
import java.awt.Graphics;
import java.util.ArrayList;
import java.util.Random;
import javax.swing.JFrame;
import javax.swing.JPanel;

/**
 *
 * @author Troy Frazier
 * 
 */
public class Maze {
    private DisjointSet[][]maze;
    
    /**
     * Creates a 2D array of user specified length and sets each element as its own disjoint set with the plan of
     * converting every element in the array into one disjoint set
     * @param x how many columns the 2D matrix will have
     * @param y how many rows the 2D matrix will have
     */
    public Maze(int x, int y){
        maze = new DisjointSet[x][y];
        
        for(int count = 0; count < maze.length; count++)
            for(int i = 0; i < maze[count].length; i++)
                maze[count][i] = new DisjointSet();
        maze[0][0].left = false; //the entrance to the maze
        maze[x-1][y-1].right = false; //the exit to the maze
    }
    
    /**
     * Randomly picks a point in the array and randomly chooses its adjacent point. Afterwards, it attempts to merge the two disjoint sets together.
     * If the two points are already in a set together then the loop will randomly choose another adjacent point until its loops through the maximum adjacent points
     * the target point has. Since it is random, there is a chance that a point will be missed but that just adds more randomness to the maze which is acceptable.
     * Loop will continue until the beginning index [0][0]'s disjoint set is equal to the total elements in the array which implies that the entire array is one disjoint set
     */
    public void generateMaze(){
        Random rPoint = new Random();
        while(getSize(maze,0,0) != -1*maze.length*maze[0].length){
            int x = rPoint.nextInt(maze.length);
            int y = rPoint.nextInt(maze[0].length);
            int[] adjPoints = getAdjacentIndexes(maze,x,y); //Max adjacent points will be 4, minimum will be 2
            boolean adjFlag = true;
            /**
             * Loop Explanation:
             * From a random point obtained above, a random adjacent point will be chose and then merged
             * If the merge is successful then the adjacent point from the main point and a new adjacent point is found and the process repeats
             * If the merge is unsuccessful then the two points are already in the same set the loop ends and returns to main loop and a new point is chosen
             * Since this sub loop pushes the main loop forward, this loop does not increase run time
             */
            for(int count = 0; count < adjPoints.length && adjFlag; count+=2){ 
               int rAdjPoint = rPoint.nextInt(adjPoints.length);
               if(rAdjPoint % 2 == 0){ //implies that the random point is on the row index
                   adjFlag = union(maze,x,y,adjPoints[rAdjPoint],adjPoints[rAdjPoint+1]);
                   x = adjPoints[rAdjPoint];
                   y = adjPoints[rAdjPoint+1];
               }
               else {//implies that the random point is on the column index
                   adjFlag = union(maze,x,y,adjPoints[rAdjPoint-1],adjPoints[rAdjPoint]);
                   x = adjPoints[rAdjPoint-1];
                   y = adjPoints[rAdjPoint];
               }
               if(adjFlag)
                adjPoints = getAdjacentIndexes(maze,x,y); //New point gets new set of adjacent points
            } 
        }
    }
    /**
     * Gets the indexes of all vertical/horizontal adjacent elements in the 2D array maze. 
     * @param maze array that holds a disjoint set
     * @param x total downward position on an array
     * @param y total leftward position on an array
     * @return returns an array with even elements being the x position and odd elements being the y position in an array of array[x][y] format
     */
    private int[] getAdjacentIndexes(DisjointSet[][]maze,int x, int y){
        ArrayList<Integer> adjPoint = new ArrayList();
        if(x+1 < maze.length){ //Point below target exists
            adjPoint.add(x+1);
            adjPoint.add(y);
        }
        if(x-1 >= 0){ //Point above target exists
            adjPoint.add(x-1);
            adjPoint.add(y);
        }
        if(y+1 < maze[0].length){ //Point to the right of target exists
            adjPoint.add(x);
            adjPoint.add(y+1);
        }
        if(y-1 >= 0){ //Point to the left of target exists
            adjPoint.add(x);
            adjPoint.add(y-1);
        }
        int[] adjArray = new int[adjPoint.size()];
        for(int count = 0; count < adjArray.length; count++) //This is constant time as at most the loop will run 4 times.
            adjArray[count] = adjPoint.get(count);
        
        return adjArray;
    }
    /**
     * Recursive function that finds root then returns its size which will be (TOTAL NODES * -1) since union by size is used
     * @param maze 2D array of disjoint sets
     * @param x column target to check for size or next element
     * @param y row target to check fir the size or next element
     * @return the size of the set that the initial x,y node resides in. Will be in (TOTAL NODES * -1) format
     */
    private int getSize(DisjointSet[][] maze,int x, int y){
        int nextX = maze[x][y].set/maze[0].length;
        if(maze[x][y].set <= -1)
            return maze[x][y].set;
        else
            return getSize(maze, nextX, (maze[x][y].set-nextX*maze[0].length));
    }
    /**
     * Given two [x][y] coordinate, the function will recursively find the root and check to see if they are equal.
     * @param maze 2D array of disjoint sets
     * @param leftX [x][y] x index of the first set
     * @param leftY [x][y] y index of the first set
     * @param rightX [x][y] x index of the second set
     * @param rightY [x][y] y index of the second set
     * @return true = sets ARE in the same set. false = sets are NOT in the same set
     */
     private boolean checkSets(DisjointSet[][]maze, int leftX, int leftY, int rightX, int rightY ){
         int nextX;
        if(maze[leftX][leftY].set >= 0){
            nextX = maze[leftX][leftY].set/maze[0].length;
             return checkSets(maze, nextX,  (maze[leftX][leftY].set - nextX*maze[0].length ),rightX,rightY );
        }
        else if(maze[rightX][rightY].set >=0){
            nextX = maze[rightX][rightY].set/maze[0].length;
           return checkSets(maze,leftX,leftY, nextX,  (maze[rightX][rightY].set - nextX*maze[0].length) );
        }
        return maze[leftX][leftY].equals(maze[rightX][rightY]); //This may work, check it
     }
     /**
      * From an index, the function will recursively move upwards until loop is found and then sets loop to the variable setChange
      * @param maze 2D disjoint array
      * @param x [x][y] x index of the set
      * @param y [x][y] y index of the set
      * @param setChange the new value to set the root of the set too
      */
     private void setRoot(DisjointSet[][]maze,int x, int y,int setChange){
         int nextX = maze[x][y].set/maze[0].length;
         if(maze[x][y].set <= -1){
             maze[x][y].set = setChange;
             //System.out.println("Position (" + x + "," + y + ") changed to: " + setChange); //DEBUG CODE
         }
         else
             setRoot(maze, nextX, (maze[x][y].set-nextX*maze[0].length),setChange);
     }
     /**
      * Union by size two different sets. if the two sets are already in the same set nothing happens.
      * @param maze 2D array of disjoint sets
      * @param leftX [x][y] x index of the first set
      * @param leftY [x][y] y index of the first set
      * @param rightX [x][y] x index of the second set
      * @param rightY [x][y] y index of the second set
      * @return True = sets were successfully united, False = sets are already in the same set
      */
     private boolean union(DisjointSet[][]maze, int leftX, int leftY, int rightX, int rightY ){
        if(!checkSets(maze,leftX,leftY,rightX,rightY)){ //Case where two nodes are not in the same set
            if(leftX-1 == rightX){ //
                maze[leftX][leftY].up = false;
                maze[rightX][rightY].down = false;
            }
            else if(leftX+1 == rightX){
                maze[leftX][leftY].down = false;
                maze[rightX][rightY].up = false;
            }
            else if(leftY-1 == rightY){
                maze[leftX][leftY].left = false;
                maze[rightX][rightY].right = false;
            }
            else if(leftY+1 == rightY){
                maze[leftX][leftY].right = false;
                maze[rightX][rightY].left = false;
            }
            int leftSize = getSize(maze,leftX,leftY);
            int rightSize = getSize(maze,rightX,rightY);
            
            if(leftSize <= rightSize){ //Merge right with left; left becomes root
                setRoot(maze,leftX,leftY,leftSize+rightSize);
                setRoot(maze,rightX,rightY, (leftX*maze[0].length)+leftY  );
            }
            else{ //Merge left with right; right becomes root
                setRoot(maze,rightX,rightY,rightSize+leftSize);
                setRoot(maze,leftX,leftY, (rightX*maze[0].length)+rightY  );
            }
                
            
            return true; 
        }
        else
            return false;
    }
    /**
     * @return a string of all the disjoint sets in the 2D array of maze with the correct set of walls.
     */
    @Override public String toString(){
        StringBuilder sb = new StringBuilder();
        
        sb.append("_");
        for(int count = 0; count < maze[0].length; count++)
            if(maze[0][count].up)
                sb.append("__");
        sb.append("\n");
        for(int count = 0; count < maze.length; count++){
            for(int subCount = 0; subCount < maze[count].length; subCount++){
                if(maze[count][subCount].left)
                    sb.append("|");
                else
                    sb.append("_");
                if(maze[count][subCount].down)
                    sb.append("_");
                else
                    sb.append(" ");
            }
            if(maze[count][maze[count].length-1].right)
                sb.append("|");
            else
                sb.append(" ");
            sb.append("\n");
        }
        return sb.toString();
    }
    
    /**
     * Creates a GUI in JFrame that represents a maze
     */
    public void draw(){
        JFrame frame = new JFrame("Maze");
        
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        MazeDraw mGUI = new MazeDraw();
        frame.add(mGUI);
        frame.setSize((maze[0].length+2)*mGUI.squareSize,(maze.length+2)*mGUI.squareSize);
        frame.setVisible(true);
       
    }
    /**
     * This class contains the walls and the disjoint set elements used to define a singular node in the maze array
     * This class is to be used with a union by size application
     * set: any value < 0 is the size of the set and also implies that the targeted node is a root
     * any value > 0 implies that the node is a child of some root
     * up: is the north facing wall
     * down: is the south facing wall
     * left: is the west facing wall
     * right: is the east facing wall
     * up,down,left,right: true = wall exists, false = wall does not exist
     */
    public class DisjointSet{
        protected int set = -1;
        protected boolean up = true;
        protected boolean down = true;
        protected boolean left = true;
        protected boolean right = true;
        
        public DisjointSet(){}
    }
    
    /**
     * This class draws the maze in JFrame
     * SQUARESIZE = the length of a square of a maze cell. Each cell is linked to one index in the 2D array and each cell is 625 pixels in total
     * WALLSIZE = the space created between two cells. Walls will overlap cells to create an illusion of a maze
     */
    public class MazeDraw extends JPanel{
       int squareSize = 25; //this is the x,y length of a index in the maze i.e 625 total pixels per index in 2D array
       static final int WALLSIZE = 3; // Total space that a wall overlaps a cell. NOTE if this is bigger then SQUARESIZE then the cell will flip in a symmetrical fashion
        
        /**
         * Method scales each cell in the maze
         * @param col total cells in a column
         * @param row total cells in a row
         */
        private void setSquareSize(int col,int row){
            if(col > 0 && row > 0){
                if(col <= 35 && row <= 35)
                    squareSize = 25;
                else if( col <= 50 && row <= 50)
                    squareSize = 15;
                else
                    squareSize = 5;
                    
            }
        }
        /**
         * Method that draws the maze
         * @param g 
         */
        @Override public void paintComponent(Graphics g){
            super.paintComponent(g);
            this.setBackground(Color.BLACK); //"Walls" will be black
            int mazeXPoint = 0;
            int mazeYPoint = 0;
            
            setSquareSize(maze[0].length,maze.length);
            g.setColor(Color.GREEN); //Starting point
            g.fillRect(WALLSIZE, WALLSIZE, squareSize-WALLSIZE, squareSize-WALLSIZE);
            g.setColor(Color.WHITE); //"Empty Space" will be white
            
            for(int row = 0; row < maze.length; row++){ //Loop draws the maze accounting for walls
                for(int count = 0; count < maze[0].length; count++){
                    if(row == 0 && count == 0)
                        count++;
                    if(count < maze[0].length){ //Handles Edge case if single column matrix
                        if(maze[row][count].left)
                            mazeXPoint = WALLSIZE;
                        if(maze[row][count].up)
                            mazeYPoint = WALLSIZE;
                        g.fillRect(count*squareSize+mazeXPoint,row*squareSize+mazeYPoint,squareSize-mazeXPoint, squareSize-mazeYPoint);
                    }
                    mazeYPoint = 0;
                    mazeXPoint = 0;
                }
            }
        
            g.setColor(Color.YELLOW); //End of Maze
            g.fillRect(maze[0].length*squareSize,(maze.length-1)*squareSize,squareSize,squareSize);
        }
    }
}
