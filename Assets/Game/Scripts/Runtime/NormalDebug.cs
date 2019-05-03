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

    private void OnDrawGizmos() {
        var corners = ColliderUtils.FindCorners(Vector2.zero, Collider);
        var normals = ColliderUtils.FindNormals(corners, Collider);

        for (int i = 0; i < corners.Length; i++) {
            Gizmos.DrawRay(corners[i], normals[i]);
        }
    }
}
