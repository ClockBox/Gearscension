using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PressurePlate : ElectricalSwitch
{
    public GameObject weightedObject;
    public UnityEvent OnActiveStay;
    
    private LightPuzzle lp;

    private void Awake()
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
        if (other.attachedRigidbody.gameObject == weightedObject)
            return;

        if (other.CompareTag("Player"))
        {
            weightedObject = other.attachedRigidbody.gameObject;
            Active = true;
        }

        else if (other.CompareTag("Freezable"))
        {
            weightedObject = other.attachedRigidbody.gameObject;
            Active = true;
            StartCoroutine(MoveOverTime(other.transform.position, transform.position));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!weightedObject || other.attachedRigidbody.gameObject == weightedObject.gameObject)
        {
            weightedObject = null;
            Active = false;
        }
    }

    private IEnumerator MoveOverTime(Vector3 fromPoint, Vector3 toPoint)
    {
        float startY = fromPoint.y;
        float moveSpeed = 1;
        float elapsedTime = 0;

        fromPoint.y = 0;
        toPoint.y = 0;

        while (elapsedTime < moveSpeed && weightedObject)
        {
            weightedObject.transform.position = Vector3.Lerp(fromPoint, toPoint, elapsedTime) + Vector3.up * startY;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}