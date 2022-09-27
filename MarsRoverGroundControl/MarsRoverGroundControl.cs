using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("MarsRover.Tests")]
namespace MarsRover
{
    public class MarsRoverGroundControl
    {
        private const int X_AXIS = 0;
        private const int Y_AXIS = 1;
        private const int HEADING = 2;
        private const int NO_OF_AXIS = 2;
        private const int BOUNDARY_X_AXIS = 2;
        private const int BOUNDARY_Y_AXIS = 3;
        private const int NO_ITEM = -1;
        private const string PARAM_SEPARATOR = " ";
        public string? CommandIn { get; set; }

        public string? TelemetryOut { get; set; }

        public static void Main(string[] args)
        {
            bool exitCode = false;
            int[] plateauCoord = new int[NO_OF_AXIS];
            int[] roverAttitude = new int[NO_OF_AXIS];

            MarsRoverGroundControl GC = new();
            List<MarsRover> _MarsRovers = new();
            int _MarsRoverCount = NO_ITEM;
            List<NavSys> _NavSys = new();
            int _NavSysCount = NO_ITEM;

            Regex regPlateau = new(@"^\d+\s\d+$");
            Regex regRoverDeploy = new(@"^\d+\s\d+\s[NESW]{1}$");
            Regex regRoverMovement = new(@"^[LMR]{1,}$");
            while (!exitCode)
            {
                GC.CommandIn = Console.ReadLine().ToUpper();
                if (regPlateau.IsMatch(GC.CommandIn))
                {
                    //determine if a valid plateau boundary is entered
                    var pCoord = GC.CommandIn.Split(PARAM_SEPARATOR, StringSplitOptions.None);
                    plateauCoord[X_AXIS] = int.Parse(pCoord[X_AXIS]);
                    plateauCoord[Y_AXIS] = int.Parse(pCoord[Y_AXIS]);

                    _NavSys.Add(new MarsRover());
                    _NavSysCount++;
                    _NavSys[_NavSysCount].SetBoundary(plateauCoord[X_AXIS], plateauCoord[Y_AXIS]);
                    Console.WriteLine($"Plateau Boundary at {_NavSys[_NavSysCount].GetBoundary()[BOUNDARY_X_AXIS]}, {_NavSys[_NavSysCount].GetBoundary()[BOUNDARY_Y_AXIS]}");
                }
                else if (regRoverDeploy.IsMatch(GC.CommandIn))
                {
                    //determine if a valid coordinate and heading is entered
                    var rCoord = GC.CommandIn.Split(PARAM_SEPARATOR, StringSplitOptions.None);
                    roverAttitude[X_AXIS] = int.Parse(rCoord[X_AXIS]);
                    roverAttitude[Y_AXIS] = int.Parse(rCoord[Y_AXIS]);

                    bool found = false;
                    foreach (var rover in _MarsRovers.Select((value, i) => new { i, value }))
                    {
                        if (Convert.ToInt32(rover.value.Detect()[X_AXIS]) == roverAttitude[X_AXIS] && Convert.ToInt32(rover.value.Detect()[Y_AXIS]) == roverAttitude[Y_AXIS])
                        {
                            MarsRover item = _MarsRovers[rover.i];
                            _MarsRovers.RemoveAt(rover.i);
                            _MarsRovers.Add(item);
                            found = true;
                            break;
                        }
                    }
                    if (_MarsRoverCount < 0 || !found)
                    {
                        _MarsRovers.Add(new MarsRover());
                        _MarsRoverCount++;
                        NavSys.UpdateVehLoc(-1, -1, roverAttitude[X_AXIS], roverAttitude[Y_AXIS]);
                    }
                    _MarsRovers[_MarsRoverCount].Deploy(roverAttitude[X_AXIS], roverAttitude[Y_AXIS], char.Parse(rCoord[HEADING]));
                    _MarsRovers[_MarsRoverCount].Myboundary = _NavSys[_NavSysCount].GetBoundary();
                    Console.WriteLine($"Rover deployed at {_MarsRovers[_MarsRoverCount].Detect()[X_AXIS]}, {_MarsRovers[_MarsRoverCount].Detect()[Y_AXIS]}, facing {_MarsRovers[_MarsRoverCount].Detect()[HEADING]}");
                }
                else if (regRoverMovement.IsMatch(GC.CommandIn))
                {
                    _MarsRovers[_MarsRoverCount].MoveandTurn(GC.CommandIn);
                    Console.WriteLine($"{_MarsRovers[_MarsRoverCount].Detect()[X_AXIS]} {_MarsRovers[_MarsRoverCount].Detect()[Y_AXIS]} {_MarsRovers[_MarsRoverCount].Detect()[HEADING]}");
                }
                else if (GC.CommandIn == "")
                {
                    if (_MarsRovers.Count > 0)
                    {
                        Console.WriteLine($"Mars Rover at {_MarsRovers[_MarsRoverCount].Detect()[X_AXIS]}, {_MarsRovers[_MarsRoverCount].Detect()[Y_AXIS]} is now being retreated.");
                        NavSys.UpdateVehLoc(Convert.ToInt32(_MarsRovers[_MarsRoverCount].Detect()[X_AXIS]), Convert.ToInt32(_MarsRovers[_MarsRoverCount].Detect()[Y_AXIS]), -1, -1);
                        _MarsRovers.RemoveAt(_MarsRoverCount--);
                    }
                    else if (_NavSys.Count > 0)
                    {
                        Console.WriteLine($"Plateau with Boundary at {_NavSys[_NavSysCount].GetBoundary()[BOUNDARY_X_AXIS]}, {_NavSys[_NavSysCount].GetBoundary()[BOUNDARY_Y_AXIS]} is now being let alone.");
                        _NavSys.RemoveAt(_NavSysCount--);
                    }
                    else exitCode = true;
                }
            }
        }
    }
}