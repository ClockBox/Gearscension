using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Bool
    private bool buttonSelected;

    // Other
    public EventSystem eventSystem;
    public GameObject selectedObject;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }


    public void LoadByIndex(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }

    // Use this for initialization
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}

