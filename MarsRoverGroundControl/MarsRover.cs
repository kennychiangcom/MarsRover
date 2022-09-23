using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public class MarsRover
    {
        private int[]? Coordinates { get; set; }

        private char Heading { get; set; }

        public string Move(string movement)
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
                        switch (Heading)
                        {
                            case 'N':
                                Coordinates[1]++;
                                break;
                            case 'E':
                                Coordinates[0]++;
                                break;
                            case 'S':
                                Coordinates[1]--;
                                break;
                            case 'W':
                                Coordinates[0]--;
                                break;
                        }
                        break;
                }
            }
            return "";
        }

        public char Turn()
        {
            throw new System.NotImplementedException();
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