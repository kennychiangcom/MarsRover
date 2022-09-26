using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public class MarsRover : NavSys
    {
        private const int X_AXIS = 0;
        private const int Y_AXIS = 1;
        public int[]? Coordinates { get; private set; }
        public char Heading { get; private set; }
        public int[]? Myboundary { get; set; }

        public object[] Deploy(int rX, int rY, char rH)
        {
            Coordinates = new int[2] { rX, rY };
            Heading = rH;
            return new object[] { Coordinates[X_AXIS], Coordinates[Y_AXIS], Heading };
        }

        public string MoveandTurn(string movement)
        {
            foreach (char move in movement)
            {
                switch (move)
                {
                    case 'L':
                        switch (Heading)
                        {
                            case 'N':
                                Heading = 'W';
                                break;
                            case 'E':
                                Heading = 'N';
                                break;
                            case 'S':
                                Heading = 'E';
                                break;
                            case 'W':
                                Heading = 'S';
                                break;
                        }
                        break;
                    case 'R':
                        switch (Heading)
                        {
                            case 'N':
                                Heading = 'E';
                                break;
                            case 'E':
                                Heading = 'S';
                                break;
                            case 'S':
                                Heading = 'W';
                                break;
                            case 'W':
                                Heading = 'N';
                                break;
                        }
                        break;
                    case 'M':
                        //Myboundary = GetBoundary();
                        switch (Heading)
                        {
                            case 'N':
                                if (Myboundary[3] >= Coordinates[Y_AXIS] + 1) Coordinates[Y_AXIS]++;
                                break;
                            case 'E':
                                if (Myboundary[2] >= Coordinates[X_AXIS] + 1) Coordinates[X_AXIS]++;
                                break;
                            case 'S':
                                if (Myboundary[1] <= Coordinates[Y_AXIS] - 1) Coordinates[Y_AXIS]--;
                                break;
                            case 'W':
                                if (Myboundary[0] <= Coordinates[X_AXIS] - 1) Coordinates[X_AXIS]--;
                                break;
                        }
                        break;
                }
            }
            return "";
        }

        public object[] Detect()
        {
            return new object[] { Coordinates[X_AXIS], Coordinates[Y_AXIS], Heading };
        }
    }
}