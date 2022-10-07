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

        List<NavigationSystem> _NavSys = new();
        int _NavSysCount = NO_ITEM;

        public int[] NewPlateau(int xx, int yy)
        {
            _NavSys.Add(new MarsRover());
            _NavSysCount++;
            _NavSys[_NavSysCount].SetBoundary(xx, yy);
            return _NavSys[_NavSysCount].GetBoundary();
        }

        List<MarsRover> _MarsRovers = new();
        int _MarsRoverCount = NO_ITEM;

        public object[] VehicleDeployOrLocate(int xx, int yy, string hh)
        {
            bool found = false;
            foreach (var rover in _MarsRovers.Select((value, i) => new { i, value }))
            {
                if (Convert.ToInt32(rover.value.Detect()[X_AXIS]) == xx && Convert.ToInt32(rover.value.Detect()[Y_AXIS]) == yy)
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
                NavigationSystem.UpdateVehicleLocation(-1, -1, xx, yy);
            }
            _MarsRovers[_MarsRoverCount].Deploy(xx, yy, char.Parse(hh));
            _MarsRovers[_MarsRoverCount].Myboundary = _NavSys[_NavSysCount].GetBoundary();
            return _MarsRovers[_MarsRoverCount].Detect();
        }

        public static void Main(string[] args)
        {
            bool exitCode = false;
            //int[] plateauCoord = new int[NO_OF_AXIS];
            //int[] roverAttitude = new int[NO_OF_AXIS];

            MarsRoverGroundControl GC = new();

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

                    int[] plateauCoord = GC.NewPlateau(int.Parse(pCoord[X_AXIS]), int.Parse(pCoord[Y_AXIS]));
                    Console.WriteLine($"Plateau Boundary at {plateauCoord[X_AXIS]}, {plateauCoord[Y_AXIS]}");
                }
                else if (regRoverDeploy.IsMatch(GC.CommandIn))
                {
                    //determine if a valid coordinate and heading is entered
                    var rCoord = GC.CommandIn.Split(PARAM_SEPARATOR, StringSplitOptions.None);

                    object[] roverAttitude = GC.VehicleDeployOrLocate(int.Parse(rCoord[X_AXIS]), int.Parse(rCoord[Y_AXIS]), rCoord[HEADING]);
                    Console.WriteLine($"Rover deployed at {roverAttitude[X_AXIS]}, {roverAttitude[Y_AXIS]}, facing {roverAttitude[HEADING]}");
                }
                else if (regRoverMovement.IsMatch(GC.CommandIn))
                {
                    GC._MarsRovers[GC._MarsRoverCount].MoveandTurn(GC.CommandIn);
                    Console.WriteLine($"{GC._MarsRovers[GC._MarsRoverCount].Detect()[X_AXIS]} {GC._MarsRovers[GC._MarsRoverCount].Detect()[Y_AXIS]} {GC._MarsRovers[GC._MarsRoverCount].Detect()[HEADING]}");
                }
                else if (GC.CommandIn == "")
                {
                    if (GC._MarsRovers.Count > 0)
                    {
                        Console.WriteLine($"Mars Rover at {GC._MarsRovers[GC._MarsRoverCount].Detect()[X_AXIS]}, {GC._MarsRovers[GC._MarsRoverCount].Detect()[Y_AXIS]} is now being retreated.");
                        NavigationSystem.UpdateVehicleLocation(Convert.ToInt32(GC._MarsRovers[GC._MarsRoverCount].Detect()[X_AXIS]), Convert.ToInt32(GC._MarsRovers[GC._MarsRoverCount].Detect()[Y_AXIS]), -1, -1);
                        GC._MarsRovers.RemoveAt(GC._MarsRoverCount--);
                    }
                    else if (GC._NavSys.Count > 0)
                    {
                        Console.WriteLine($"Plateau with Boundary at {GC._NavSys[GC._NavSysCount].GetBoundary()[BOUNDARY_X_AXIS]}, {GC._NavSys[GC._NavSysCount].GetBoundary()[BOUNDARY_Y_AXIS]} is now being let alone.");
                        GC._NavSys.RemoveAt(GC._NavSysCount--);
                    }
                    else exitCode = true;
                }
            }
        }
    }
}