using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wokarol.Utils;

namespace Wokarol
{
    public class RopeController : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer;
        [Header("Physics line settings")]
        [SerializeField] Transform ropeOrigin = default;
        [SerializeField] LayerMask groundMask = default;
        [SerializeField] float edgeOffset = 1f;
        [Space]
        [SerializeField] bool drawDebug = default;

        PhysicsLine line;

        private void OnValidate() {
            if (edgeOffset < 0) edgeOffset = 0;

            if(line != null) {
                line.GroundMask = groundMask;
                line.EdgeOffset = edgeOffset;
                line.DrawDebug = drawDebug;
            }
        }

        private void Start() {
            // Creating new line
            line = new PhysicsLine(transform.position, ropeOrigin.position, edgeOffset, groundMask) {
                DrawDebug = drawDebug
            };

            // Setting starting line
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, ropeOrigin.position);

            // Events for anchor adding/removing
            line.AnchorAdded += a => {
                int positionCount = lineRenderer.positionCount;
                lineRenderer.positionCount = positionCount + 1;
                lineRenderer.SetPosition(positionCount - 1, a.Position);
            };
            line.AnchorRemoved += a => {
                lineRenderer.positionCount -= 1;
            };
        }

        private void Update() {
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, (Vector2)transform.position);
        }

        private void FixedUpdate() {
            line.Tick(transform.position);
        }
    }
}
