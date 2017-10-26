using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Electric,
    Ice

}
public class EffectArea : MonoBehaviour
{
    public EffectType type;
    public float lifeTime;
    public float effectRadius;

    private float elapsedTime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);

        if (elapsedTime >= 0.5f)
        {
            Collider[] cols;
            cols = Physics.OverlapSphere(transform.position, effectRadius, LayerMask.GetMask("Debris", "Character"));
            for (int i = 0; i < cols.Length; i++)
            {
                GameObject TestObject = cols[i].gameObject;
                if (type == EffectType.Ice)
                {
                    if (TestObject.CompareTag("Enemy") || TestObject.CompareTag("Water"))
                    {
                        Debug.Log("PK FREEZE!!!");
                        TestObject.GetComponent<Freezable>().Freeze = true;
                    }
                }
                else if (type == EffectType.Electric)
                {
                    if (TestObject.CompareTag("Generator"))
                    {
                        Debug.Log("POWER");
                        TestObject.GetComponent<Generator>().generate();
                    }
                }
            }
        }
        else elapsedTime += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (type == EffectType.Ice)
            Gizmos.color = Color.blue;
        else
            Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}
