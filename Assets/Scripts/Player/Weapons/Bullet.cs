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
        SpawnEffectArea().transform.parent = other.transform;
        if(other.gameObject.tag=="Enemy")
        Destroy(gameObject);
    }

    GameObject SpawnEffectArea()
    {
        GameObject newEffect = Instantiate(effectPrefab, transform.position, transform.rotation);
        newEffect.transform.localScale = transform.localScale;
        return newEffect;
    }
}
