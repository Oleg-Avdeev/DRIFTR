using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    public void setTarget(Transform _target)
    { 
        target = _target;

        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!target) return;

        Vector3 MoveDelta = (target.position - m_LastTargetPosition);

        if (MoveDelta.magnitude > 50)
        {
            m_LastTargetPosition = target.position;
            transform.position = m_LastTargetPosition;
            return;
        }

        bool updateLookAheadTarget = Mathf.Abs(MoveDelta.magnitude) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            m_LookAheadPos = lookAheadFactor * MoveDelta;
        }
        else
        {
            m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

        transform.position = newPos;
    }

}

