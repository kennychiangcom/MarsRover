﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public class NavSys
    {
        private const int PLATEAU_ORIGIN_X = 0;
        private const int PLATEAU_ORIGIN_Y = 0;

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

        public int[]? VehicleLocation { get; set; }

        public int[]? UpdateVehLoc(int oldX, int oldY, int newX, int newY)
        {
            return VehicleLocation;
        }
    }
}