using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBreakable : MonoBehaviour {
	public float durability;
	public AICrystal crystalPrefab;
	public Transform crystalSpawn;
	private bool destroyed=false;
	public Transform ownerUnit;
	private void Update()
	{
		if (durability <= 0)
		{
			if (!destroyed)
			{
				if (gameObject.transform.parent.GetComponent<AIStateManager>())
					gameObject.transform.parent.GetComponent<AIStateManager>().Stun();

				Breaks();
			}
		
		}

	}
	public void TakeDamage(float damage)
	{
		if (durability > 0)
		{
			if (damage > 0)
			{
				Debug.Log("Breakable takes damage");
				durability -= 1;
				//if (!broken)
				//{
				//	gameObject.transform.root.GetComponent<AIStateManager>().Stun();
				//	breakablePart.GetComponent<Rigidbody>().useGravity = true;
				//	breakablePart.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

				//	breakablePart.transform.parent = null;
				//	breakablePart.GetComponent<BoxCollider>().enabled = true;
				//	broken = true;
				//}
				//else
				//{
				//	gameObject.transform.root.GetComponent<AIStateManager>().Die();

				//}

			}
		}

	}

	public void Breaks()
	{
		if (!destroyed)
		{
			if (crystalPrefab)
			{
				AICrystal crystal = Instantiate(crystalPrefab, crystalSpawn.position, crystalSpawn.rotation);
				crystal.gameObject.transform.parent = ownerUnit.transform;
				crystal.gameObject.GetComponent<BoxCollider>().enabled = true;

			}
			transform.parent = null;
			GetComponent<BoxCollider>().enabled = true;
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			GetComponent<Rigidbody>().AddForce(new Vector3(1,5,0), ForceMode.Impulse);
			destroyed = true;
		}

	}
}
