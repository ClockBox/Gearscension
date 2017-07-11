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
    public float effectDuration = 3;

	void Start ()
    {
        Destroy(gameObject, 3);
	}
    private void OnCollisionEnter(Collision other)
    {
        SpawnEffectArea(effectDuration).transform.parent = other.transform;
        Destroy(gameObject);
    }

    GameObject SpawnEffectArea(float duration)
    {
        GameObject newEffect = Instantiate(effectPrefab, transform.position, transform.rotation);
        newEffect.transform.localScale = transform.localScale;
        Destroy(newEffect, duration);
        return newEffect;
    }
}
