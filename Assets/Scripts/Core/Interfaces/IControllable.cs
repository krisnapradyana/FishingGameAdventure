using UnityEngine;

public interface IControllable
{
    InputSystem_Actions Controller { get; }
    Animator CharacterAnimator { get; }
    void InitializeControl();
}
