using System;
using System.Collections;
using System.Collections.Generic;
using TicTacToe.Scripts;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace TicTacToe.Scripts
{
    public class Gameplay : MonoBehaviour
    { 
        public static bool IsGameFinished { get; set; }
        [SerializeField] private GameObject letterX;
        [SerializeField] private GameObject letterO;
        [SerializeField] private GameObject originalBoard;
        private GameObject _board;
        private const int BoardSize = 3; 
        private int[,] _mat;
        private int _round = 0;
        private bool _playerTurn; //True 1st player, False 2nd player turn
        private bool _firstRun;

        TtoElement _lastClickedElement;
        //TicTacToe Element
        public struct TtoElement
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public int Value { get; set; }

            public TtoElement(int row, int col, int value)
            {
                Row = row;
                Col = col;
                Value = value;
            }
        }

        private void Restart()
        {
            Debug.Log(_firstRun? "Started" : "Restarted");
            _mat = new int[BoardSize, BoardSize];
            _playerTurn = true;
            _round = 0;
            IsGameFinished = false;

            if (!_firstRun)
            {
                if (null == letterX)
                {
                    letterX = GameObject.FindGameObjectWithTag("X");
                }
                Debug.Assert(null != letterX, "null == letterX");

                if (null == letterO)
                {
                    letterX = GameObject.FindGameObjectWithTag("O");
                }
                Debug.Assert(null != letterO, "null == letterO");

                if (null == originalBoard)
                {
                    originalBoard = GameObject.FindGameObjectWithTag("Board");
                }
                Debug.Assert(null != letterO, "null == letterO");
                
                Destroy(_board);
                _board = Instantiate(originalBoard, new Vector3(0, 1, 0), Quaternion.identity);
            }

            _firstRun = false;
        }
        
        private void Awake()
        {
            _firstRun = true;
            Restart();
        }

        void Start()
        {
            if (null == letterX)
            {
                letterX = GameObject.FindGameObjectWithTag("X");
            }
            Debug.Assert(null != letterX, "null == letterX");

            if (null == letterO)
            {
                letterX = GameObject.FindGameObjectWithTag("O");
            }
            Debug.Assert(null != letterO, "null == letterO");

            if (null == originalBoard)
            {
                originalBoard = GameObject.FindGameObjectWithTag("Board");
            }
            Debug.Assert(null != letterO, "null == letterO");
            
            _board = Instantiate(originalBoard, new Vector3(0, 1, 0), Quaternion.identity);
        }

        private void OnEnable()
        {  
            ClickEvent.OnClicked += OnClicked;
            GameManager.Restart += Restart;
        }

        private void OnDisable()
        {
            ClickEvent.OnClicked -= OnClicked;
            GameManager.Restart -= Restart;
        }

        void OnClicked(Collider other)
        {
            ++_round;
            // Debug.Log("Clicked on " + other.name);
            int i = Int32.Parse(other.name[1].ToString());
            int j = Int32.Parse(other.name[2].ToString());
            
            _mat[i,j] = _playerTurn ? 1 : 2;
            
            // Debug.Log("_mat[" + i + "," + j + "] = 1");
            if (!other.CompareTag($"TicTacToeClick")) return;

            Transform position = _board.transform;//other.gameObject.transform;
            GameObject whatToInstantiate = _playerTurn ? letterX : letterO;

            // Instantiate(whatToInstantiate, position, false);
            Instantiate(whatToInstantiate, position, _board);

            _lastClickedElement = new TtoElement(i, j, _playerTurn ? 1 : 2);
            if (CheckIfWeHaveWinner())
            {
                IsGameFinished = true;
                Debug.Log("Winner is" + (_playerTurn ? 1 : 2));
            }
            
            _playerTurn = !_playerTurn;
        }
        private bool CheckIfWeHaveWinner()
        {
            int minimumRoundsToWin = (BoardSize - 1) * 2; 
            if (_round <= minimumRoundsToWin)
            {
                return false;
            }


            bool rowCondition = true, colCondition = true;
            // int specialConditionSwitch = 0;
            // bool specialCondition = true;
            bool specialCondition1 = false;
            bool specialCondition2 = false;
            bool specialCondition3 = false;

            /* Special Condition refers to \ or /.
            ** 1 -> means \
             * 2 -> means /
             * 3 -> means / and \
             */
            if (_lastClickedElement.Row == _lastClickedElement.Col)
                specialCondition1 = true;
            if (_lastClickedElement.Row + _lastClickedElement.Col == BoardSize - 1)
            {
                specialCondition2 = true;
                if (specialCondition1)
                    specialCondition3 = true;
            }

            for (int i = 0; i < BoardSize; ++i)
            {
                if (rowCondition)
                {
                    if (_mat[_lastClickedElement.Row, i] != _lastClickedElement.Value)
                        rowCondition = false;
                }

                if (colCondition)
                {
                    if (_mat[i, _lastClickedElement.Col] != _lastClickedElement.Value)
                        colCondition = false;
                }
                
                if (specialCondition1)
                {
                    if (_mat[i, i] != _lastClickedElement.Value)
                    {
                        specialCondition1 = false;
                    }
                }

                int j = BoardSize - i - 1;
                if (specialCondition2)
                {
                    if (_mat[i, j] != _lastClickedElement.Value)
                    {
                        specialCondition2 = false;
                    }
                }

                if (specialCondition3)
                {
                    if (_mat[i, i] != _lastClickedElement.Value || _mat[i, j] != _lastClickedElement.Value)
                    {
                        specialCondition3 = false;
                    }
                }
            }

            // Not used
            // switch (condition)
            // {
            //     case 1:
            //         specialCondition = CheckSpecialCondition1();
            //         break;
            //     case 2:
            //         specialCondition = CheckSpecialCondition2();
            //         break;
            //     case 3:
            //         specialCondition = CheckSpecialCondition1();
            //         specialCondition = CheckSpecialCondition2()  || specialCondition ? true : false;
            //         break;
            //     default:
            //         Debug.Log("Special Condition went wrong");
            //         break;
            // }

            return rowCondition || colCondition || specialCondition1 || specialCondition2 || specialCondition3;
        }

        // Not used
        // private bool CheckSpecialCondition1()
        // {
        //     
        //     for (int i = 0; i < BoardSize; ++i)
        //     {
        //         if (_mat[i, i] != lastPlacedElement.Value)
        //         {
        //             return false;
        //         }
        //     }
        //
        //     return true;
        // }
        //
        // private bool CheckSpecialCondition2()
        // {
        //     for (int i = 0; i < BoardSize; ++i)
        //     {
        //         int j = BoardSize - i + 1;
        //         if (_mat[i, j] != lastPlacedElement.Value)
        //         {
        //             return false;
        //         }
        //     }
        //
        //     return true;
        // }
    }
}
