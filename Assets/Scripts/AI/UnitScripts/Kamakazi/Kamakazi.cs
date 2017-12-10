using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamakazi : AIStateManager
{
    private Animator anim;

    public GameObject effectPrefab;
    public GameObject BrokenPrefab;

	public float maxHeight;
	public float force;
	public float proximity;
	private Vector3 playerPos;
	private GameObject smoker;

	public override void StartEvents()
    {
        anim = GetComponent<Animator>();
        if (smoker)
        {
            smoker = Instantiate(smokePrefab, exhaust.transform.position, transform.rotation);
            smoker.transform.parent = transform;
        }
	}

	public override void MeleeAttack()
	{
	}

	public override void RangedAttack()
	{
		if (!callOnce)
		{
			pathAgent.angularSpeed = 0;
			pathAgent.enabled = false;
			callOnce = true;
            if (smoker) smoker.GetComponent<ParticleSystem>().Play();
            pathAgent.enabled = false;
            StartCoroutine(LaunchExplode(1f));
        }
	}

    public override void Update()
    {
        if (pathAgent)
        {
            base.Update();
            anim.SetFloat("Speed", (rb.velocity.magnitude > 0.1f || rb.angularVelocity.magnitude > 0.1f) ? 1 : 0);
        }
    }

    IEnumerator LaunchExplode(float delayTimer)
    {
        anim.SetTrigger("Jump");
        anim.SetBool("Grounded", false);

        float startHeight = transform.position.y;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        while (transform.position.y <= startHeight + maxHeight)
        {
            rb.AddForce(transform.up * force);
            yield return null;
        }

        yield return new WaitForSeconds(delayTimer);

        rb.velocity = Vector3.zero;
        playerPos = player.transform.position;
        playerPos = new Vector3(playerPos.x, playerPos.y - 2, playerPos.z);
        if (smoker) smoker.GetComponent<ParticleSystem>().Play();
        Vector3 target = playerPos - transform.position + Vector3.up * 1.5f;
		rb.AddForce(target, ForceMode.Impulse);
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (callOnce && collision.collider.CompareTag("Player"))
        {
            GameManager.Player.TakeDamageDirect(20);
            Explode();
            Destroy(gameObject);
        }
        else if(callOnce && collision.collider.CompareTag("Breakable"))
        {
            Explode();
            Destroy(gameObject);
        }
        else if (Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.5f)
        {
            anim.SetBool("Grounded", true);
            pathAgent.enabled = true;
        }
	}

	public void Explode()
    {
        Instantiate(BrokenPrefab, transform.position, transform.rotation);
        GameObject newEffect = Instantiate(effectPrefab, transform.position, transform.rotation);
        newEffect.transform.localScale = transform.localScale;
	}
}
