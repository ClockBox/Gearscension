using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIActions : ScriptableObject {

	public abstract void Act(AIStateManager manager);
}
