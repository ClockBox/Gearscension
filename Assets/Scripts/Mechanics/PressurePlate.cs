﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PressurePlate : ElectricalSwitch
{
    public GameObject weightedObject;
    public UnityEvent OnActiveStay;

    private float moveSpeed;
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
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            weightedObject = other.gameObject;
            Active = true;
            if (other.gameObject.CompareTag("Enemy"))
                StartCoroutine(MoveOverTime(other, transform.position - other.transform.position));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PushableObject>())
        {
            weightedObject = null;
            Active = false;
        }
    }

    private IEnumerator MoveOverTime(Collider col, Vector3 _toFrom)
    {
        while (_toFrom.magnitude > 0.01f)
        {
            weightedObject.transform.Translate(_toFrom * Time.deltaTime * moveSpeed);
            _toFrom = transform.position - col.transform.position;
            yield return null;
        }

        Vector3 endPos = (transform.position - new Vector3(0, 0.1f, 0)) - col.transform.position;
        Debug.Log(endPos.magnitude);

        while (endPos.magnitude > 0.01f)
        {
            weightedObject.transform.Translate(endPos * Time.deltaTime * 2f);
            endPos = (transform.position - new Vector3(0, 0.1f, 0)) - col.transform.position;
            yield return null;
        }
    }
}