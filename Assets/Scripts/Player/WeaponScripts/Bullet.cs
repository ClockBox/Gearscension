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
        if (other.collider.attachedRigidbody)
        {
            if (other.collider.attachedRigidbody.gameObject.CompareTag("Enemy"))
                SpawnEffectArea();
        }
        else SpawnEffectArea(other.transform);
        Destroy(gameObject);
    }

    void SpawnEffectArea(Transform parent = null)
    {
        GameObject temp = Instantiate(effectPrefab, transform.position, transform.rotation);
        temp.transform.localScale = Vector3.one;
        if (parent) temp.transform.parent = parent;
    }
}
