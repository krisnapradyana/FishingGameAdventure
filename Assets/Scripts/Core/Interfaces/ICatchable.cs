using UnityEngine;

public interface ICatchable
{
    float PullResistence { get; }
    float StruggleStrength { get; }

    float CatchDifficulty { get; set; }
    float Stamina { get; set; }
    void OnHook();
    void OnCaptured();
    void ReelIn(float reelSpeed);
    void ReduceStamina(float amount);
}
