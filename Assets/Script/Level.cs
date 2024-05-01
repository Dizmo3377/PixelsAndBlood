using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const int roomScale = 29; //Adjust to room prefab size

    private static Vector2Int[] scalars = { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down};

    private List<Room> enemyRooms = new List<Room>();
    private Room[,] grid = new Room[7,7];
    private Vector2Int middle;
    private int[] currentRoomCoordinates = new int[2];
    private bool highlightRoomsOnMinimap = true;
    private bool dontSpawnRoomsOnEdge = false;

    [Header("Settings")]
    [SerializeField] private bool bossRoom;
    [SerializeField] [Range(2, 5)] private int enemyRoomsAmount;
    [SerializeField] [Range(1, 5)] private int bonusRoomsAmount;
    [SerializeField] private GameObject[] bonusRoomsPrefabs;
    [SerializeField] private GameObject[] enemyRoomsPrefabs;
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject finishRoomPrefab;
    [SerializeField] private GameObject bossRoomPrefab;

    private void Start() 
    {
        middle = new Vector2Int((grid.GetLength(0) - 1) / 2, (grid.GetLength(1) - 1) / 2);

        if (LevelData.instance.lvl == 5) bossRoom = true;
        Generate();
    }

    private void LateUpdate()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.X)) Regenerate();
    }

    public static Vector2Int IntToScalar(int num) => scalars[num];
    private int BranchIdToRotation(int num) => (num + 2) % scalars.Length;
    private bool SafeOfEdgeSpawn(int x, int y) => !dontSpawnRoomsOnEdge || !IsOnMinimapEdge(x, y);
    private bool IsOnMinimapEdge(int x, int y) => Mathf.Abs(middle.x - x) > middle.x - 1 || Mathf.Abs(middle.y - y) > middle.x - 1;
    private bool HasFreeBranch(int x, int y) => scalars.Any(scalar => grid[x + scalar[0], y + scalar[1]] == null 
        && SafeOfEdgeSpawn(x + scalar[0], y + scalar[1]));

    private void Generate()
    {
        //Create Start Room and first enemy room

        int[] nextRoom = new int[3]; //nextRoom[0] - x; nextRoom[1] - y; nextRoom[2] - rotation
        CreateRoom(3, 3, 0, RoomType.Start);
        int[] newBranch = FindFreeBranchFor(3,3);
        CreateRoom(newBranch[0],newBranch[1], newBranch[2],  RoomType.Enemy);
        highlightRoomsOnMinimap = false; //We only want to highlight first 2 rooms, so set this to false

        //Create Other Rooms

        //Enemy Rooms
        dontSpawnRoomsOnEdge = true; //We dont want to spawn enemy rooms on the edge
        for (int i = 0; i < enemyRoomsAmount - 1; i++)
        {
            nextRoom = FindRoomSpace();
            CreateRoom(nextRoom[0], nextRoom[1], nextRoom[2], RoomType.Enemy);
        }
        dontSpawnRoomsOnEdge = false;

        //Bonus Rooms
        for (int i = 0; i < bonusRoomsAmount; i++)
        {
            nextRoom = FindRoomSpace();
            CreateRoom(nextRoom[0],nextRoom[1], nextRoom[2], RoomType.Bonus);
        }

        //Finish OR Boss Room
        nextRoom = FindRoomSpace();
        RoomType lastRoomType = bossRoom ? RoomType.Boss : RoomType.Finish;
        CreateRoom(nextRoom[0], nextRoom[1], nextRoom[2], lastRoomType);
    }

    private void CreateRoom(int x, int y, int rotation, RoomType roomType)
    {
        //Pick new room of a roomType
        GameObject newRoom = null;
        switch (roomType)
        {
            case RoomType.Start: newRoom = startRoomPrefab; break;
            case RoomType.Enemy: newRoom = enemyRoomsPrefabs.GetRandom(); break;
            case RoomType.Bonus: newRoom = bonusRoomsPrefabs.GetRandom(); break;
            case RoomType.Finish: newRoom = finishRoomPrefab; break;
            case RoomType.Boss: newRoom = bossRoomPrefab; break;
        }

        //Create New Room
        grid[x, y] = Instantiate(newRoom, new Vector3(x * roomScale, y * roomScale, 0), Quaternion.identity).GetComponent<Room>();
        grid[x, y].SetCoordinates(x,y);

        Minimap.LinkCellToRoom(x,y, roomType, highlightRoomsOnMinimap);

        currentRoomCoordinates[0] = x; 
        currentRoomCoordinates[1] = y;

        //Create new Branch
        if (roomType != RoomType.Start)
        {
            grid[x, y].CreateDoor(rotation);

            var scalar = IntToScalar(rotation);
            int branchId = (rotation + 2) % scalars.Length;
            grid[x + scalar.x, y + scalar.y].CreateBranch(branchId);

            if (highlightRoomsOnMinimap) Minimap.GetCell(x + scalar.x, y + scalar.y).SetBranch(branchId, true);
        }

        if (roomType == RoomType.Enemy) enemyRooms.Add(grid[x, y]);

        //Logging
        grid[x, y].gameObject.name = $"X: {x} || Y: {y}";
    }

    private void Regenerate()
    {
        foreach (Room room in grid)
        {
            if (room != null) Destroy(room.gameObject);
        }
        enemyRooms.Clear();
        currentRoomCoordinates[0] = 3;
        currentRoomCoordinates[1] = 3;
        Generate();
    }

    private int[] FindRoomSpace()
    {
        //We take random *enemy* room and check if it has free branch for creating another room
        int randomEnemyRoom;

        do { randomEnemyRoom = Random.Range(0, enemyRooms.Count); }
        while (!HasFreeBranch(enemyRooms[randomEnemyRoom].x, enemyRooms[randomEnemyRoom].y));

        currentRoomCoordinates[0] = enemyRooms[randomEnemyRoom].x;
        currentRoomCoordinates[1] = enemyRooms[randomEnemyRoom].y;

        return FindFreeBranchFor(currentRoomCoordinates[0], currentRoomCoordinates[1]);
    }

    private int[] FindFreeBranchFor(int x, int y)
    {
        int[] newCoordinates = new int[3];
        int randomBranch;
        Vector2Int newRoom;

        do
        {
            randomBranch = Random.Range(0, 4);
            newRoom = new Vector2Int(x, y) + IntToScalar(randomBranch);
        }
        while(grid[newRoom.x, newRoom.y] != null || !SafeOfEdgeSpawn(newRoom.x, newRoom.y));

        //Connect new room to previos room (Basically set proper rotation)
        (newCoordinates[0], newCoordinates[1]) = (newRoom[0], newRoom[1]);
        grid[currentRoomCoordinates[0], currentRoomCoordinates[1]].CreateDoor(randomBranch);
        newCoordinates[2] = BranchIdToRotation(randomBranch);
        return newCoordinates;
    }
}