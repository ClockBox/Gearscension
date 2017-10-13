using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushableObject : MonoBehaviour
{
    StateManager stateManager;
    LightPuzzle lp;
    public UnityEvent myEvent;
    [SerializeField]
    private bool useEvent;
    float moveSpeed;

    private void Start()
    {
        stateManager = FindObjectOfType<StateManager>();
        lp = FindObjectOfType<LightPuzzle>();
        moveSpeed = 10;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "PressurePlate")
        {
            Vector3 toFrom = collider.transform.position - transform.position;
            
            StartCoroutine(MoveOverTime(collider, toFrom));

            stateManager.ChangeState(new UnequipedState(stateManager, true));

            lp.CheckPuzzlePiece(collider.gameObject);
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "PressurePlate")
        {
            lp.DisengagePuzzlePiece(collider.gameObject);
        }
    }

    private IEnumerator MoveOverTime(Collider col, Vector3 _toFrom)
    {
        while (_toFrom.magnitude > 0.01f)
        {
            transform.Translate(_toFrom * Time.deltaTime * moveSpeed);
            _toFrom = col.transform.position - transform.position;
            yield return null;
        }

        Vector3 endPos = (col.transform.position - new Vector3(0, 0.1f, 0)) - transform.position;
        Debug.Log(endPos.magnitude);

        while (endPos.magnitude > 0.01f)
        {
            transform.Translate(endPos * Time.deltaTime * 2f);
            endPos = (col.transform.position - new Vector3(0, 0.1f, 0)) - transform.position;
            yield return null;
        }

        if (useEvent)
            myEvent.Invoke();
    }
}
