using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitCondition : Condition
{
    public BulletType type;

    private void Start()
    {
        switch (type)
        {
            case BulletType.Electric:

                break;
            case BulletType.Ice:
                break;
            case BulletType.Explosive:
                break;
            case BulletType.Magnetic:
                break;
            default:
                break;
        }
    }

    public override bool CheckCondition()
    {
        return conditionIsMet;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
            conditionIsMet = true;
    }
}
