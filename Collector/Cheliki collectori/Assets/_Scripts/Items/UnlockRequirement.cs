using UnityEngine;

public abstract class UnlockRequirement : ScriptableObject
{
    public abstract bool IsMet(GameState state);
}

