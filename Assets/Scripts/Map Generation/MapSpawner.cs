using UnityEngine;

namespace Map_Generation
{
    public class MapSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject cubeTile;
        [SerializeField] private GameObject wallTile;
        [SerializeField] private GameObject wallTileHor;
        [SerializeField] private int boardSize;
        void Start()
        {
            GameObject boardEmptyParent = new GameObject("Board");
            GameObject floorEmptyParent = new GameObject("Floors");
            floorEmptyParent.transform.SetParent(boardEmptyParent.transform);
            GameObject wallEmptyParent = new GameObject("Walls");
            wallEmptyParent.transform.SetParent(boardEmptyParent.transform);
            for (int i = 0; i < boardSize; ++i)
            {
                for (int j = 0; j < boardSize; ++j)
                {
                    GameObject temp = Instantiate(cubeTile, new Vector3(i,0, j), Quaternion.identity, floorEmptyParent.transform)
                        .transform.GetChild(0).gameObject;
                    temp.name = "M" + i + j;
                    if (i != 0)
                        Instantiate(wallTile, new Vector3(i,0, j), Quaternion.identity, wallEmptyParent.transform);
                    if (j != 0)
                        Instantiate(wallTileHor, new Vector3(i,0, j), Quaternion.identity, wallEmptyParent.transform);
                }
            }
            
            Destroy(gameObject);
        }
    }
}
