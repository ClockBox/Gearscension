using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrystals : MonoBehaviour {
	public GameObject _crystal;
	[SerializeField]
	private Grim boss;
	public bool isExposed = false;
	[SerializeField]
	private float resetTimer;

	private float elapsed;
	// 0 is not affected, 1 is electric, 2 is ice
	public BulletType effectState;

	//private void Start()
	//{
	//	elapsed = resetTimer;
	//}
	//private void OnTriggerEnter(Collider other)
	//{
	//	if (isExposed)
	//	{
	//		if (other.gameObject.CompareTag("Sword"))
	//		{
	//			boss.CrystalDestroy(this);
	//		}

	//	}


	//}
	//private void OnCollisionEnter(Collision collision)
	//{
		
	//		if (collision.gameObject.GetComponent<Bullet>())
	//		{
	//			effectState = collision.gameObject.GetComponent<Bullet>().type;
	//			boss.CrystalHit();
	//			StopAllCoroutines();
	//			elapsed = resetTimer;
	//			StartCoroutine(Reset());
	//		}

		
	

	//}

	//private IEnumerator Reset()
	//{
	//	while (elapsed >=0)
	//	{
	//		elapsed -= Time.deltaTime;
	//		yield return null;
	//	}
	//	elapsed = resetTimer;
	//	effectState = BulletType.Magnetic;
	//	yield break;
	//}

}
