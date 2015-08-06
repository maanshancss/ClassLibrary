using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Geometry
{
    /// <summary>
    /// RotationAngles 旋转的角度。
    /// </summary>
    public static class RotationAngles
    {
        public const float A0 = (float)0;
        public const float A45 = (float)Math.PI / 4;
        public const float A90 = (float)Math.PI / 2;
        public const float A135 = (float)(3 * Math.PI) / 4;
        public const float A180 = (float)Math.PI;
        public const float A225 = (float)(5 * Math.PI) / 4;
        public const float A270 = (float)-Math.PI / 2;
        public const float A315 = (float)-Math.PI / 4;

        #region GetAngleOfDirection
        /// <summary>
        /// GetAngleOfDirection 获取当前方向与CompassDirections.North的夹角。
        /// </summary>       
        public static float GetAngleOfDirection(CompassDirections direction)
        {
            if (direction == CompassDirections.North)
            {
                return RotationAngles.A0;
            }

            if (direction == CompassDirections.NorthEast)
            {
                return RotationAngles.A45;
            }

            if (direction == CompassDirections.NorthWest)
            {
                return RotationAngles.A315;
            }

            if (direction == CompassDirections.South)
            {
                return RotationAngles.A180;
            }

            if (direction == CompassDirections.SouthEast)
            {
                return RotationAngles.A135;
            }

            if (direction == CompassDirections.SouthWest)
            {
                return RotationAngles.A225;
            }

            if (direction == CompassDirections.East)
            {
                return RotationAngles.A90;
            }

            if (direction == CompassDirections.West)
            {
                return RotationAngles.A270;
            }

            return RotationAngles.A0;
        }
        #endregion
    }
}
