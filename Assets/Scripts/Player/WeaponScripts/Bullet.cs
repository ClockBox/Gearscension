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
    public AudioClip OnHitClip;

    void Start ()
    {
        Destroy(gameObject, 4);
	}
    private void OnCollisionEnter(Collision other)
    {
        GameObject temp = Instantiate(effectPrefab, transform.position, transform.rotation);
        GameManager.Instance.AudioManager.playAudio(temp.GetComponent<AudioSource>(), OnHitClip);
        Destroy(gameObject);
    }
}
