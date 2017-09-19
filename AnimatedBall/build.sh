#!/bin/bash
#Author: Steve Sanchez
#Author's email: returnofsimba@gmail.com
#Course: CPSC223N
#Assignment number: 2
#Due date: 10/1/16

#This is a bash shell script to be used for compiling, linking, and executing the C sharp files.
#Give permissions to this file, execute it in the terminal window of the folder where this file resides, and then enter the command: ./build.sh

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile AnimatedBallLogic.cs to create the file: AnimatedBallLogic.dll
mcs -target:library AnimatedBallLogic.cs -r:System.Drawing.dll -out:AnimatedBallLogic.dll

echo Compile AnimatedBallFrame.cs to create the file: AnimatedBallFrame.dll
mcs -target:library AnimatedBallFrame.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:AnimatedBallLogic.dll -out:AnimatedBallFrame.dll

echo Compile AnimatedBallMain.cs and link with AnimatedBallLogic and AnimatedBallFrame.cs to create AnimatedBall.exe
mcs AnimatedBallMain.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:AnimatedBallFrame.dll -out:AnimatedBall.exe

echo View the list of files in the current folder
ls -l

echo Run Assignment 2.
./AnimatedBall.exe

echo The script has finished.












