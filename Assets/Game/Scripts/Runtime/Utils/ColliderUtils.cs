using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wokarol.Utils
{
    public static class ColliderUtils
    {
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
                return FindCornersForBox(origin, (BoxCollider2D)collider);
            }
            if (collider is CircleCollider2D) {
                return FindCornersForCircle(origin, (CircleCollider2D)collider);
            }
            throw new NotImplementedException($"Given collider ({collider.name}) is not implemented");
        }

        private static Vector2[] FindCornersForCircle(Vector2 from, CircleCollider2D collider)
        {
            throw new NotImplementedException();
        }

        private static Vector2[] FindCornersForBox(Vector2 from, BoxCollider2D collider)
        {
            Transform transform = collider.transform;
            Vector2 size = collider.size * 0.5f; // Halfed size of collider in local space
            return new Vector2[4] {
                transform.TransformPoint(new Vector3( size.x,  size.y)),
                transform.TransformPoint(new Vector3( size.x, -size.y)),
                transform.TransformPoint(new Vector3(-size.x,  size.y)),
                transform.TransformPoint(new Vector3(-size.x, -size.y))
            };
        }
    }
}
