using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPathFinding : MonoBehaviour {




	const float minPathUpdateTime = .2f;
	const float pathUpdateMoveThreshold = .5f;

	public Vector3 target;
	public float speed = 20;
	public float turnSpeed = 500;
	public float turnDst = 2;
	public float stoppingDst = 5;

	UnitPath path;

	void Start()
	{
		StartCoroutine(UpdatePath());
	}
	public void travel(Vector3 tar)
	{
		target = tar;
       

    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
	{
		if (pathSuccessful)
		{
			path = new UnitPath(waypoints, transform.position, turnDst, stoppingDst);

			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator UpdatePath()
	{

		if (Time.timeSinceLevelLoad < .3f)
		{
			yield return new WaitForSeconds(.3f);
		}
		PathRequestManager.RequestPath(transform.position, target, OnPathFound);

		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target;

		while (true)
		{
			yield return new WaitForSeconds(minPathUpdateTime);
			if ((target - targetPosOld).sqrMagnitude > sqrMoveThreshold)
			{
				PathRequestManager.RequestPath(transform.position, target, OnPathFound);
				targetPosOld = target;
			}
		}
	}

	IEnumerator FollowPath()
	{

		bool followingPath = true;
		int pathIndex = 0;
		//transform.LookAt(path.lookPoints[0]);

		float speedPercent = 1;


		while (followingPath&&pathIndex<path.turnBoundaries.Length&&pathIndex<path.lookPoints.Length)
		{
			
			Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
			while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
			{
				if (pathIndex == path.finishLineIndex)
				{
					followingPath = false;
					break;
				}
				else
				{
					pathIndex++;
				}
			}

			if (followingPath)
			{

				if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
				{
					speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
					if (speedPercent < 0.01f)
					{
						followingPath = false;
					}
				}

				transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
				Vector3 lookDir = path.lookPoints[pathIndex] - transform.position;
				//lookDir.y = 0;
				Quaternion targetRotation = Quaternion.LookRotation(lookDir);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);


			}

			yield return null;

		}
	}

	
}
