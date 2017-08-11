using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Electric,
    Ice

}
public class EffectArea : MonoBehaviour {

    public EffectType type;
    public float lifeTime;
    public float effectRadius;

    void Start()
    {
       
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        Collider[] cols;
        cols = Physics.OverlapSphere(transform.position, effectRadius);
        for (int i = 0; i < cols.Length; i++)
        {
            if (type == EffectType.Ice)
            {
                if (cols[i].gameObject.tag == "Water")
                {
                    Debug.Log("FREEZE");
                    cols[i].gameObject.GetComponent<WaterArea>().freeze();
                }
            }
            else if (type == EffectType.Electric)
            {
                if (cols[i].gameObject.tag == "Generator")
                {
                    Debug.Log("POWER");
                    cols[i].gameObject.GetComponent<Generator>().generate();
                }
            }

        }
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
