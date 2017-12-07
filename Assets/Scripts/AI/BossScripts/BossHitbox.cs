using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitbox : MonoBehaviour {

	[SerializeField]
	private Grim boss;
	[SerializeField]
	private bool isLeft;

	[SerializeField]
	private string collisionTag;

	[SerializeField]
	private GameObject breakable;

	[SerializeField]
	private BossCrystals crystal;
	void Start () {


		
	}
	void Update () {
		
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (collisionTag == "Freeze")
		{
			if (other.gameObject.GetComponent<Bullet>() && other.gameObject.GetComponent<Bullet>().type == BulletType.Ice)
			{
				if (isLeft)
					boss.freeze(0);
				else
					boss.freeze(1);
			}

		}

		else if (collisionTag == other.gameObject.tag)
		{
			Debug.Log("Hit by " + collisionTag);
			if (breakable)
			{
				Destroy(breakable.gameObject);
			}

			if (crystal)
			{
				crystal.isExposed = true;
			}
		}

	} 
}
