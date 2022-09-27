using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public static class Globals
    {
        public static List<int[]>? VehicleLocation { get; set; }
    }
    public class NavSys
    {
        private const int PLATEAU_ORIGIN_X = 0;
        private const int PLATEAU_ORIGIN_Y = 0;
        private const int NO_OF_AXIS = 2;
        private const int X_AXIS = 0;
        private const int Y_AXIS = 1;
        private int[]? PlateauBoundry { get; set; }

        public NavSys()
        {
            Globals.VehicleLocation ??= new();
        }

        public int[] SetBoundary(int bX, int bY)
        {
            PlateauBoundry = new int[NO_OF_AXIS * 2] { PLATEAU_ORIGIN_X, PLATEAU_ORIGIN_Y, bX, bY };
            return PlateauBoundry;
        }

        public int[]? GetBoundary()
        {
            return PlateauBoundry;
        }

        public static List<int[]>? UpdateVehLoc(int oldX, int oldY, int newX, int newY)
        {
            if (oldX < PLATEAU_ORIGIN_X || oldY < PLATEAU_ORIGIN_Y)   //new rover register
            {
                if  (newX >= PLATEAU_ORIGIN_X && newY >= PLATEAU_ORIGIN_Y)    //new rover coordination valid
                {
                    Globals.VehicleLocation!.Add(new int[NO_OF_AXIS] { newX, newY });
                }
                else
                {
                    throw new ArgumentException("New rover registration with negative coordinates denied.");
                }
            }
            else if (newX < PLATEAU_ORIGIN_X || newY < PLATEAU_ORIGIN_Y)  //existing rover deregister
            {
                if (oldX >= PLATEAU_ORIGIN_X && oldY >= PLATEAU_ORIGIN_Y)    //old rover coordination valid
                {
                    foreach (var vLoc in Globals.VehicleLocation!.Select((value, i) => new { i, value }))
                    {
                        if (vLoc.value[X_AXIS] == oldX && vLoc.value[Y_AXIS] == oldY)
                        {
                            Globals.VehicleLocation!.RemoveAt(vLoc.i);
                            break;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Old rover deregistration with negative coordinates denied.");
                }
            }
            else
            {
                foreach (var vLoc in Globals.VehicleLocation!.Select((value, i) => new { i, value }))
                {
                    if (vLoc.value[X_AXIS] == oldX && vLoc.value[Y_AXIS] == oldY)
                    {
                        Globals.VehicleLocation!.RemoveAt(vLoc.i);
                        Globals.VehicleLocation!.Add(new int[NO_OF_AXIS] { newX, newY });
                        break;
                    }
                }
            }
            return Globals.VehicleLocation;
        }

        public bool CheckVehLoc(int vX, int vY)
        {
            foreach (var vLoc in Globals.VehicleLocation!.Select((value, i) => new { i, value }))
            {
                if (vLoc.value[X_AXIS] == vX && vLoc.value[Y_AXIS] == vY)
                {
                    return true;
                }
            }
            return false;
        }
    }
}