using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Geometry
{
    /// <summary>
    /// CompassDirections 代表8个方向。
    /// </summary>
    public enum CompassDirections
    {
        NotSet = 0,
        North = 1, //UP
        NorthEast = 2, //UP Right
        East = 3,
        SouthEast = 4,
        South = 5,
        SouthWest = 6,
        West = 7,
        NorthWest = 8
    }

    public static class CompassDirectionsHelper
    {
        private static IList<CompassDirections> AllCompassDirections = new List<CompassDirections>();

        #region Static Ctor
        static CompassDirectionsHelper()
        {
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.East);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.West);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.South);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.North);

            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.SouthEast);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.SouthWest);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.NorthEast);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.NorthWest);
        } 
        #endregion

        #region GetAllCompassDirections
        public static IList<CompassDirections> GetAllCompassDirections()
        {
            return CompassDirectionsHelper.AllCompassDirections;
        }
        #endregion

        #region GetAntiCompassDirections
        /// <summary>
        /// GetAntiCompassDirections 获取当前方向相反的方向。
        /// </summary>       
        public static CompassDirections GetAntiCompassDirections(CompassDirections direction)
        {
            if (direction == CompassDirections.NotSet)
            {
                return CompassDirections.NotSet;
            }

            if (direction == CompassDirections.North)
            {
                return CompassDirections.South;
            }

            if (direction == CompassDirections.South)
            {
                return CompassDirections.North;
            }

            if (direction == CompassDirections.East)
            {
                return CompassDirections.West;
            }

            if (direction == CompassDirections.West)
            {
                return CompassDirections.East;
            }

            if (direction == CompassDirections.NorthEast)
            {
                return CompassDirections.SouthWest;
            }

            if (direction == CompassDirections.NorthWest)
            {
                return CompassDirections.SouthEast;
            }

            if (direction == CompassDirections.SouthWest)
            {
                return CompassDirections.NorthEast;
            }

            if (direction == CompassDirections.SouthEast)
            {
                return CompassDirections.NorthWest;
            }

            return CompassDirections.NotSet;
        } 
        #endregion
    }
}
