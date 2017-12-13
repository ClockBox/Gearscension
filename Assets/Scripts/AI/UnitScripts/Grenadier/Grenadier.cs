using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenadier : AIStateManager
{
	public Transform[] gunPoints;
	public float launchAngle;
	public Rigidbody explosionPrefab;
	public float shotInterval;
	private float shotFrequency;

	private Vector3 hopDirection;
    public Transform[] hopTargets;

	public override void StartEvents() { }

	public override void RangedAttack()
    {
        int choice = UnityEngine.Random.Range(0, 2);
        if (!callOnce)
        {
            GameManager.Instance.AudioManager.playAudio(SFXSource, "sfxgunimpactexplosion");
            shotFrequency = shotInterval;
			callOnce = true;
		}
		if (shotFrequency >= shotInterval)
        {
            Vector3 displacement = player.transform.position - transform.position;
			Rigidbody bulletRb = Instantiate(explosionPrefab, gunPoints[choice].position, gunPoints[choice].rotation) as Rigidbody;
            bulletRb.velocity = CalculateVelocityArc(launchAngle, displacement);

			shotFrequency = 0;
        }
        shotFrequency += Time.deltaTime;
	}
	public override void MeleeAttack()
	{
		if (!callOnce)
		{
			pathAgent.enabled = false;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            hopDirection = transform.position - player.transform.position;
            float closetAngle = 45;
            Vector3 hopNode = Vector3.zero;

            for (int i = 0; i < hopTargets.Length; i++)
            {
                Vector3 targetDirection = hopTargets[i].position - transform.position;
                float temp = Vector3.Angle(hopDirection.normalized, targetDirection.normalized);
                if (temp < closetAngle && targetDirection.magnitude > 5)
                {
                    closetAngle = temp;
                    hopNode = hopTargets[i].position - transform.position;
                }
            }
            if (hopNode == Vector3.zero)
            {
                if (hopTargets.Length > 0)
                    hopNode = hopTargets[UnityEngine.Random.Range(0, hopTargets.Length)].position - transform.position;
                else
                    hopNode = transform.forward * 10;
            }

            hopDirection = hopNode;
            callOnce = true;

            rb.velocity = CalculateVelocityArc(45, hopDirection);
        }
    }

	private Vector3 CalculateVelocityArc(float angle, Vector3 displacement)
	{
		float height = displacement.y;
		displacement.y = 0;
		float horizontalDistance = displacement.magnitude;
		float angleRad = angle * Mathf.Deg2Rad;
		displacement.y = horizontalDistance * Mathf.Tan(angleRad);
		horizontalDistance += height / Mathf.Tan(angleRad);
		float velocityStart = Mathf.Sqrt(horizontalDistance * Physics.gravity.magnitude / Mathf.Sin(2 * angleRad));
		return velocityStart * displacement.normalized;
	}
}
