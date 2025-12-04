using UnityEngine;

public class Player : PlayableCharacter
{
    public Player() : base(
        name: "Player",
        description: "The main playable character controlled by the user."
    )
    { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        Controller = new InputSystem_Actions();
        Controller.Player.Enable();

        InitializeControl();
        InitializePlayerEquipment();

        GameManager.Instance.OnGameStateChanged += () =>
        {
            if (GameManager.Instance.CurrentGameState == GameStates.fishing)
            {
                Debug.Log("Player has entered fishing state.");
                // Additional logic for entering fishing state can be added here
                Controller.Player.Disable();
                Controller.Fishing.Enable();
            }
            else if (GameManager.Instance.CurrentGameState == GameStates.exploration)
            {
                Debug.Log("Player has entered exploration state.");
                // Additional logic for entering exploration state can be added here
                Controller.Player.Enable();
                Controller.Fishing.Disable();
            }
        };
    }

    protected override void Update()
    {
        base.Update();
    }

    void InitializePlayerEquipment()
    {
        if (fishingRod == null)
        {
            Debug.LogWarning("Player has no fishing rod assigned.");
            return;
        }
        fishingRod.EquipLure(lure);
    }
}
