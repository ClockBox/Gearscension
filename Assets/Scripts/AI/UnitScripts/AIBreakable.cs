using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBreakable : MonoBehaviour {
	bool broken = false;
    public GameObject breakablePart;

	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.G))
		//{
		//	TakeDamage(1);
			
		//}
	}

	public void TakeDamage(float damage)
	{
		if (damage > 0) {

			if (!broken)
			{
				gameObject.transform.root.GetComponent<AIStateManager>().Stun();
				breakablePart.GetComponent<Rigidbody>().useGravity = true;
				
				breakablePart.transform.parent = null;
				breakablePart.GetComponent<BoxCollider>().enabled = true;
				broken = true;
			}
			else
			{
				gameObject.transform.root.GetComponent<AIStateManager>().Die();

			}

		}

	}

}
