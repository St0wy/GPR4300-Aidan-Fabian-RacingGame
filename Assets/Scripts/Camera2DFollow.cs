using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UnityStandardAssets
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float offsetZ;
        private Vector3 mLastTargetPosition;
        private Vector3 currentVelocity;
        private Vector3 lookAheadPos;

        // Use this for initialization
        private void Start()
        {
            var position = target.position;
            mLastTargetPosition = position;
            var transform1 = transform;
            offsetZ = (transform1.position - position).z;
            transform1.parent = null;
        }

        // Update is called once per frame
        private void Update()
        {
            // only update lookahead pos if accelerating or changed direction
            var xMoveDelta = (target.position - mLastTargetPosition).x;

            var updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                lookAheadPos = Vector3.right * (lookAheadFactor * Mathf.Sign(xMoveDelta));
            }
            else
            {
                lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            var position = target.position;
            var aheadTargetPos = position + lookAheadPos + Vector3.forward * offsetZ;
            var newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

            transform.position = newPos;

            mLastTargetPosition = position;
        }
    }
}