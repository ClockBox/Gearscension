using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrystals : MonoBehaviour {
	[SerializeField]
	private Grim boss;
	public bool isExposed = false;
	[SerializeField]
	private float resetTimer;

	private float elapsed;
	// 0 is not affected, 1 is electric, 2 is ice
	public BulletType effectState;

	private void Start()
	{
		elapsed = resetTimer;
	} 
	//private void OnTriggerEnter(Collider other)
	//{
	//	if (!isExposed)
	//	{
	//		if (other.gameObject.GetComponent<Bullet>())
	//		{
	//			effectState = other.gameObject.GetComponent<Bullet>().type;
	//			boss.CrystalHit();
	//			StopAllCoroutines();
	//			elapsed = resetTimer;
	//			StartCoroutine(Reset());
	//		}

	//	}
	//	else
	//	{
	//		if (other.gameObject.CompareTag("Projectile"))
	//		{
	//			boss.CrystalDestroy(this);
	//		}

	//	}

	//}
	private void OnCollisionEnter(Collision collision)
	{
		if (!isExposed)
		{
			if (collision.gameObject.GetComponent<Bullet>())
			{
				effectState = collision.gameObject.GetComponent<Bullet>().type;
				boss.CrystalHit();
				StopAllCoroutines();
				elapsed = resetTimer;
				StartCoroutine(Reset());
			}

		}
		else
		{
			if (collision.gameObject.CompareTag("Projectile"))
			{
				boss.CrystalDestroy(this);
			}

		}

	}

	private IEnumerator Reset()
	{
		while (elapsed >=0)
		{
			elapsed -= Time.deltaTime;
			yield return null;
		}
		elapsed = resetTimer;
		effectState = BulletType.Magnetic;
		yield break;
	}

}
