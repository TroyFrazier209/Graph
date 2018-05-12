#Troy Frazier 
#CS 3340.003
# Graph Movement
# From a simple undireced/direced graph which is represented as an adjacency matrix...
# Find the possible moves that each node can make in an x amount of turns.
# Graph input is taken from Graph.txt.
# Boolean matrix multiplication is used to find the possible moves
	#Current TODO:
	#Export to file
	
#Macros

.macro notSimpleGraph #Error for a matrix not following simple graph rules
	li $v0,4
	la $a0,complexGraph
	syscall
	li $v0,10
	syscall
.end_macro
.macro fileNotFound #Error message for file error
	li $v0,4
	la $a0,notFound
	syscall
	li $v0,10
	syscall
.end_macro

.data
	length: .word 0 #holds the length of some array. Not in total bits
	totalIndex: .word 0 #holds total indexes of an array
	turns: .word 0 #holds the total amount of times to mutiply an array
	file: .asciiz "Graph.txt" #Finds Graph.txt and creates an array
	output: .asciiz "GraphOut.txt"
	fBuffer: .space 10200 #Enough space for a 100x100 array including \r\n
	tarNodeMsg: .asciiz "Enter a targeted node(0 for no target): "
	turnMsg: .asciiz "Enter total amount of moves being made: "
	complexGraph: .asciiz "Graph is not a simple graph."
	notFound: .asciiz "File: Graph.txt : Not found"
.text
main:
	lw $t0,length
	
	jal readFile #gets file
	jal getLength #Reads one line of file to find matrix length
	jal createArray #Allocated memory for matrix
	
	addi $s1,$v1,0 #Stores n array length
	move $s0,$v0 #Pointer for Dynamic Array
	sw $s1,length
	
	move $a0,$s0
	mult $s1,$s1 # squared matrix length to find total indexes in matrix. not total bit size
	mflo $a1
	sw $a1, totalIndex
	jal loadArray #creation of base array
	
	jal diaTrace #Ensures a simple graph. program ends if not simple
	
	jal createArray #creation of array to be multi
	move $a0,$v0
	lw $a1,totalIndex
	jal loadArray #creates a copy of base matrix
	move $s1,$a0
	
	jal createArray #Result array
	move $s2,$v0
	
	li $v0,4 # gets target node
	la $a0,tarNodeMsg
	syscall
	li $v0,5
	syscall
	addi $s5,$v0,0 #  stores target into s5
	
	li $v0,4 # gets total turns
	la $a0,turnMsg
	syscall
	li $v0,5
	syscall
	addi $s6,$v0,0 #  stores total turns into s6
	
	
	pathFor:
		#Finds a path
		beq $s6,0,exitPath
			move $a0,$s0 #Base Array
			move $a1,$s1 #Multi Array
			move $a2,$s2 #Result Array
			jal multiMat
		
			move $a0,$s1 
			move $a1,$s2
			jal setAr #Merges result array with multi array to prepare for next op
		
			addi $s6,$s6,-1
		
		j pathFor
	exitPath:
	beq $s5,0,skipLine
		move $a1,$s2 
		addi $a2,$s5,0
		jal disLine #Sends previous two statements to display a line of an array
	skipLine:
	move $a1,$s2
	jal disArray
	

exit:
	li $v0,10
	syscall
	
	
	#subroutines
readFile: #Read entire file with max bits of 10100 used to find a adjacency matrix.
# Returns nothing
	li   $v0, 13       # Open file
	la   $a0, file      
	li   $a1, 0        #To read
	li   $a2, 0
	syscall
	
	bge $v0,0,fileFound #If "Graph.txt" is not found.
		fileNotFound
		
fileFound:    
	move $t0, $v0 
	
	li   $v0, 14     #Read file
	move $a0, $t0      
	la   $a1, fBuffer   
	li   $a2, 10200     
	syscall 
	
	li   $v0, 16       # Close File
	move $a0, $t0      
	syscall             
	jr $ra     
