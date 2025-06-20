using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HelloWorld.Utils
{
    public class PauseManager : Singleton<PauseManager>
    {
        public enum PauseState
        {
            Game,
            Paused,
            Screen
        }

        public PauseState currentPauseState { get; private set; } = PauseState.Game;

        public static event Action<PauseState> OnGameStateChanged;

        public void SetPauseState(PauseState newState)
        {
            if (currentPauseState == newState) return;
            currentPauseState = newState;
            OnGameStateChanged?.Invoke(currentPauseState);
        }
    }
}
