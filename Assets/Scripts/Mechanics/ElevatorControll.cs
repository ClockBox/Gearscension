using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorControll : MonoBehaviour
{
    public Animator[] WallSections;
    public float offset;

    public void Activate()
    {
        StartCoroutine(MoveWalls());
    }

    private IEnumerator MoveWalls()
    {
        for (int i = 0; i < WallSections.Length; i++)
        {
            WallSections[i].SetTrigger("Move");
            yield return new WaitForSeconds(offset);
        }
    }
}
