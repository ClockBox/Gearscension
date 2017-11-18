using UnityEngine;

public class Gun : Weapon
{
    public GameObject[] Bullet = new GameObject[4];

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
        GameObject bullet = Instantiate(Bullet[player.AmmoType], bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward * 100), ForceMode.Impulse);
    }
}
