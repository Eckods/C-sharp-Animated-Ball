//Author: Steve Sanchez
//Author's email: returnofsimba@gmail.com
//Course: CPSC223N
//Assignment number: 2
//Due date: 10/1/16
//Purpose of Program: Displays a ball at the center of a graphic area
//					  that travels in a direction based on the user input
//					  (in degrees). The user is given the coordinates of 
//					  the balls center per each update it makes during travel.
//Purpose of File: Calculates the movement of the ball in the graphic area
//				   as well as re-centers the ball
//Filename: AnimatedBallLogic.cs
//Command to compile this file:
//mcs -target:library AnimatedBallLogic.cs -r:System.Drawing.dll -out:AnimatedBallLogic.dll
using System;

public class AnimatedBallLogic{
	private double radians;
	private double xDelta;
	private double yDelta;
	private double ballDoubleCoordX;
	private double ballDoubleCoordY;
	private const double distTraveledPerRefresh = 200;
	private const double ballRefreshRate = 30.0; //Units are Hz
	private int radius;
	private int graphicWidth;
	private int graphicHeight;
	private int graphicTop;

	public AnimatedBallLogic(int ballRadius, int frameWidth, int graphTopLocation, int graphHeight){
		//Sets variables needed for calculations for initial coordinates (centered ball values)
		radius = ballRadius;
		graphicWidth = frameWidth;
		graphicTop = graphTopLocation;
		graphicHeight = graphHeight;
		
	}//End of constructor
	
	public void setRadians(double ballRadians){
		//Sets the radians of the ball travel to the value input by user
		radians = ballRadians;
		
	}//End of method setRadians
	
	public double getBallRefreshRate(){
		//Returns ball refresh rate
		return ballRefreshRate;
		
	}//End of method getBallRefreshRate
	
	public double getUpdatedDoubleCoordX(){
		//Calculates vertical displacement of ball
		xDelta = (distTraveledPerRefresh/ ballRefreshRate) * System.Math.Cos(radians);
		
		//Sets x-coordinate to new coordinate where ball traveled and returns value
		ballDoubleCoordX = ballDoubleCoordX + xDelta;
		return ballDoubleCoordX;
		
	}//End of method getUpdatedDoubleCoordX
	
	public double getUpdatedDoubleCoordY(){
		//Calculates vertical displacement of ball
		yDelta = (distTraveledPerRefresh /  ballRefreshRate) * System.Math.Sin(radians);
		
		//In the next statement the minus sign is used because the y-axis is upside down relative to the standard cartesian
		//coordinate system. Sets y-coordinate to new coordinate where ball traveled and returns value
		ballDoubleCoordY = ballDoubleCoordY - yDelta;
		return ballDoubleCoordY;
		
	}//End of method getUpdatedDoubleCoordY
	
	public double getInitialDoubleCoordX(){
		//Sets x-coordinate to its centered value and returns value
		ballDoubleCoordX = (double)(graphicWidth/2 - radius*1.25);
		return ballDoubleCoordX;
		
	}//End of method getInitialDoubleCoordX
	
	public double getInitialDoubleCoordY(){
		//Sets y-coordinate to its centered value and returns value
		ballDoubleCoordY = (double)(graphicTop + graphicHeight/2 - radius);
		return ballDoubleCoordY;
		
	}//End of method getInitialDoubleCoordY
	
}//End of class AnimatedBallLogic
