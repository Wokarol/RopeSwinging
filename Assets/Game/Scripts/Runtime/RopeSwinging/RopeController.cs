using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wokarol.Utils;

namespace Wokarol
{
    public class RopeController : MonoBehaviour
    {
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
            line = new PhysicsLine(transform.position, ropeOrigin.position, edgeOffset, groundMask) {
                DrawDebug = drawDebug
            };
        }

        private void FixedUpdate() {
            line.Tick(transform.position);
        }
    }
}
