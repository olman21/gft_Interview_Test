# gft_Interview_Test
Programming Exercise Shape Manager
Please write a console application with the following behaviour.

1. When the user enters the name of a shape followed by the corresponding number of numeric parameters, define that shape and keep it in memory.  The numbers may be of type double.  Examples:

circle 1.7 -5.05 6.9
square 3.55 4.1 2.77
rectangle 3.5 2.0 5.6 7.2
triangle 4.5 1 -2.5 -33 23 0.3
donut 4.5 7.8 1.5 1.8

•	For the circle, the numbers are the x and y coordinates of the centre followed by the radius.
•	For the square it is x and y of one corner followed by the length of the side.
•	For the rectangle it is x and y of one corner followed by the two sides.
•	For the triangle it is the x and y coordinates of the three vertices (six numbers in total).
•	For the donut it is the x and y of the centre followed by the two radiuses.

In addition, every time such a line is entered, the application should give it a unique identifier and print it out in a standardised form, for example:

=> shape 1: circle with centre at (1.7, -5.05) and radius 6.9

2. When the user enters a pair of numbers, the application should print out all the shapes that include that point in the (x, y) space, i.e. it should print out shape X if the given point is inside X.  (A point is inside a donut shape if it is inside the outer circle but not inside the inner one.)

It should also print out the surface area of each shape found, and the total area of all the shapes returned for a given point.

3. It should accept the commands “help” for printing instructions and “exit” for terminating the execution.

4. If the user enters anything unexpected (including errors like too few/many arguments, incorrect number format, etc.), it should print a meaningful error message and continue the execution.

5. Feel free to add additional shapes (e.g. ellipsis) or operations (e.g. to delete a given shape).  An advanced option could be to find all the shapes that overlap one that’s named by the user.

6. Allow input from a file as well as the console.  It should be possible, for example, to read a file with shape definitions and then to continue with an interactive session.

7. Think about implementing it in a way which would perform well even for a very large number shapes (e.g., tens of millions, but assuming it can still be held in the program memory).

8. Please provide a sample input file for testing.
