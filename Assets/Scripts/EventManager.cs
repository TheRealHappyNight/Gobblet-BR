using UnityEngine;
using UnityEngine.EventSystems;

namespace TicTacToe.Scripts
{
    public class EventManager : MonoBehaviour, IPointerClickHandler
    {
        public delegate void ClickAction();
        public static event ClickAction OnClicked;

        private void OnMouseDown()
        {
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (null != OnClicked)
            {
                OnClicked();
            }   
        }
    }
}
