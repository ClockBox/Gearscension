using UnityEngine;

public class Gun : Weapon
{
    public Rigidbody[] Bullet = new Rigidbody[4];

    Transform bulletSpawn;
    PlayerController player;

    public override void Start()
    {
        base.Start();
        bulletSpawn = transform.GetChild(0);
        player = GameManager.Player;
        if (player.GunUpgrades < 0)
            gameObject.SetActive(false);
    }
	
	public void Shoot ()
    {
        Rigidbody bullet = Instantiate(Bullet[player.AmmoType], bulletSpawn.position, bulletSpawn.rotation);
        bullet.AddForce(bullet.transform.forward * 100 * bullet.mass, ForceMode.Impulse);
    }
}
