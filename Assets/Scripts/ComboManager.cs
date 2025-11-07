using UnityEngine;

public static class ComboManager
{
    public static int CurrentCombo { get; private set; }
    public static int MaxCombo { get; private set; }

    public static void ResetCombo()
    {
        if (CurrentCombo > MaxCombo) MaxCombo = CurrentCombo;
        CurrentCombo = 0;
    }

    public static void IncrementPositive()
    {
        CurrentCombo++;
        if (CurrentCombo > MaxCombo) MaxCombo = CurrentCombo;
    }
}


