using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateTransitions : StateTransitions
{
    PlayerController Player;

	protected override void Start ()
    {
        //Unequiped
        Transitions["UnequipedState"]["CombatState"] = UnequipedToCombat;
        Transitions["UnequipedState"]["AimState"] = UnequipedToAiming;
        Transitions["UnequipedState"]["ClimbingState"] = UnequipedToClimbing;
        Transitions["UnequipedState"]["CarrysState"] = UnequipedToCarrying;
        Transitions["UnequipedState"]["PushState"] = UnequipedToPushing;

        // Pushing
        Transitions["PushState"]["UnequipedState"] = PushingToUnequiped;

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
    private IEnumerator UnequipedToCombat(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }
    private IEnumerator UnequipedToAiming(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }
    private IEnumerator UnequipedToClimbing(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }
    private IEnumerator UnequipedToCarrying(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }
    private IEnumerator UnequipedToPushing(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }

    // Pushing
    private IEnumerator PushingToUnequiped(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }

    // Combat
    private IEnumerator CombatToUnequiped(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }
    private IEnumerator CombatToAiming(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }
    private IEnumerator CombatToClimbing(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }

    // Aiming
    private IEnumerator AimingToUnequiped(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }

    // Climbing
    private IEnumerator ClimbingToUnequiped(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }
    private IEnumerator ClimbingToCombat(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }

    // Carrying
    private IEnumerator CarryingToUnequiped(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }
    private IEnumerator CarryingToCombat(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }
}
