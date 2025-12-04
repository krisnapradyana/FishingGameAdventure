using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [field:SerializeField] public GameStates CurrentGameState { get; private set; } = GameStates.exploration;
    public Action OnGameStateChanged;

    private void Awake()
    {
        if (Instance)
        {
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void ChangeGameState(GameStates newState)
    {
        if (CurrentGameState == newState)
        {
            return;
        }
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke();
    }
}
