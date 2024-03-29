﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour {

    // counting system
    [Serializable]
    public class Count {
        public int minimum;
        public int maximum;

        public Count(int min, int max) {
            if (min > max) {
                Debug.LogWarning("min bigger than max " + min + " " + max);
            }
            minimum = min;
            maximum = max;
        }
    }

    private float squaresPerInch = 6.0f;
    public int columns = 8;
    public int rows = 8;
    public bool drawMode = false;
    public GameObject[] floorTiles;
    public GameObject unitTile;
    public GameObject brickTile;
    public LinkedList<Vector2> SpawnSites;
    public GameObject pressureZoneTile;

    private Transform boardHolder;
    public float frequency = 0.5f;

    private List<Vector3> gridPositions = new List<Vector3>();
    private List<GameObject> pressureZones = new List<GameObject>();
    private List<GameObject> bricks = new List<GameObject>();

    public List<GameObject> GetPressureZones() => pressureZones;

    public List<GameObject> GetBricks() => bricks;

    public Vector2 GetBoardSize() {
        return new Vector2(columns, rows);
    }

    public void ResetBoardState() {
        ClearPressureZones();
        InstantiateSpawnSites(SpawnSites);
    }

    public void ClearBoard() {
        ClearPressureZones();
    }

    private void ClearPressureZones() {
        while (pressureZones.Count > 0) {
            GameObject zone = pressureZones[0];
            pressureZones.RemoveAt(0);
            Destroy(zone);
        }
    }

    public Vector2 BoardCenter() {
        return new Vector2(Mathf.Round(columns / 2), Mathf.Round(rows / 2));
    }

    public List<Vector3> GetGridPositions() {
        return gridPositions;
    }
    void InitializeList() {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++) {
            for (int y = 1; y < rows - 1; y++) {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    public void SetSpawnSites(LinkedList<Vector2> sites) {
        SpawnSites = sites;
    }

    public void RemovePressureZone(GameObject zone) {
        pressureZones.Remove(zone);
        Destroy(zone);
    }

    public void RemoveBrickZone(GameObject zone) {
        bricks.Remove(zone);
        Destroy(zone);
    }

    public bool PositionIsBrick(Vector2 position) {
        foreach(GameObject brick in bricks) {
            if (position == (Vector2)brick.transform.position) {
                return true;
            }
        }
        return false;
    }

    public bool PositionIsOnBoard(Vector2 position) {
        if (columns < position.x - 1) {
            return false;
        }
        if (0 > position.x + 1) {

            return false;
        }
        if (rows < position.y - 1) {

            return false;
        }
        if (0 > position.y + 1) {

            return false;
        }
        return true;
    }

    private bool IsTrackedZone(PressureZone zone) {
        return true;// (zone.GetType() != typeof(Brick));
    }

    public void AddPressureZone(GameObject zone) {

        // finish adding zone
        zone.transform.SetParent(boardHolder);
        if (IsTrackedZone(zone.GetComponent<PressureZone>())) {
            pressureZones.Add(zone);
        } else {
            bricks.Add(zone);
        }
    }

    public int GridPositionsLength() {
        return gridPositions.Count;
    }

    public void SetupScene(int level) {
        BoardSetup();
        InitializeList();

    }

    private void InstantiateSpawnSites(LinkedList<Vector2> sites) {

        LinkedListNode<Vector2> node = sites.First;

        while (node != null) {
            GameObject pzInstance = Instantiate(unitTile, new Vector3(node.Value.x, node.Value.y, 0f), Quaternion.identity) as GameObject;
            pzInstance.transform.SetParent(boardHolder);
            pressureZones.Add(pzInstance);
            node = node.Next;
        }
    }
    void BoardSetup() {
        float heightInInches = Screen.height / Screen.dpi;
        float widthInInches = Screen.width / Screen.dpi;
        columns = (int)Mathf.Floor(widthInInches * squaresPerInch);
        rows = (int)Mathf.Floor(heightInInches * squaresPerInch);
        boardHolder = new GameObject("Board").transform;
        InstantiateSpawnSites(SpawnSites);
    }

    void Awake() {

    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
