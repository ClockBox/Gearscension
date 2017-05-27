using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DropdownRes : MonoBehaviour
{

     List<string> size = new List<string>() { "Set Resolution", "1600 x 900", "1280 x 720", "1024 x 768", "800 x 600" };

    // Other
    public Dropdown dropDown;
    public Text selectedSize;

    public void DropDown_IndexChanger(int index)
    {
         selectedSize.text = "(" + size[index] + ")";
        if(index == 0)
        {
            selectedSize.text = "(Default)";
            selectedSize.color = Color.red;
        }
        else if(index == 1)
        {
            Screen.SetResolution(1600, 900, false);
            selectedSize.text = "(" + size[index] + ")";
            selectedSize.color = Color.white;
        }
        else if (index == 2)
        {
            Screen.SetResolution(1280, 720, false);
            selectedSize.text = "(" + size[index] + ")";
            selectedSize.color = Color.white;
        }
        else if (index == 3)
        {
            Screen.SetResolution(1024, 768, false);
            selectedSize.text = "(" + size[index] + ")";
            selectedSize.color = Color.white;
        }
        else if (index == 4)
        {
            Screen.SetResolution(800, 600, false);
            selectedSize.text = "(" + size[index] + ")";
            selectedSize.color = Color.white;
        }
        else
        {
             selectedSize.text = "(" + size[index] + ")";
        }
        // Screen.SetResolution = size[index]
    }

    void Start()
    {
        PopulateList();
    }

	void Update()
    {
        
    }
    	
    void PopulateList()
    {
        dropDown.AddOptions(size);
    }

}
