using System;
using UnityEngine;

namespace TicTacToe.Scripts
{
    public class ClickEvent : MonoBehaviour
    {
        public delegate void OnClickedPosition(Collider other);
        public static event OnClickedPosition OnClicked;

        private bool _wasClicked = false;
        private void OnMouseDown()
        {
            if (_wasClicked) return;
            
            try
            {
                if (!TryGetComponent<Collider>(out var other)) return;

                OnClicked?.Invoke(other);
                _wasClicked = true;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void StartNewGame()
        {
            _wasClicked = false;
        }
    }
}
