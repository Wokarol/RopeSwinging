using System;
using UnityEngine;

namespace Wokarol.Utils
{
    public static class VectorUtils
    {
        /// <summary>
        /// Finds closest point based on angle between targetDirection and point relative to origin in clockwise order
        /// </summary>
        public static Vector2 FindFirstPointInCircularOrder(Vector2 targetDirection, Vector2 origin, Vector2[] points, bool clockwise)
        {
            // Initialisation
            int closest = 0;
            float highest = (clockwise ? -1 : 1) * 180;

            // Loops through all points
            for (int i = 0; i < points.Length; i++) {
                // Checks angle from -targetDirection to point (negative vector together with SignedAngle is used for -180..180 range)
                float angle = Vector2.SignedAngle(-targetDirection, points[i] - origin);

                // Updates current angle if its closer to target
                if (angle > highest == clockwise) {
                    highest = angle;
                    closest = i;
                }
            }

            // Returns result
            return points[closest];
        }
    }
}