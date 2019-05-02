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
        [SerializeField] bool drawDebug;

        Vector2 previousPosition;

        // Anchor memory
        Stack<Anchor> previousAnchors = new Stack<Anchor>();
        Anchor currentAnchor;

        private void Start() {
            previousPosition = transform.position;

            // Sets first anchor to origin position
            currentAnchor = new Anchor(ropeOrigin.position, true);
        }

        private void FixedUpdate() {
            Vector2 originPos = currentAnchor.Position;
            Vector2 playerPos = transform.position;

            // Raycasts from last anchor to current player position
            RaycastHit2D hit = Physics2D.Raycast(originPos, (playerPos - originPos).normalized, Vector2.Distance(originPos, playerPos), groundMask);

            // Checks if last anchor should be deleted
            if (previousAnchors.Count > 0) {
                Anchor previousAnchor = previousAnchors.Peek();
                Vector2 lastToCurrentAnchorPos = currentAnchor.Position - previousAnchor.Position;
                float angle = Vector2.SignedAngle(lastToCurrentAnchorPos, playerPos - currentAnchor.Position);
                if (currentAnchor.Clockwise == angle > 0) {
                    // Removes anchor from memory
                    currentAnchor = previousAnchors.Pop();
                }
            }

            // Checks if hitted something and ands anchor in that case
            if (hit.transform != null) {
                // Gets previous and current direction
                Vector2 previousDirection = previousPosition - originPos;
                Vector2 direction = playerPos - originPos;

                // Ckecks if hit was clockwise or counter clockwise
                float angleBetweenRays = Vector2.SignedAngle(previousDirection, direction);
                bool clockwise = angleBetweenRays < 0;

                // Gets all corners for given collider and find correct one
                var corners = ColliderUtils.FindCorners(originPos, hit.collider);
                var corner = VectorUtils.FindFirstPointInClockWiseOrder(clockwise ? previousDirection : direction, originPos, corners);

                // Ads offset
                corner += (corner - (Vector2)hit.transform.position).normalized * edgeOffset;

                // Add new anchor to memory
                previousAnchors.Push(currentAnchor);
                currentAnchor = new Anchor(corner, clockwise);
            }

            if (drawDebug) {
                // Debug Drawing
                var anchorArray = previousAnchors.ToArray();
                for (int i = 0; i < previousAnchors.Count - 1; i++) {
                    Debug.DrawLine(anchorArray[i].Position, anchorArray[i + 1].Position, Color.red);
                }
                if (previousAnchors.Count > 0) {
                    Debug.DrawLine(previousAnchors.Peek().Position, currentAnchor.Position, Color.green);
                }
                Debug.DrawLine(currentAnchor.Position, playerPos, Color.cyan);
                // ----------- 
            }

            // Updates previous position
            previousPosition = playerPos;
        }
    }

    /// <summary>
    /// Stores info about anchor, position and direction
    /// </summary>
    public struct Anchor
    {
        public readonly Vector2 Position;
        public readonly bool Clockwise;

        public Anchor(Vector2 position, bool clockwise) {
            Position = position;
            Clockwise = clockwise;
        }
    }
}
