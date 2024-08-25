using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace Seacore
{
    /// <summary>
    /// Calculates the density and adjusts the position of dice objects within a defined area, considering border constraints, proximity to other dice, and a central circle.
    /// </summary>
    [RequireComponent(typeof(DiceManager))]
    public class DiceDistributionController : MonoBehaviour
    {
        [Header("Steering Variables")]
        [SerializeField] private CircleController circle;

        [Space]

        [SerializeField] private float densityMinDistance = 2.5f;
        [SerializeField] private float densitySegmentHeight;
        [SerializeField] private Camera mainCamera;

        [SerializeField][Range(0.0f, 1.0f)] private float diceWeight = 0.1f;
        [SerializeField][Range(0.0f, 1.0f)] private float borderWeight = 0.6f;
        [SerializeField][Range(0.0f, 1.0f)] private float chapeauCircleWeight = 0.3f;


        private DiceManager _diceManager;
        private const ushort Size = 4;
        private readonly Vector3[] cornerPointsLineSegments = new Vector3[Size];
        private Vector3 centerLineSegment = Vector3.zero;

        private struct TranspositionInfo
        {
            public Vector3 TransposeVec;
            public bool MaxForce;
            public float Weight;

            public TranspositionInfo(Vector3 transposeVec, float weight, bool maxForce = false)
            {
                TransposeVec = transposeVec;
                Weight = weight;
                MaxForce = maxForce;
            }
        }

#if UNITY_EDITOR
        private readonly List<Color> diceColors = new List<Color>();
#endif

        private void Awake()
        {
            _diceManager = GetComponent<DiceManager>();
        }

        private void Start()
        {

            if (_diceManager == null)
            {
                Debug.LogError("DiceManager is null in DiceDistributionController.");
                enabled = false; // Disable this component if diceManager is null
                return;
            }

            foreach (Die die in _diceManager.Dice)
            {
                if (die == null)
                {
                    Debug.LogError("Die object is null in DiceDistributionController.");
                    continue;
                }

                if (mainCamera == null)
                {
                    Debug.LogError("No Camera is added to the DiceDistributionController");
                    return;
                }
            }
            CalculateLineSegments();
        }

        private void FixedUpdate()
        {
            foreach (Die die in _diceManager.Dice)
            {
                TransposeDiePosition(die);
            }
        }

        private void TransposeDiePosition(Die die)
        {
            Vector3 diePos = die.transform.position;

            List<TranspositionInfo> transposeVecs = new List<TranspositionInfo>();

            AddBorderTranspositions(diePos, transposeVecs);
            AddDiceTranspositions(die, diePos, transposeVecs);
            AddCircleTransposition(die, diePos, transposeVecs);

            //Cancelling out any y movement
            foreach (TranspositionInfo transposeVec in transposeVecs)
            {
                transposeVec.TransposeVec.Scale(new Vector3(1.0f, 0.0f, 1.0f));
            }


            Vector3 newPos = TransposePosition(diePos, transposeVecs, densityMinDistance);
            die.transform.position = newPos;
        }

        private void AddBorderTranspositions(Vector3 diePos, List<TranspositionInfo> transposeVecs)
        {
            for (int i = 0; i < Size; i++)
            {
                Vector3 x = CalculatePerpendicularPointTowardsLine(cornerPointsLineSegments[i], cornerPointsLineSegments[(i + 1) % Size], diePos);
                Vector3 transposeVec = diePos - x;

                Vector3 right = Vector3.Cross(cornerPointsLineSegments[(i + 1) % Size] - cornerPointsLineSegments[i], Vector3.up);
                if (Vector3.Dot(diePos - cornerPointsLineSegments[i], right) > 0)
                    transposeVecs.Add(new TranspositionInfo(-transposeVec, borderWeight, true));
                else
                    transposeVecs.Add(new TranspositionInfo(transposeVec, borderWeight));
            }
        }

        private void AddDiceTranspositions(Die die, Vector3 diePos, List<TranspositionInfo> transposeVecs)
        {
            foreach (Die otherDie in _diceManager.Dice)
            {
                if (die == otherDie) continue;

                Vector3 otherPos = otherDie.transform.position;
                otherPos.y = densitySegmentHeight;

                transposeVecs.Add(new TranspositionInfo(diePos - otherPos, diceWeight));
            }
        }

        private void AddCircleTransposition(Die die, Vector3 diePos, List<TranspositionInfo> transposeVecs)
        {
            Vector3 circlePos = circle.Position;
            circlePos.y = densitySegmentHeight;

            Vector3 circlePoint = CalculatePointTowardsCircle(circlePos, circle.Radius, diePos);
            bool shouldBeInside = _diceManager.DiceContainers[die].State.HasFlag(DieState.Inside);

            Vector3 transposeVec = diePos - circlePoint;
            // If the die is not where it should be, reverse the transpose vector
            if (circle.IsPositionInCircle(diePos) != shouldBeInside)
            {
                transposeVec *= -1;

                // If the die is not where it is supposed to be, ignore minimum distance
                transposeVecs.Add(new TranspositionInfo(transposeVec, chapeauCircleWeight, maxForce: true));
            }
            else
            {
                // If the die is where it should be, apply minimum distance
                transposeVecs.Add(new TranspositionInfo(transposeVec, chapeauCircleWeight));
            }
        }

        private Vector3 TransposePosition(Vector3 currentPosition, List<TranspositionInfo> transposeVecs, float minDistance)
        {
            Vector3 transposeVec = Vector3.zero;
            float totalWeight = 0f;

            foreach (TranspositionInfo transpositionInfo in transposeVecs)
            {
                if (transpositionInfo.MaxForce)
                {
                    totalWeight += transpositionInfo.Weight;
                    transposeVec += transpositionInfo.TransposeVec.normalized * transpositionInfo.Weight;
                }
                else if (transpositionInfo.TransposeVec.sqrMagnitude < (minDistance * minDistance))
                {
                    float adjustedWeight = transpositionInfo.Weight * (1f - transpositionInfo.TransposeVec.sqrMagnitude / (minDistance * minDistance));
                    totalWeight += adjustedWeight;
                    transposeVec += transpositionInfo.TransposeVec.normalized * adjustedWeight;
                }
            }

            return totalWeight == 0f ? currentPosition : currentPosition + transposeVec;
        }

        private void CalculateLineSegments()
        {
            Plane plane = new Plane(Vector3.up, -densitySegmentHeight);

            cornerPointsLineSegments[0] = GetPointOnPlane(mainCamera.ViewportPointToRay(new Vector3(0, 0, 0)), plane);
            cornerPointsLineSegments[1] = GetPointOnPlane(mainCamera.ViewportPointToRay(new Vector3(0, 1, 0)), plane);
            cornerPointsLineSegments[2] = GetPointOnPlane(mainCamera.ViewportPointToRay(new Vector3(1, 1, 0)), plane);
            cornerPointsLineSegments[3] = GetPointOnPlane(mainCamera.ViewportPointToRay(new Vector3(1, 0, 0)), plane);
            centerLineSegment = GetPointOnPlane(mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), plane);
        }

        private Vector3 GetPointOnPlane(Ray ray, Plane plane)
        {
            if (!plane.Raycast(ray, out float distance))
            {
                Debug.LogWarning("One of the rays from the camera is not colliding with the plane at densitySegmentHeight");
                return Vector3.zero;
            }
            return ray.GetPoint(distance);
        }

        private Vector3 CalculatePerpendicularPointTowardsLine(Vector3 a, Vector3 b, Vector3 p)
        {
            Vector3 heading = (b - a).normalized;
            float dotP = Vector3.Dot(p - a, heading);
            return a + heading * Mathf.Clamp(dotP, 0, (b - a).magnitude);
        }

        private Vector3 CalculatePointTowardsCircle(Vector3 center, float radius, Vector3 point)
        {
            return center + (point - center).normalized * radius;
        }

        #region EditorOnly
        private void OnValidate()
        {
            if (mainCamera == null) return;
            _diceManager = GetComponent<DiceManager>();

            CalculateLineSegments();
            diceColors.Clear();
            for (int i = 0; i < Globals.c_amountDie; i++)
                diceColors.Add(UnityEngine.Random.ColorHSV());
        }

        private void OnDrawGizmosSelected()
        {
            DrawViewportRays();
        }

        private void OnDrawGizmos()
        {
            DrawCornerPoints();
            DrawCircle();
            DrawDiceGizmos();
        }

        private void DrawViewportRays()
        {
            DrawRay(mainCamera.ViewportPointToRay(new Vector3(0, 0, 0)));
            DrawRay(mainCamera.ViewportPointToRay(new Vector3(0, 1, 0)));
            DrawRay(mainCamera.ViewportPointToRay(new Vector3(1, 0, 0)));
            DrawRay(mainCamera.ViewportPointToRay(new Vector3(1, 1, 0)));
        }

        private void DrawRay(Ray ray)
        {
            Gizmos.DrawRay(ray);
        }

        private void DrawCornerPoints()
        {
            foreach (var point in cornerPointsLineSegments)
            {
                Handles.DrawSolidDisc(point, Vector3.up, 0.2f);
            }

            Handles.DrawSolidDisc(centerLineSegment, Vector3.up, 0.2f);
            Handles.DrawLines(cornerPointsLineSegments, new int[] { 0, 1, 1, 2, 2, 3, 3, 0 });
        }

        private void DrawCircle()
        {
            Handles.DrawWireDisc(circle.Position, Vector3.up, circle.Radius);
        }

        private void DrawDiceGizmos()
        {
            int count = 0;
            foreach (var die in _diceManager.Dice)
            {
                DrawDieGizmos(die, diceColors[count++]);
            }
        }

        private void DrawDieGizmos(Die die, Color color)
        {
            Vector3 diePos = die.transform.position;
            diePos.y = densitySegmentHeight;

            Handles.color = color;
            Handles.DrawWireDisc(diePos, Vector3.up, densityMinDistance);

            Vector3 circlePos = circle.Position;
            circlePos.y = densitySegmentHeight;
            Vector3 circlePoint = CalculatePointTowardsCircle(circlePos, circle.Radius, diePos);
            Handles.DrawSolidDisc(circlePoint, Vector3.up, Mathf.Lerp(0.1f, 0.4f, chapeauCircleWeight));

            for (int j = 0; j < Size; j++)
            {
                Vector3 x = CalculatePerpendicularPointTowardsLine(cornerPointsLineSegments[j], cornerPointsLineSegments[(j + 1) % Size], diePos);
                Handles.DrawSolidDisc(x, Vector3.up, Mathf.Lerp(0.1f, 0.4f, borderWeight));
            }
        }
        #endregion EditorOnly
    }
}
