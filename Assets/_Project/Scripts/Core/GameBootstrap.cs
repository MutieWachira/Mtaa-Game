using UnityEngine;

/// <summary>
/// Initializes the core runtime systems required by Mtaa
/// </summary>
public sealed class GameBootstrap : MonoBehaviour
{
    private GameState _currentState = GameState.Booting;
   private void Awake()
   {
    Debug.Log("[GameBootstrap] Mtaa initialization started.");
    }

    private void Start()
    {
        SetState(GameState.MainMenu);
    }
    ///<summary>
    /// Changes the current high-level game state
    /// </summary>
    /// <param name="newState">The new state to transition to</param>
    private void SetState(GameState newState)
    {
        _currentState = newState;
        Debug.Log($"[GameBootstrap] Game state changed to: {_currentState}");
    }
}

