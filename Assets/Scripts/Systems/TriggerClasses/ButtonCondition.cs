using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    Down,
    Held,
    Up
}

public class ButtonCondition : Condition
{
    public string button;
    public InputType type;
    
    public override bool checkCondition()
    {
        if(type == InputType.Down)
            return conditionIsMet = Input.GetButtonDown(button);

        else if(type == InputType.Held)
            return conditionIsMet = Input.GetButton(button);

        else if(type == InputType.Up)
            return conditionIsMet = Input.GetButtonUp(button);

        else return false;
    }
}
