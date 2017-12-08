using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggers : MonoBehaviour {
	private GameObject player;
	[SerializeField]
	private float radius;
	[SerializeField]
	private float force;
	[SerializeField]
	private float hitFrequency;
	[SerializeField]
	private float damage;
	private float counter;
	private float explodeTimer = 1f;
	private float magnetTimer = 1.5f;
	private delegate void hit();
	hit myHit;
	
	private enum TriggerType
	{
		Magnetic,
		BasicHit,
		Explosive
	}

	[SerializeField]
	private TriggerType myType;
	// Use this for initialization
	void Start () {
		counter = hitFrequency;
		if (myType == TriggerType.BasicHit)
			myHit = BasicHit;
		else if (myType == TriggerType.Magnetic)
			myHit = MagneticHit;
		else if (myType == TriggerType.Explosive)
			myHit = ExplosiveHit;

		GetComponent<Collider>().enabled = false;
			
	}
	
	//// Update is called once per frame
	//void Update () {
	//	if(Input.GetKeyDown (KeyCode.X))
	//	GetComponent<Collider>().enabled = !GetComponent<Collider>().enabled;
		
	//}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player"&&hitFrequency<=counter)
		{
			player = other.gameObject;
			counter = 0;
			myHit();
			other.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
			StartCoroutine(CountDown());			
		}

	}
	private IEnumerator CountDown()
	{
		while (counter < hitFrequency)
		{
			counter += Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	private void BasicHit()
	{
		Debug.Log("Basic");
	}
	private void MagneticHit() {
		StartCoroutine(MagnetField());
		Debug.Log("Magn");
	}
	private void ExplosiveHit() {
		StartCoroutine(ForceField(transform.position, player.transform.position));

		Debug.Log("Expood");
	}

	IEnumerator ForceField(Vector3 pos, Vector3 playerPos)
	{
		while (explodeTimer >= 0)
		{
			Vector3 direction = (playerPos-pos).normalized;
			direction = new Vector3(direction.x, Mathf.Abs(direction.y), direction.z );
			if (player)
			{
				player.GetComponent<Rigidbody>().AddForce(direction * force * Time.fixedDeltaTime);
				PlayerState.grounded = false;
			}
			explodeTimer -= Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();

		}
		explodeTimer =1f;
		yield break;
	}

	IEnumerator MagnetField()
	{
		while (magnetTimer >= 0)
		{
			if (player)
			{
				player.GetComponent<Rigidbody>().AddForce((transform.position - player.transform.position) * force * Time.fixedDeltaTime);
				PlayerState.grounded = false;
			}
			magnetTimer -= Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}
		magnetTimer = 1.5f;
		yield break;
	}

}
