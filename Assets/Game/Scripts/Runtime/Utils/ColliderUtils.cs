using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wokarol.Utils
{
    public static class ColliderUtils
    {
        static Vector2[] boxNormals = new Vector2[4] {
            new Vector2( 1,  1).normalized,
            new Vector2( 1, -1).normalized,
            new Vector2(-1, -1).normalized,
            new Vector2(-1,  1).normalized
        };


        public static Vector2 FindNormal(Vector2 corner, int cornerIndex, Collider2D collider) {
            if (collider is BoxCollider2D) {
                return FindNormalForBox((BoxCollider2D)collider, cornerIndex);
            }
            if (collider is CircleCollider2D) {
                return FindNormalForCircle((CircleCollider2D)collider, corner);
            }
            throw new NotImplementedException($"Given collider ({collider.name}) is not implemented");
        }

        public static Vector2[] FindNormals(Vector2[] corners, Collider2D collider) {
            Vector2[] results = new Vector2[corners.Length];
            for (int i = 0; i < corners.Length; i++) {
                results[i] = FindNormal(corners[i], i, collider);
            }
            return results;
        }

        /// <summary>
        /// Finds corners for collider
        /// </summary>
        /// <param name="origin">origin used for calculating corners (circle for example)</param>
        /// <param name="collider">source collider</param>
        /// <returns>array of corners</returns>
        /// <exception cref="NotImplementedException">Thrown when given collider type is not supported</exception>
        public static Vector2[] FindCorners(Vector2 origin, Collider2D collider)
        {
            if (collider is BoxCollider2D) {
                return FindCornersForBox((BoxCollider2D)collider);
            }
            if (collider is CircleCollider2D) {
                return FindCornersForCircle(origin, (CircleCollider2D)collider);
            }
            throw new NotImplementedException($"Given collider ({collider.name}) is not implemented");
        }

        // Corners
        private static Vector2[] FindCornersForCircle(Vector2 from, CircleCollider2D collider)
        {
            throw new NotImplementedException();
        }

        private static Vector2[] FindCornersForBox(BoxCollider2D collider)
        {
            Transform transform = collider.transform;
            Vector2 size = collider.size * 0.5f; // Halfed size of collider in local space
            return new Vector2[4] {
                transform.TransformPoint(new Vector3( size.x,  size.y)),
                transform.TransformPoint(new Vector3( size.x, -size.y)),
                transform.TransformPoint(new Vector3(-size.x, -size.y)),
                transform.TransformPoint(new Vector3(-size.x,  size.y))
            };
        }

        // Normals
        private static Vector2 FindNormalForBox(BoxCollider2D collider, int cornerIndex) {
            return collider.transform.TransformDirection(boxNormals[cornerIndex]);
        }

        private static Vector2 FindNormalForCircle(CircleCollider2D collider, Vector2 vector2) {
            throw new NotImplementedException();
        }
    }
}
