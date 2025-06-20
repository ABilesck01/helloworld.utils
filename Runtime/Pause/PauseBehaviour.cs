using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HelloWorld.Utils
{
    public class PauseBehaviour : MonoBehaviour
    {
        public Action OnGame;
        public Action OnPause;

        private void OnEnable()
        {
            PauseManager.OnGameStateChanged += HandlePauseStateChanged;
        }

        private void OnDisable()
        {
            PauseManager.OnGameStateChanged -= HandlePauseStateChanged;
        }

        private void HandlePauseStateChanged(PauseManager.PauseState newState)
        {
            switch (newState)
            {
                case PauseManager.PauseState.Game:
                    OnGame?.Invoke();
                    break;
                case PauseManager.PauseState.Screen:
                case PauseManager.PauseState.Paused:
                    OnPause?.Invoke();
                    break;
            }
        }
    }
}
