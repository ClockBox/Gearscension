using UnityEngine;

public class Gun : Weapon
{
    [Header("Normal Bullet")]
    public GameObject NormalBullet;
    public ParticleSystem NormalParticles;

    [Space (20)]

    [Header("Normal Bullet")]
    public GameObject IceBullet;
    public ParticleSystem IceParticles;

    [Space(20)]

    [Header("Exploding Bullet")]
    public GameObject ExplodingBullet;
    public ParticleSystem ExplosionParticles;

    [Space(20)]

    [Header("Electric Bullet")]
    public GameObject ElectricBullet;
    public ParticleSystem ElectricParticles;

    [Space(20)]

    [Header("Magnetic Bullet")]
    public GameObject MagneticBullet;
    public ParticleSystem MagneticParticles;

    Transform bulletSpawn;

    public override void Start()
    {
        base.Start();
        bulletSpawn = transform.GetChild(0);
    }
	
	void Shoot (float BulletScale)
    {
        GameObject bullet = Instantiate(NormalBullet, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        bullet.transform.localScale = bullet.transform.localScale * BulletScale;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 50/ BulletScale, ForceMode.Impulse);
        bullet.tag = "Player";
        Destroy(bullet, 3);
	}
}
