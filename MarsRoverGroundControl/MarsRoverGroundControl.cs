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
        public string CommandIn { get; set; }

        public string TelemetryOut { get; set; }

        public static void Main(string[] args)
        {
            MarsRoverGroundControl GC = new MarsRoverGroundControl();
            bool exitCode = false;
            int[] plateauCoord = new int[2];
            int[] roverAttitude = new int[2];
            List<MarsRover> _MarsRover = new();
            int _MarsRoverCount = 0;
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
                    int.TryParse(pCoord[0], out plateauCoord[0]);
                    int.TryParse(pCoord[1], out plateauCoord[1]);

                    _MarsRover.Add(new MarsRover());
                    _MarsRover[_MarsRoverCount].SetBoundry(plateauCoord[0], plateauCoord[1]);
                    Console.WriteLine($"Plateau Boundary at {_MarsRover[_MarsRoverCount].GetBoundary()[0]}, {_MarsRover[_MarsRoverCount].GetBoundary()[1]}");
                }
                else if (regRoverDeploy.IsMatch(GC.CommandIn))
                {
                    //determine if a valid coordinate and heading is entered
                    var rCoord = GC.CommandIn.Split(" ", StringSplitOptions.None);
                    int.TryParse(rCoord[0], out roverAttitude[0]);
                    int.TryParse(rCoord[1], out roverAttitude[1]);

                    _MarsRover[_MarsRoverCount].Deploy(roverAttitude[0], roverAttitude[1], char.Parse(rCoord[2]));
                    object[] testrover = _MarsRover[_MarsRoverCount].Detect();
                    Console.WriteLine($"Rover deployed at {_MarsRover[_MarsRoverCount].Detect()[0]}, {_MarsRover[_MarsRoverCount].Detect()[1]}, facing {_MarsRover[_MarsRoverCount].Detect()[2]}");
                }
                else if (regRoverMovement.IsMatch(GC.CommandIn))
                {
                    _MarsRover[_MarsRoverCount].MoveandTurn(GC.CommandIn);
                    Console.WriteLine($"{_MarsRover[_MarsRoverCount].Detect()[0]} {_MarsRover[_MarsRoverCount].Detect()[1]} {_MarsRover[_MarsRoverCount].Detect()[2]}");
                    exitCode = true;
                }
            }
        }
    }
}