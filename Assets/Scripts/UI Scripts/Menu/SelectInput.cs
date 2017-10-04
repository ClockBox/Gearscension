using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SelectInput : MonoBehaviour
{
    // Bool
    private bool buttonSelected;

    // Other
    public EventSystem eventSystem;
    public GameObject selectedObject;
	
    void Awake()
    {

    }

	// Update is called once per frame
	void Update () 
    {
		if(Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
