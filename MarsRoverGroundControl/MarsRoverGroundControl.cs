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
        public string CommandIn
        {
            get => default;
            set
            {
            }
        }

        public string TelemetryOut
        {
            get => default;
            set
            {
            }
        }

        public static void Main(string[] args)
        {
            bool exitCode = false;
            int[] plateauCoord = new int[2];
            int[] roverAttitude = new int[2];
            Regex regPlateau = new Regex(@"^\d+\s\d+$");
            Regex regDeploy = new Regex(@"^\d+\s\d+\s[NESW]{1}$");
            Regex regMovement = new Regex(@"^[LMR]{1,}$");
            var _MarsRover = new MarsRover();
            while (!exitCode)
            {
                var cmdIn = Console.ReadLine().ToUpper();
                if (regPlateau.IsMatch(cmdIn))
                {
                    //determine if a valid plateau boundary is entered
                    var pCoord = cmdIn.Split(" ", StringSplitOptions.None);
                    int.TryParse(pCoord[0], out plateauCoord[0]);
                    int.TryParse(pCoord[1], out plateauCoord[1]);
                    _MarsRover.SetBoundry(plateauCoord[0], plateauCoord[1]);
                    Console.WriteLine($"Plateau Boundary at {_MarsRover.GetBoundary()[0]}, {_MarsRover.GetBoundary()[1]}");
                }
                else if (regDeploy.IsMatch(cmdIn))
                {
                    //determine if a valid coordinate and heading is entered
                    var rCoord = cmdIn.Split(" ", StringSplitOptions.None);
                    int.TryParse(rCoord[0], out roverAttitude[0]);
                    int.TryParse(rCoord[1], out roverAttitude[1]);
                    _MarsRover.Deploy(roverAttitude[0], roverAttitude[1], char.Parse(rCoord[2]));
                    object[] testrover = _MarsRover.Detect();
                    Console.WriteLine($"Rover deployed at {_MarsRover.Detect()[0]}, {_MarsRover.Detect()[1]}, facing {_MarsRover.Detect()[2]}");
                }
                else if (regMovement.IsMatch(cmdIn))
                {
                    _MarsRover.MoveandTurn(cmdIn);
                    Console.WriteLine($"{_MarsRover.Detect()[0]} {_MarsRover.Detect()[1]} {_MarsRover.Detect()[2]}");
                    exitCode = true;
                }
            }
        }
    }
}