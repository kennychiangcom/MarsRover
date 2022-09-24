using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public class MarsRover : NavSys
    {
        private int[]? Coordinates { get; set; }

        private char Heading { get; set; }
        private int[]? Myboundary { get; set; }

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
                        Myboundary = GetBoundary();
                        switch (Heading)
                        {
                            case 'N':
                                if (Myboundary[3] >= Coordinates[1] + 1) Coordinates[1]++;
                                break;
                            case 'E':
                                if (Myboundary[2] >= Coordinates[0] + 1) Coordinates[0]++;
                                break;
                            case 'S':
                                if (Myboundary[1] <= Coordinates[1] - 1) Coordinates[1]--;
                                break;
                            case 'W':
                                if (Myboundary[0] <= Coordinates[0] - 1) Coordinates[0]--;
                                break;
                        }
                        break;
                }
            }
            return "";
        }

        public object[] Deploy(int rX, int rY, char rH)
        {
            Coordinates = new int[2] { rX, rY };
            Heading = rH;
            return new object[] { Coordinates[0], Coordinates[1], Heading };
        }

        public object[] Detect()
        {
            return new object[] { Coordinates[0], Coordinates[1], Heading };
        }
    }
}