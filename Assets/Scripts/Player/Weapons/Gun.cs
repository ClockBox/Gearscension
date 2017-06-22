using UnityEngine;

public class Gun : Weapon
{
    public GameObject[] Bullet = new GameObject[4];
    public ParticleSystem[] Particles = new ParticleSystem[4];

    Transform bulletSpawn;
    PlayerController player;

    public override void Start()
    {
        base.Start();
        bulletSpawn = transform.GetChild(0);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
	
	void Shoot (float BulletScale)
    {
        GameObject bullet = Instantiate(Bullet[player.AmmoType], bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        bullet.transform.localScale = bullet.transform.localScale * BulletScale;
        bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward * 100) / (BulletScale * BulletScale), ForceMode.Impulse);
        Destroy(bullet, 3);
	}
}
