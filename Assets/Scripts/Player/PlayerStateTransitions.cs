using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateTransitions : StateTransitions
{
	protected override void Start ()
    {
        //Unequiped
        Transitions["UnequipedState"]["CombatState"] = UnequipedToCombat;
        Transitions["UnequipedState"]["AimState"] = UnequipedToAiming;
        Transitions["UnequipedState"]["ClimbingState"] = UnequipedToClimbing;
        Transitions["UnequipedState"]["CarrysState"] = UnequipedToCarrying;
        Transitions["UnequipedState"]["PushState"] = UnequipedToPushing;

        // Pushing
        Transitions["PushState"]["UnequipedState"] = PushingToWalking;

        // Combat
        Transitions["CombatState"]["UnequipedState"] = CombatToUnequiped;
        Transitions["CombatState"]["AimState"] = CombatToAiming;

        // Aiming
        Transitions["AimState"]["UnequipedState"] = AimingToUnequiped;

        // Climbing
        Transitions["ClimbState"]["UnequipedState"] = ClimbingToUnequiped;
        Transitions["ClimbState"]["CombatState"] = ClimbingToCombat;

        // Carrying
        Transitions["CarryState"]["UnequipedState"] = CarryingToUnequiped;
        Transitions["CarryState"]["CombatState"] = CarryingToCombat;
    }

    //Unequiped
    private IEnumerator UnequipedToCombat()
    {
        yield return null;
    }
    private IEnumerator UnequipedToAiming()
    {
        yield return null;
    }
    private IEnumerator UnequipedToClimbing()
    {
        yield return null;
    }
    private IEnumerator UnequipedToCarrying()
    {
        yield return null;
    }
    private IEnumerator UnequipedToPushing()
    {
        yield return null;
    }

    // Pushing
    private IEnumerator PushingToWalking()
    {
        yield return null;
    }

    // Combat
    private IEnumerator CombatToUnequiped()
    {
        yield return null;
    }
    private IEnumerator CombatToAiming()
    {
        yield return null;
    }

    // Aiming
    private IEnumerator AimingToUnequiped()
    {
        yield return null;
    }

    // Climbing
    private IEnumerator ClimbingToUnequiped()
    {
        yield return null;
    }
    private IEnumerator ClimbingToCombat()
    {
        yield return null;
    }

    // Carrying
    private IEnumerator CarryingToUnequiped()
    {
        yield return null;
    }
    private IEnumerator CarryingToCombat()
    {
        yield return null;
    }
}
