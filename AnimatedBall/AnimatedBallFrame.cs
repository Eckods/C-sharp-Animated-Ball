//Author: Steve Sanchez
//Author's email: returnofsimba@gmail.com
//Course: CPSC223N
//Assignment number: 2
//Due date: 10/1/16
//Purpose of Program: Displays a ball at the center of a graphic area
//					  that travels in a direction based on the user input
//					  (in degrees). The user is given the coordinates of 
//					  the balls center per each update it makes during travel.
//Purpose of File: Creates the user interface for the program
//Filename: AnimatedBallFrame.cs
//Command to compile this file:
//mcs -target:library AnimatedBallFrame.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:AnimatedBallLogic.dll -out:AnimatedBallFrame.dll
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class AnimatedBallFrame : Form{
	private const int frameWidth = 1600;
	private const int frameHeight = 900; 
	private Label title = new Label();
	private Label degrees = new Label();
	private Label coordText = new Label();
	private Label xText = new Label();
	private Label yText = new Label();
	private Button startButton = new Button();
	private Button quitButton = new Button();
	private TextBox degreeBox = new TextBox();
	private TextBox xBox = new TextBox();
	private TextBox yBox = new TextBox();

	private const int ballRadius = 14;
	private const int graphTopLocation = 50;
	private const int graphHeight = 670;
	private const int varToConvertToRadians = 180;
	private const double delayNumerator = 1000.0;
	private int ballIntCoordX;
	private int ballIntCoordY;
	private double ballRadians;
	private string degreesWhenPaused;
	private const double graphicRefreshRate = 30.0; //Constant refresh rate during execution of this program
	private static System.Timers.Timer graphicClock = new System.Timers.Timer();
	private static System.Timers.Timer ballClock = new System.Timers.Timer();
	private bool ballClockActive = false; //Initial state: The clock control is inactive
	private bool isContinuingPause = false;
	AnimatedBallLogic algorithm = new AnimatedBallLogic(ballRadius, frameWidth, graphTopLocation, graphHeight); //Pass these variables for calculation of actual coordinates & initial coordinates
	
	private Point titleLocation = new Point(680,15);
	private Point degreesLocation = new Point(125,750);
	private Point degreeBoxLocation = new Point(160,785);
	private Point coordTextLocation = new Point(440,740);
	private Point xTextLocation = new Point(472,760);
	private Point xBoxLocation = new Point(440,785);
	private Point yTextLocation = new Point(613,760);
	private Point yBoxLocation = new Point(580,785);
	private Point startLocation = new Point(1026,770);
	private Point quitLocation = new Point(1326,770);
   
	public AnimatedBallFrame(){
		//Sets the title of form
		Text = "Animated Ball";
		System.Console.WriteLine("frameWidth = {0}. frameHeight = {1}.",frameWidth,frameHeight);

		//Sets the size of form
		Size = new Size(frameWidth,frameHeight);

		//Sets the background color of form
		BackColor = Color.FromArgb(48,48,48);

		//Sets the font size, text, size, and location of labels
		title.Font = new Font(title.Font.FontFamily.Name, 14);
		title.Text = "Animation by Steve Sanchez";
		title.Size = new Size(280,30);
		title.Location = titleLocation;
		title.BackColor = Color.Transparent;
		degrees.Font = new Font(degrees.Font.FontFamily.Name, 12);
		degrees.Text = "Enter the degrees:";
		degrees.Size = new Size(200,20);
		degrees.Location = degreesLocation;
		coordText.Font = new Font(coordText.Font.FontFamily.Name, 12);
		coordText.Text = "Coordinates of ball's center";
		coordText.Size = new Size(300,20);
		coordText.Location = coordTextLocation;
		xText.Font = new Font(xText.Font.FontFamily.Name, 12);
		xText.Text = "X";
		xText.Size = new Size(20,20);
		xText.Location = xTextLocation;
		yText.Font = new Font(yText.Font.FontFamily.Name, 12);
		yText.Text = "Y";
		yText.Size = new Size(20,20);
		yText.Location = yTextLocation;

		//Sets the font size, text, size, location, and background color of buttons
		startButton.Font = new Font(startButton.Font.FontFamily.Name, 12);
		startButton.Text = "Start";
		startButton.Size = new Size(120,55);
		startButton.Location = startLocation;
		startButton.BackColor = Color.FromArgb(24,76,59);
		quitButton.Font = new Font(quitButton.Font.FontFamily.Name, 12);
		quitButton.Text = "Quit";
		quitButton.Size = new Size(120,55);
		quitButton.Location = quitLocation;
		quitButton.BackColor = Color.FromArgb(86,35,26);
		
		//Sets the font size, size, and location of textboxes
		//Also sets the 2 output boxes to read-only with a white backcolor
		degreeBox.Font = new Font(degreeBox.Font.FontFamily.Name, 12);
		degreeBox.Size = new Size(80,55);
		degreeBox.Location = degreeBoxLocation;
		xBox.Font = new Font(xBox.Font.FontFamily.Name, 12);
		xBox.Size = new Size(80,55);
		xBox.Location = xBoxLocation;
		xBox.ReadOnly = true; //Makes the text box non-editable by user
		xBox.BackColor = Color.White;
		yBox.Font = new Font(yBox.Font.FontFamily.Name, 12);
		yBox.Size = new Size(80,55);
		yBox.Location = yBoxLocation;
		yBox.ReadOnly = true;
		yBox.BackColor = Color.White;
		
		//Adds the objects to the form
		Controls.Add(title);
		Controls.Add(degrees);
		Controls.Add(degreeBox);
		Controls.Add(coordText);
		Controls.Add(xText);
		Controls.Add(xBox);
		Controls.Add(yText);
		Controls.Add(yBox);
		Controls.Add(startButton);
		Controls.Add(quitButton);

		//Associates each button to a method
		startButton.Click += new EventHandler(startGraphing);
		quitButton.Click += new EventHandler(closeProgram);
		
		//Sets the initial coordinates of the ball
		ballIntCoordX = (int)(algorithm.getInitialDoubleCoordX());
		ballIntCoordY = (int)(algorithm.getInitialDoubleCoordY());
		
		//Displays the X and Y Coordinates of the ball in textboxes
		xBox.Text = (ballIntCoordX + ballRadius).ToString();
		yBox.Text = (ballIntCoordY + ballRadius).ToString();
		
		//Initializes the graphicClock and ballClock
		//and associates them to a method to perform per clock tic
		graphicClock.Enabled = false;
		graphicClock.Elapsed += new ElapsedEventHandler(updateDisplay);
		
		ballClock.Enabled = false;
		ballClock.Elapsed += new ElapsedEventHandler(updateBall);
	}//End of constructor

	protected override void OnPaint(PaintEventArgs ee){
		Graphics graph = ee.Graphics;
		
		//Creates rgb color brush and draws a red color band behind title on form
		var barBrush = new SolidBrush(Color.FromArgb(68,32,26));
		graph.FillRectangle(barBrush, 0, 0, frameWidth, graphTopLocation);

		//Changes rgb color brush and draws a light gray graphic area in which the ball
		//will travel
		barBrush = new SolidBrush(Color.FromArgb(68,68,68)); 
		graph.FillRectangle(barBrush, 0, graphTopLocation, frameWidth, graphHeight); 

		//Draws the ball on the graphic area
		barBrush = new SolidBrush(Color.FromArgb(114,98,75));
		if (ballIntCoordX + ballRadius >= frameWidth || ballIntCoordX + ballRadius <= 0 || ballIntCoordY + ballRadius <= graphTopLocation
				|| ballIntCoordY + ballRadius >= (graphTopLocation + graphHeight)){
			//Does not draw if center of the ball is out of graphic area
		}
		else{
			graph.FillEllipse(barBrush,ballIntCoordX,ballIntCoordY,2*ballRadius,2*ballRadius);
		}
		
		//The next statement looks like recursion, but it really is not recursion.
		//In fact, it calls the method with the same name located in the super class.
		base.OnPaint(ee);
	}//End of method OnPaint
	
	protected void startGraphing(Object sender, EventArgs events){
		//If the user hasn't input a value in the degrees textbox, do nothing
		//Otherwise, commence ball movement
		if (String.IsNullOrEmpty(degreeBox.Text)){
			System.Console.WriteLine("Input a value in the degrees box to start animation.");
		}
		else{
			//If the start button was pressed when it had changed to Pause,
			//then stop both clocks and set isContinuingPause to true.
			//isContinuingPause helps to identify if the ball should continue
			//travel from where it was stopped at
			if (startButton.Text == "Pause"){
				graphicClock.Enabled = false;
				ballClock.Enabled = false;
				ballClockActive = false;
				System.Console.WriteLine("Both clocks have been stopped.");
				startButton.Text = "Start";
				isContinuingPause = true;
				degreesWhenPaused = degreeBox.Text; //This is to check if ball should be drawn in new direction
			}
			//If the ball was paused in place and the degrees input by user has not changed
			//continue its travel
			else if (isContinuingPause == true && degreesWhenPaused == degreeBox.Text){
				startButton.Text = "Pause";
				ballClockActive = true;
				ballClock.Enabled = true;
				graphicClock.Enabled = true;
				isContinuingPause = false; 
				ballIntCoordX = (int)(algorithm.getUpdatedDoubleCoordX());
				ballIntCoordY = (int)(algorithm.getUpdatedDoubleCoordY());	
			}
			else{
				//Change Start button text to Pause
				startButton.Text = "Pause";
				System.Console.WriteLine("Graphing in progress...");
				
				//Initialize or Center the ball again
				initializeCoordinates();
				
				//Sets degrees to whatever degrees the user entered in textbox
				double degreesInput;
				if (Double.TryParse(degreeBox.Text, out degreesInput)){
					ballRadians = degreesInput * System.Math.PI / varToConvertToRadians;
				}
				System.Console.WriteLine("Ball radians = {0}", ballRadians);
				System.Console.WriteLine("Direction of ball = {0} degrees", ballRadians*varToConvertToRadians/System.Math.PI);
				
				//Passes the radians input by user to the algorithm.
				//This is for delta(x) and delta(y) calculation
				algorithm.setRadians(ballRadians);
				
				//Sets the rate at which the display area is repainted
				startGraphicClock(graphicRefreshRate);
				
				//Sets the rate at which the ball is updated/moves
				startBallClock(algorithm.getBallRefreshRate());//ballRefreshRate);
			}
		}
	}//End of method startGraphing
	
	protected void closeProgram(Object sender, EventArgs events){
		//Closes the form
		System.Console.WriteLine("Program closing...");
		Close();
	}//End of method closeProgram
 
 	protected void initializeCoordinates(){
		//Sets the initial coordinates of the ball (centered)
		ballIntCoordX = (int)(algorithm.getInitialDoubleCoordX());
		ballIntCoordY = (int)(algorithm.getInitialDoubleCoordY());
		System.Console.WriteLine("Initial Coordinates: (ballIntCoordX = {0}, ballIntCoordY = {1})", 
							ballIntCoordX, ballIntCoordY);
							
		//Displays the X and Y Coordinates of the ball in textboxes
		xBox.Text = (ballIntCoordX + ballRadius).ToString();
		yBox.Text = (ballIntCoordY + ballRadius).ToString();
	}//End of method initializeCoordinates
	
	protected void startGraphicClock(double refreshRate){
		double elapsedTimeBtwnTics;
		
		//Sets the delay interval for the graphicClock and enables it
		elapsedTimeBtwnTics = delayNumerator/refreshRate; //elapstedTimeBtwnTics has units milliseconds.
		graphicClock.Interval = (int)System.Math.Round(elapsedTimeBtwnTics);
		graphicClock.Enabled = true;
	}//End of method startGraphicClock
	
	protected void startBallClock(double updateRate){
		double elapsedTimeBtwnMovement;
		
		//Sets the delay interval for the ballClock and enables it
		elapsedTimeBtwnMovement = delayNumerator/updateRate; //1000.0ms = 1second. elapsedTimeBtwnMovement has units milliseconds.
		ballClock.Interval = (int)System.Math.Round(elapsedTimeBtwnMovement);
		ballClock.Enabled = true;
		ballClockActive = true;
	}//End of method startBallClock
	
	protected void updateDisplay(Object sender, ElapsedEventArgs evt){
		//Makes the graphic area repaint itself
		Invalidate();
		
		//If the ballClock is inactive, disable the graphicClock and
		//change the start button text from "Pause" to "Start"
		if(ballClockActive == false){
			graphicClock.Enabled = false;
			System.Console.WriteLine("The ball has exited frame.");
			startButton.Text = "Start";
		}
	}//End of method updateDisplay
	
	protected void updateBall(Object sender, ElapsedEventArgs evt){		
		//Rounds the actual coordinates to the nearest whole number
		ballIntCoordX = (int)System.Math.Round(algorithm.getUpdatedDoubleCoordX());
		ballIntCoordY = (int)System.Math.Round(algorithm.getUpdatedDoubleCoordY());
		
		//Updates the displayed coordinates of the ball
		xBox.Text = (ballIntCoordX + ballRadius).ToString();
		yBox.Text = (ballIntCoordY + ballRadius).ToString();
		
		//If the ball has exited graphic area, sets ballClockActive to
		//false and disables the ballClock
		if (ballIntCoordX + ballRadius >= frameWidth || ballIntCoordX + ballRadius <= 0 || ballIntCoordY + ballRadius <= graphTopLocation
				|| ballIntCoordY + ballRadius >= (graphTopLocation + graphHeight)){
			ballClockActive = false;
			ballClock.Enabled = false;
			System.Console.WriteLine("The ball clock has stopped.");
		}
	}//End of method updateBall

}//End of class AnimatedBallFrame



