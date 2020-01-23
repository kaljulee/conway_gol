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
    public Unit unitTile;
    public LinkedList<Vector2> SpawnSites;
    public PressureZone PressureZoneClass;

    private Transform boardHolder;
    public float frequency = 0.5f;

    private List<Vector3> gridPositions = new List<Vector3>();
    private List<PressureZone> pressureZones = new List<PressureZone>();

    public List<PressureZone> GetPressureZones() => pressureZones;



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

    public void RemovePressureZone(PressureZone zone) {
    pressureZones.Remove(zone);
        Destroy(zone);
        
}
    public void AddPressureZone(PressureZone zone)
    {
        zone.transform.SetParent(boardHolder);
        pressureZones.Add(zone);
    }

    public int GridPositionsLength()
    {
        return gridPositions.Count;
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
        if (SpawnSites == null)
        {
            SpawnSites = new LinkedList<Vector2>();
            SpawnSites.AddFirst(new Vector2(2, 3));
            //SpawnSites.AddFirst(new Vector2(3, 3));
            SpawnSites.AddFirst(new Vector2(3, 4));
        }

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {

                // start by assuming random floor tile
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                LinkedListNode<Vector2> node = SpawnSites.First;
                // should only check till the first good value;
                while (node != null)
                {
                    if (node.Value.x == x && node.Value.y == y)
                    {
                        Unit pzInstance = Instantiate(unitTile, new Vector3(x, y, 0f), Quaternion.identity) as Unit;
                        pzInstance.transform.SetParent(boardHolder);
                        pressureZones.Add(pzInstance);
                        SpawnSites.Remove(node);
                        node = null;
                        
                    } else
                    {
                        node = node.Next;
                    }
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

    void Awake()
    {
           
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
