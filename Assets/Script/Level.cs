using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const int scale = 29;

    private Room[,] grid = new Room[27,27];
    private List<Room> enemyRooms = new List<Room>();
    private static Vector2Int[] scalars = { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down};
    private int[] currentRoomCoordinates = new int[2];
    private bool highlightOnMinimap = true;

    [Header("Settings")]
    [SerializeField] [Range(2, 5)] private int enemyRoomsCount;
    [SerializeField] [Range(1, 5)] private int bonusRoomsCount;
    [SerializeField] private bool bossRoom; //Should not be SerializeField, but it is for debugging
    [SerializeField] private GameObject[] bonusRoomsPrefabs;
    [SerializeField] private GameObject[] enemyRoomsPrefabs;
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject finishRoomPrefab;
    [SerializeField] private GameObject bossRoomPrefab;

    private void Start() 
    {
        if (LevelData.instance.lvl == 5) bossRoom = true;
        Generate();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) Regenerate();
    }

    public static Vector2Int IntToScalar(int num) => scalars[num];
    private int BranchIdToRotation(int num) => (num + 2) % scalars.Length;
    private GameObject GetRandomRoom(GameObject[] rooms) => rooms[Random.Range(0, rooms.Length)];
    private bool HasFreeBranch(int x, int y) 
        => scalars.Any(scalar => grid[x + scalar[0], y + scalar[1]] == null);

    private void CreateRoom(int x, int y, int rotation, RoomType roomType)
    {
        //Pick new room of a roomType
        GameObject newRoom = null;
        switch (roomType)
        {
            case RoomType.Start: newRoom = startRoomPrefab; break;
            case RoomType.Enemy: newRoom = GetRandomRoom(enemyRoomsPrefabs); break;
            case RoomType.Bonus: newRoom = GetRandomRoom(bonusRoomsPrefabs); break;
            case RoomType.Finish: newRoom = finishRoomPrefab; break;
            case RoomType.Boss: newRoom = bossRoomPrefab; break;
        }

        //Write New Room
        grid[x, y] = Instantiate(newRoom, new Vector3(x * scale, y * scale, 0), Quaternion.identity).GetComponent<Room>();
        grid[x, y].SetCoordinates(x,y);

        Minimap.InitializeCell(x,y, roomType, highlightOnMinimap);

        currentRoomCoordinates[0] = x; 
        currentRoomCoordinates[1] = y;
        //Create new Branch
        if (roomType != RoomType.Start)
        {
            grid[x, y].CreateDoor(rotation);
            var scalar = IntToScalar(rotation);
            int branchId = (rotation + 2) % scalars.Length;
            grid[x + scalar.x, y + scalar.y].CreateBranch(branchId);

            if (highlightOnMinimap) Minimap.GetCell(x + scalar.x, y + scalar.y).SetBranch(branchId, true);
        }
        if (roomType == RoomType.Enemy) enemyRooms.Add(grid[x, y]);
        //Logging
        grid[x, y].gameObject.name = $"{x} || {y}";
    }

    private void Generate()
    {
        //Create Start Room and first enemy room
        int[] nextRoom = new int[3];
        CreateRoom(grid.GetLength(0) / 3, grid.GetLength(1) / 3, 0, RoomType.Start);
        int[] newBranch = FindFreeBranch(9,9);
        CreateRoom(newBranch[0],newBranch[1], newBranch[2],  RoomType.Enemy);
        highlightOnMinimap = false;
        //Create Other Rooms
        //Enemy Rooms
        for (int i = 0; i < enemyRoomsCount - 1; i++)
        {
            nextRoom = FindRoomSpace();
            CreateRoom(nextRoom[0], nextRoom[1], nextRoom[2], RoomType.Enemy);
        }
        //Bonus Rooms
        for (int i = 0; i < bonusRoomsCount; i++)
        {
            nextRoom = FindRoomSpace();
            CreateRoom(nextRoom[0],nextRoom[1], nextRoom[2], RoomType.Bonus);
        }
        //Finish OR Boss Room
        nextRoom = FindRoomSpace();
        RoomType lastRoomType = bossRoom ? RoomType.Boss : RoomType.Finish;
        CreateRoom(nextRoom[0], nextRoom[1], nextRoom[2], lastRoomType);
    }

    private void Regenerate()
    {
        foreach (Room room in grid)
        {
            if (room != null) Destroy(room.gameObject);
        }
        enemyRooms.Clear();
        currentRoomCoordinates[0] = 9;
        currentRoomCoordinates[1] = 9;
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

        return FindFreeBranch(currentRoomCoordinates[0], currentRoomCoordinates[1]);
    }

    private int[] FindFreeBranch(int x, int y)
    {
        int[] newCoordinates = new int[3];
        int randomBranch;
        Vector2Int newRoom;

        do
        {
            randomBranch = Random.Range(0, 4);
            newRoom = new Vector2Int(x, y) + IntToScalar(randomBranch);
        }
        while(grid[newRoom.x, newRoom.y] != null);

        //Connect new room to previos room
        (newCoordinates[0], newCoordinates[1]) = (newRoom[0], newRoom[1]);
        grid[currentRoomCoordinates[0], currentRoomCoordinates[1]].CreateDoor(randomBranch);
        newCoordinates[2] = BranchIdToRotation(randomBranch);
        return newCoordinates;
    }
}