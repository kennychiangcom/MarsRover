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
        public string CommandIn { get; set; }

        public string TelemetryOut { get; set; }

        public static void Main(string[] args)
        {
            bool exitCode = false;
            int[] plateauCoord = new int[NO_OF_AXIS];
            int[] roverAttitude = new int[NO_OF_AXIS];

            MarsRoverGroundControl GC = new MarsRoverGroundControl();
            List<MarsRover> _MarsRovers = new();
            int _MarsRoverCount = -1;
            List<NavSys> _NavSys = new();
            int _NavSysCount = -1;

            Regex regPlateau = new(@"^\d+\s\d+$");
            Regex regRoverDeploy = new(@"^\d+\s\d+\s[NESW]{1}$");
            Regex regRoverMovement = new(@"^[LMR]{1,}$");
            while (!exitCode)
            {
                GC.CommandIn = Console.ReadLine().ToUpper();
                if (regPlateau.IsMatch(GC.CommandIn))
                {
                    //determine if a valid plateau boundary is entered
                    var pCoord = GC.CommandIn.Split(" ", StringSplitOptions.None);
                    int.TryParse(pCoord[X_AXIS], out plateauCoord[X_AXIS]);
                    int.TryParse(pCoord[Y_AXIS], out plateauCoord[Y_AXIS]);

                    _NavSys.Add(new MarsRover());
                    _NavSysCount++;
                    _NavSys[_NavSysCount].SetBoundry(plateauCoord[X_AXIS], plateauCoord[Y_AXIS]);
                    Console.WriteLine($"Plateau Boundary at {_NavSys[_NavSysCount].GetBoundary()[X_AXIS]}, {_NavSys[_NavSysCount].GetBoundary()[Y_AXIS]}");
                }
                else if (regRoverDeploy.IsMatch(GC.CommandIn))
                {
                    //determine if a valid coordinate and heading is entered
                    var rCoord = GC.CommandIn.Split(" ", StringSplitOptions.None);
                    int.TryParse(rCoord[X_AXIS], out roverAttitude[X_AXIS]);
                    int.TryParse(rCoord[Y_AXIS], out roverAttitude[Y_AXIS]);

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
                    }
                    _MarsRovers[_MarsRoverCount].Deploy(roverAttitude[X_AXIS], roverAttitude[Y_AXIS], char.Parse(rCoord[HEADING]));
                    _MarsRovers[_MarsRoverCount].Myboundary = _NavSys[_NavSysCount].GetBoundary();
                    //object[] testrover = _MarsRover[_MarsRoverCount].Detect();
                    Console.WriteLine($"Rover deployed at {_MarsRovers[_MarsRoverCount].Detect()[X_AXIS]}, {_MarsRovers[_MarsRoverCount].Detect()[Y_AXIS]}, facing {_MarsRovers[_MarsRoverCount].Detect()[HEADING]}");
                }
                else if (regRoverMovement.IsMatch(GC.CommandIn))
                {
                    _MarsRovers[_MarsRoverCount].MoveandTurn(GC.CommandIn);
                    Console.WriteLine($"{_MarsRovers[_MarsRoverCount].Detect()[X_AXIS]} {_MarsRovers[_MarsRoverCount].Detect()[Y_AXIS]} {_MarsRovers[_MarsRoverCount].Detect()[HEADING]}");
                }
                else if (GC.CommandIn == "")
                {
                    if (_MarsRovers.Count > 0) _MarsRovers.RemoveAt(_MarsRoverCount--);
                    else if (_NavSys.Count > 0) _NavSys.RemoveAt(_NavSysCount--);
                    else exitCode = true;
                }
            }
        }
    }
}