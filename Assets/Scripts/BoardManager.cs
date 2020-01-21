using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour
{

    // counting system
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            if (min > max)
            {
                Debug.LogWarning("min bigger than max " + min + " " + max);
            }
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public GameObject[] floorTiles;
    public GameObject[] unitTiles;
    public GameObject[] pressureZoneTiles;
    private Transform boardHolder;
    public float frequency = 0.5f;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();

    }

    void BoardSetup()
    {
        Console.WriteLine("setting up board with gridPositions " + gridPositions.Count);
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                // start by assuming random floor tile
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == 2 && (y == 3 || y == 2))
                {
                   toInstantiate = unitTiles[Random.Range(0, unitTiles.Length)];
                }
                if (x == 3 && y == 3)
                {
                    toInstantiate = pressureZoneTiles[Random.Range(0, pressureZoneTiles.Length)];
                }
                //// correct to outer wall tile if it is an edge tile
                //if (x == -1 || x == columns || y == -1 || y == rows)
                //{
                //    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                //}

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
