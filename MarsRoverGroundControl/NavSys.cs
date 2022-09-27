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
        private const int X_AXIS = 0;
        private const int Y_AXIS = 1;

        public NavSys()
        {
            Globals.VehicleLocation ??= new();
        }

        private int[]? PlateauBoundry { get; set; }

        public int[] SetBoundry(int bX, int bY)
        {
            PlateauBoundry = new int[4] { PLATEAU_ORIGIN_X, PLATEAU_ORIGIN_Y, bX, bY };
            return PlateauBoundry;
        }

        public int[]? GetBoundary()
        {
            return PlateauBoundry;
        }

        //public List<int[]>? VehicleLocation { get; set; }

        public List<int[]>? UpdateVehLoc(int oldX, int oldY, int newX, int newY)
        {
            //VehicleLocation ??= new();
            
            if (oldX < 0 || oldY < 0)   //new rover register
            {
                if  (newX >= 0 && newY >= 0)    //new rover coordination valid
                {
                    Globals.VehicleLocation.Add(new int[2] { newX, newY });
                }
                else
                {
                    throw new ArgumentException("New rover registration with negative coordinates denied.");
                }
            }
            else if (newX < 0 || newY < 0)  //existing rover deregister
            {
                if (oldX >= 0 && oldY >= 0)    //old rover coordination valid
                {
                    foreach (var vLoc in Globals.VehicleLocation.Select((value, i) => new { i, value }))
                    {
                        if (vLoc.value[X_AXIS] == oldX && vLoc.value[Y_AXIS] == oldY)
                        {
                            Globals.VehicleLocation.RemoveAt(vLoc.i);
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
                foreach (var vLoc in Globals.VehicleLocation.Select((value, i) => new { i, value }))
                {
                    if (vLoc.value[X_AXIS] == oldX && vLoc.value[Y_AXIS] == oldY)
                    {
                        Globals.VehicleLocation.RemoveAt(vLoc.i);
                        Globals.VehicleLocation.Add(new int[2] { newX, newY });
                        break;
                    }
                }
            }
            return Globals.VehicleLocation;
        }

        public bool CheckVehLoc(int vX, int vY)
        {
            foreach (var vLoc in Globals.VehicleLocation.Select((value, i) => new { i, value }))
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