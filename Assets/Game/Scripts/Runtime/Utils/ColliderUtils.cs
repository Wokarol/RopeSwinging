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
        public static Vector2[] FindCorners(Vector2 origin, Collider2D collider) {
            if (collider is BoxCollider2D) {
                return FindCornersForBox((BoxCollider2D)collider);
            }
            if (collider is CircleCollider2D) {
                return FindCornersForCircle(origin, (CircleCollider2D)collider);
            }
            throw new NotImplementedException($"Given collider ({collider.name}) is not implemented");
        }

        // Corners
        private static Vector2[] FindCornersForCircle(Vector2 from, CircleCollider2D collider) {
            Transform transform = collider.transform;
            Vector2 p = transform.InverseTransformPoint(from);
            Vector2 c = collider.offset;
            float r = collider.radius;

            Vector2 c2 = Vector2.Lerp(c, p, .5f);
            float r2 = Vector2.Distance(c2, c);
            float d = Vector2.Distance(c, c2);

            float a = (r * r - r2 * r2 + d * d) / (2 * d);
            Vector2 p2 = c + a * (c2 - c) / d;
            float h = Mathf.Sqrt(r * r - a * a);

            Vector2 scalledOffset = h * (c2 - c);

            return new Vector2[] {
                transform.TransformPoint(p2 + new Vector2( scalledOffset.y, -scalledOffset.x) / d),
                transform.TransformPoint(p2 + new Vector2(-scalledOffset.y,  scalledOffset.x) / d)
            };
        }

        private static Vector2[] FindCornersForBox(BoxCollider2D collider) {
            Transform transform = collider.transform;
            Vector2 size = collider.size * 0.5f; // Halfed size of collider in local space
            return new Vector2[4] {
                transform.TransformPoint(new Vector2( size.x,  size.y) + collider.offset),
                transform.TransformPoint(new Vector2( size.x, -size.y) + collider.offset),
                transform.TransformPoint(new Vector2(-size.x, -size.y) + collider.offset),
                transform.TransformPoint(new Vector2(-size.x,  size.y) + collider.offset)
            };
        }

        // Normals
        private static Vector2 FindNormalForBox(BoxCollider2D collider, int cornerIndex) {
            return collider.transform.TransformDirection(boxNormals[cornerIndex]);
        }

        private static Vector2 FindNormalForCircle(CircleCollider2D collider, Vector2 corner) {
            Vector2 c = collider.transform.TransformPoint(collider.offset);
            return (corner - c).normalized;
        }
    }
}
