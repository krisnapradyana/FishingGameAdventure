public class CubeTrigger : Triggers
{
    public override void Interact()
    {
        base.Interact();    
        GameManager.Instance.ChangeGameState(GameStates.exploration);
    }
}
