
public class PondTrigger : Triggers
{
    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.ChangeGameState(GameStates.fishing);
    }
}
