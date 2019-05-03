using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wokarol.Utils;

[RequireComponent(typeof(Collider2D))]
[ExecuteInEditMode]
public class NormalDebug : MonoBehaviour
{
    new Collider2D collider;
    Collider2D Collider {
        get {
            if (collider == null) collider = GetComponent<Collider2D>();
            return collider;
        }
    }

    [SerializeField] float distance = 2;
    [SerializeField] [Range(0, 360 * 2)] float angle = 360;

    private void OnDrawGizmos() {
        Vector2 from = FromAngle(angle) * distance + (Vector2)transform.position;
        Gizmos.DrawWireSphere(from, 0.2f);

        try {
            var corners = ColliderUtils.FindCorners(from, Collider);

            Gizmos.color = Color.cyan;
            for (int i = 0; i < corners.Length; i++) {
                Gizmos.DrawLine(from, corners[i]);
            }

            try {
                var normals = ColliderUtils.FindNormals(corners, Collider);
                Gizmos.color = Color.white;
                for (int i = 0; i < normals.Length; i++) {
                    Gizmos.DrawRay(corners[i], normals[i]);
                }
            } catch { }
        } catch { }

    }

    Vector2 FromAngle(float angle) {
        return new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad));
    }
}
