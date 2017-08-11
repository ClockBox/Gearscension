using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecisions : ScriptableObject {

	public abstract bool Decide(AIStateManager manager);
}
