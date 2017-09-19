//Author: Steve Sanchez
//Author's email: returnofsimba@gmail.com
//Course: CPSC223N
//Assignment number: 2
//Due date: 10/1/16
//Purpose of Program: Displays a ball at the center of a graphic area
//					  that travels in a direction based on the user input
//					  (in degrees). The user is given the coordinates of 
//					  the balls center per each update it makes during travel.
//Purpose of File: Creates the window for the user interface to be shown
//Filename: AnimatedBallMain.cs
//Command to compile this file:
//mcs AnimatedBallMain.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:AnimatedBallFrame.dll -out:AnimatedBall.exe

using System;
using System.Windows.Forms;

public class AnimatedBall{
	
	public static void Main(){
		System.Console.WriteLine("Program commencing...");
		AnimatedBallFrame animationApp = new AnimatedBallFrame();
		Application.Run(animationApp);
		System.Console.WriteLine("The program has finished executing.");
   }//End of Main function
   
}//End of class AnimatedBall