getLength:
#Finds and stores file length needed for a nxn array. Returns nothing
	la $t0,fBuffer
	lb $t1,($t0)
	li $t2, 0
	
	lengthLoop:
		beq $t1,'\r',exitLengthLoop
		addi $t2,$t2,1
		addi $t0,$t0,1
		lb $t1, ($t0)
		j lengthLoop
		
	exitLengthLoop:
	sw $t2,length
	jr $ra
			
createArray: #Takes nothing, returns v0 = pointer to a nxn dynamic Array, v1 = N length of array
#Checks a line of a file to create a proper square array	
	lw $t1,length
	
	mult $t1,$t1 #nxn for square matrix
	mflo $t2 
	sll $t2,$t2,2 #after nxn, multiply by four to create a .word array of nxn
	
	li $v0,9 #Dynamic memory allocation
	move $a0,$t2
	syscall
	addi $v1,$t1,0 #Matrix length N to be returned	
	
	jr $ra
loadArray:#Takes a0 = array pointer, a1 = total indexes of array
#From a file read, a pointer scans the buffer and stores the data in a square array
	la $t0,($a0) #Pointer to array
	li $t1,0 #Counting variable
	la $t3,fBuffer
	lw $t4,length #Loads length of matrix to ensure file has correct square format
	li $t5,0#Counts length to ensure proper square
	
	loadFor:
		bge $t1,$a1,exitFor
			lb $t2,($t3)#Temp var to store a constant, will change to a file read		
			addi $t5,$t5,1
			
			bne $t2,'\r',breakSkip
				li $t5,1 #resets column counting
				addi $t3,$t3,2 #Skips \r\n
				
				lb $t2,($t3)
			breakSkip:
				ble $t5,$t4,matrixCheck
					notSimpleGraph #Error msg
					matrixCheck:
				blt $t2,'0',notSimple # if(x < 0 || x > 1) then not a simple graph
				bgt $t2,'1',notSimple
					addi $t2,$t2,-48 #Converts number from ascii to numeral 
					sw $t2,($t0)
					
					addi $t1,$t1,1 #Move pointers and couters
					addi $t0,$t0,4
					addi $t3,$t3,1
			j loadFor
		notSimple:
			notSimpleGraph	
	exitFor:
	jr $ra
disLine: #a1 = pointer to array, a2 = row to be displayed, returns nothing
#Displays a single line used for seeing the possible moves starting at one node
	addi $t0,$a1,0 #pointer to display row of array
	lw $t1,length 
	li $t2,1 #counter variable
	sll $t1,$t1,2 
	movePoint:
		bge $t2,$a2,exitMovePoint
			add $t0,$t0,$t1
			addi $t2,$t2,1
			j movePoint
	exitMovePoint:
	li $t2,4
	disLoop:
		bgt $t2,$t1,exitDisLoop
			li $v0,1 #Display int
			lw $a0,($t0)
			syscall
			
			addi $t0,$t0,4 #move pointers
			addi $t2,$t2,4
		j disLoop
	exitDisLoop:
	
	li $v0,11 #Creates two break lines for formatting
	li $a0,'\n'
	syscall
	li $v0,11
	li $a0,'\n'
	syscall #End format
	
	jr $ra
disArray:#Takes a1 = pointer to array, returns nothing
#Displays all nodes possible moves for some turn
	addi $sp,$sp,-4 #push for $a1
	sw $a1,($sp)
	
	lw $t0,length #Holds column length
	li $t1,0 #counts current column position
	li $t2,0 #couts total rows passed
	
	disWhile:
		li $v0,1 #Display int
		lw $a0,($a1)
		syscall
		
		addi $t1,$t1,1 #moves one column
		addi $a1,$a1,4 #moves to next index
		
		bne $t1,$t0,disElse #If true then at last index of row, move to next row
			li $v0, 11#Display char break line
			li $a0,'\n'
			syscall
			
			addi $t2,$t2,1 #moves row
			li $t1,0 #resets current columns position
		disElse:
		bge $t2,$t0,exitDis #Ends at end of matrix
		j disWhile
	exitDis:
	lw $a1,($sp) #pop
	addi $sp,$sp,4
	jr $ra	
