using UnityEngine;

public enum BulletType
{
    Electric,
    Ice,
    Explosive,
    Magnetic
}

public class Bullet : MonoBehaviour
{
    public BulletType type;
    public GameObject effectPrefab;

	void Start ()
    {
        Destroy(gameObject, 4);
	}
    private void OnCollisionEnter(Collision other)
    {
        Instantiate(effectPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
