using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBreakable : MonoBehaviour
{
	public float durability;
	public AICrystal crystalPrefab;
	public Transform crystalSpawn;
	public Transform ownerUnit;

    public GameObject[] BreakingPieces;
    public GameObject[] DestroyPieces;

    private bool destroyed = false;

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

            for (int i = 0; i < BreakingPieces.Length; i++)
            {
                Collider[] temp = BreakingPieces[i].GetComponents<Collider>();
                if (temp.Length > 0)
                    for (int t = 0; t < temp.Length; t++)
                        temp[t].enabled = false;
                else BreakingPieces[i].AddComponent<BoxCollider>();

                if (!BreakingPieces[i].GetComponent<Rigidbody>())
                    BreakingPieces[i].AddComponent<Rigidbody>();

                BreakingPieces[i].transform.parent = null;
                Destroy(BreakingPieces[i].gameObject, Random.Range(8, 10));
            }

            for (int i = 0; i < DestroyPieces.Length; i++)
                Destroy(DestroyPieces[i]);

            transform.parent = null;
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).parent = null;

            GetComponent<BoxCollider>().enabled = true;
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			GetComponent<Rigidbody>().AddForce(new Vector3(1,5,0), ForceMode.Impulse);
			destroyed = true;
            Destroy(gameObject, Random.Range(8, 10));
		}

	}
}
