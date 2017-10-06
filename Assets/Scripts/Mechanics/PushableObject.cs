using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    StateManager stateManager;
    LightPuzzle lp;
    private void Start()
    {
        stateManager = FindObjectOfType<StateManager>();
        lp = FindObjectOfType<LightPuzzle>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "PressurePlate")
        {
            Vector3 toFrom = collider.transform.position - transform.position;

            while (toFrom.magnitude > 0.01f)
            {
                transform.Translate(toFrom * Time.deltaTime);
                toFrom = collider.transform.position - transform.position;
            }
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
}
