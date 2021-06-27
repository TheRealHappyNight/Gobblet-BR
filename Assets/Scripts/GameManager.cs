using System;
using System.Collections;
using System.Collections.Generic;
using TicTacToe.Scripts;
using UnityEngine;

namespace TicTacToe
{
    public class GameManager : MonoBehaviour
    {
        public delegate void OnRestart();
        public static event OnRestart Restart;

        private void Start()
        {
            
        }

        void Update()
        {
            if (Gameplay.IsGameFinished && Input.GetMouseButtonUp(0))
            {
                if (Restart != null) Restart();
            }
        }
    }
}