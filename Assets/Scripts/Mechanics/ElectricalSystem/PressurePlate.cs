using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PressurePlate : ElectricalSwitch
{
    private List<GameObject> weightedObject = new List<GameObject>();
    public UnityEvent OnActiveStay;
    
    private LightPuzzle lp;

    public virtual void Awake()
    {
        lp = FindObjectOfType<LightPuzzle>();
    }

    public override void Activate()
    {
        base.Activate();
        if (lp) lp.CheckPuzzlePiece(gameObject);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (lp) lp.DisengagePuzzlePiece(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject refObject = other.attachedRigidbody.gameObject;

        if (weightedObject.Contains(refObject))
            return;

        if (other.CompareTag("Player"))
            weightedObject.Add(refObject);

        else if (other.CompareTag("Freezable"))
        {
            weightedObject.Add(refObject);
            StartCoroutine(MoveOverTime(refObject, refObject.transform.position, transform.position));
        }

        if(weightedObject.Count == 1)
            Active = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject refObject = other.attachedRigidbody.gameObject;

        if (weightedObject.Contains(refObject))
            weightedObject.Remove(refObject);

        if(weightedObject.Count == 0)
            Active = false;
    }

    private IEnumerator MoveOverTime(GameObject refObject,Vector3 fromPoint, Vector3 toPoint)
    {
        PlayerController.StateM.ChangeState(new UnequipedState(PlayerController.StateM, true));
        if (!active)
        {
            float startY = fromPoint.y;
            float moveSpeed = 1;
            float elapsedTime = 0;

            fromPoint.y = 0;
            toPoint.y = 0;

            while (elapsedTime < moveSpeed && refObject)
            {
                refObject.transform.position = Vector3.Lerp(fromPoint, toPoint, elapsedTime) + Vector3.up * startY;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}