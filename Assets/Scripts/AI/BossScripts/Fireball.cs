using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour 
{
    [SerializeField]
    private GameObject fireRing;
    private void Update()
    {
        Vector3 direction = GetComponent<Rigidbody>().velocity.normalized;
        transform.LookAt(transform.position + direction);
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject == GameManager.Player)
            GameManager.Player.TakeDamage(20);


		GameObject fr = Instantiate(fireRing, transform.position, new Quaternion(0, 0, 0, 0));

        GameManager.Instance.AudioManager.playAudio(fr.GetComponent<AudioSource>(), "sfxgunimpactexplosion");

        Destroy(fr.gameObject, 2.1f);
		Destroy(gameObject);
	}	 
}