diaTrace: #Ensure diagoanl trace is all 0s to obey rule of simple graph $a0 = base matrix to be checked
	addi $sp,$sp -4 #push for base array
	sw $a0,($sp)
	lw $t0,length  #holds length of matrix then scales it to account for 4 bit words
	sll $t0,$t0,2
	li $t1,0 #counter
	traceCheck:
		beq $t1,$t0,exitTrace
			lw $t2,($a0) #checks every diagonal
			beq $t2,0,conTrace
				notSimpleGraph
			conTrace:
				add $a0,$a0,$t0 #shifts a row
				addi $a0,$a0,4 #moves to correct diagonal
				addi $t1,$t1,4
		j traceCheck
	exitTrace:
	
	lw $a0,($sp) #pop
	addi $sp,$sp,4
	jr $ra
multiMat: # $a0 = base matrix, $a1 = multipied matrix, $a2 = resulting matrix
#Boolean Matrix multiplication shows all possible moves a node can make on its next turn
	addi $sp,$sp,-4 #push for $a0
	sw $a0,($sp)
	
	lw $t0,length
	sll $t0,$t0,2 #holds total bits of a length
	li $t1,0 #counts columns
	li $t2,0 #counts rows
	addi $t3,$a0,0 #Will move around matrix 1
	addi $t4,$a1,0 #will move around matrix 2
	addi $t9,$a2,0 #moves around matrix 3
	li $t7,0 # holds the equality between t5 and t6 which will be places into the result array
	lw $t8,totalIndex #The limit of how many loops occur
	
	rowFor:
		li $t7,0 #sets index equality back to 0 to prepare for next math op
		math:
			lw $t5,($t3) #holds the value of an element at matrix 1
			lw $t6,($t4) #holds the balue of an element at matrix 2
			
			addi $t1,$t1,4 #counts total times of math operation
			
			beq $t5,0,zeroEle #if a 1 exists then end early since boolean math
				beq $t6,0,zeroEle
					j isOne
				zeroEle:
					addi $t3,$t3,4 #Move to next Column
					add $t4,$t4,$t0 #move to next row
				bne $t1,$t0,math #loop until a full row col multiplication is applied
					sw $t7,($t9) #If program reaches here then $t7 is 0 add that to result array
				j nextIndex
			isOne:
				li $t7,1
				sw $t7,($t9)
			nextIndex:
				li $t1,0 #reset counter
				
				addi $t9,$t9,4 #move to next element
				addi $t2,$t2,4 #counts total indexs passed if equal to totalIndex then end matrix multi
				addi $t3,$a0,0 #set t3 to start of modified base array
				addi $t4,$a1,0 #set t4 to beginning of multi array
				
				bne $t2,$t0,nextEle #if false then a full row multi occured
					add $a0,$a0,$t0 #Remove a row from base array
					addi $t3,$a0,0 #set t3 to start of modified base array
					
					li $t2,0
					j endCheck
				nextEle:
					add $t4,$t4,$t2 #move t4 to the next column
				endCheck:
				addi $t8,$t8,-1
				beq $t8,0,endMulti
					j rowFor
	endMulti:
	 
	lw $a0,($sp) #pop
	addi $sp,$sp,4	
	jr $ra
setAr: # $a0 = Array that will be equal to, $a1 = target array
#Sets an array equal to another array
	addi $sp,$sp,-8 #Push for a0,a1
	sw $a0,($sp)
	sw $a1,4($sp)
	
	lw $t0,totalIndex
	mergeLoop:
		lw $t1,($a1) #transfer target element to new array
		sw $t1,($a0)
		 
		addi $a0,$a0,4 #Increament pointers to next element
		addi $a1,$a1,4
		addi $t0,$t0,-1 #counter
		beq $t0,0,exitMerge
		j mergeLoop
	exitMerge:
	
	lw $a0,($sp) #Pop of a0,a1
	lw $a1,4($sp)
	addi $sp,$sp,8
	
	jr $ra
