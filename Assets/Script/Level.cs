using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private Room[,] grid = new Room[27,27];
    private List<Room> enemyRooms = new List<Room>();

    [SerializeField]  private int[] currentRoom = new int[2];
    [SerializeField] [Range(2, 5)] private int enemyRoomCount;
    [SerializeField] [Range(1, 5)] private int bonusRoomCount;
    [SerializeField] private GameObject[] bonusRoomPrefabs;
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private bool bossRoom;
    private Vector2Int[] dirMap = { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down};

    private const int scale = 29;

    private void Start() => Generate();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) Regenerate();
    }

    private Vector2Int IntToVector(int num) => dirMap[num];

    private int ReverseInt(int num)
    {
        num += 2;
        if (num > dirMap.Length - 1) num -= dirMap.Length;
        return num;
    }
    private void CreateRoom(int x, int y, int rotate, int roomId)
    {
        //Content
        Room newRoom;
        if (roomId == (int)RoomType.Bonus)
            newRoom = bonusRoomPrefabs[Random.Range(0, bonusRoomPrefabs.Length)].GetComponent<Room>();
        else
            newRoom = roomPrefabs[roomId].GetComponent<Room>();
        //Write New Room
        grid[x, y] = Instantiate(newRoom, new Vector3(x * scale, y * scale, 0), Quaternion.identity);
        grid[x, y].SetCoordinates(x,y);
        currentRoom[0] = x; currentRoom[1] = y;
        if (roomId == (int)RoomType.Enemy) enemyRooms.Add(grid[x, y]);
        //Create new Branch
        if (roomId != (int)RoomType.Start) grid[x, y].CreateBranch(rotate);

        //Logging
        grid[x, y].gameObject.name = $"{x} || {y}";
    }

    private void Generate()
    {
        //Create Start Room
        int[] nextRoom = new int[3];
        CreateRoom(grid.GetLength(0) / 3, grid.GetLength(1) / 3, 0, (int)RoomType.Start);
        int[] newBranch = FindFreeBranch(9,9);
        CreateRoom(newBranch[0],newBranch[1], newBranch[2], (int)RoomType.Enemy);
        //Create Other Rooms
        //Enemy Rooms
        for (int i = 0; i < enemyRoomCount - 1; i++)
        {
            nextRoom = FindRoomSpace();
            CreateRoom(nextRoom[0],nextRoom[1], nextRoom[2], (int)RoomType.Enemy);
        }
        //Bonus Rooms
        for (int i = 0; i < bonusRoomCount; i++)
        {
            nextRoom = FindRoomSpace();
            CreateRoom(nextRoom[0],nextRoom[1], nextRoom[2], (int)RoomType.Bonus);
        }
        //Finish OR Boss Room
        nextRoom = FindRoomSpace();
        int lastRoomId;

        if (bossRoom) lastRoomId = (int)RoomType.Boss;
        else lastRoomId = (int)RoomType.Finish;

        CreateRoom(nextRoom[0], nextRoom[1], nextRoom[2], lastRoomId);
    }
    private void Regenerate()
    {
        foreach (Room room in grid)
        {
            if (room != null) Destroy(room.gameObject);
        }
        enemyRooms.Clear();
        currentRoom[0] = 9;
        currentRoom[1] = 9;
        Generate();
    }

    private int[] FindRoomSpace()
    {
        int[] newCoordinates = new int[2];
        while (true)
        {
            int roomId = Random.Range(0, enemyRooms.Count);
            if (HasFreeBranch(enemyRooms[roomId].x, enemyRooms[roomId].y))
            {
                currentRoom[0] = enemyRooms[roomId].x;
                currentRoom[1] = enemyRooms[roomId].y;
                newCoordinates = FindFreeBranch(enemyRooms[roomId].x, enemyRooms[roomId].y);
                break;
            }
        }
        return newCoordinates;
    }

    private int[] FindFreeBranch(int x, int y)
    {
        int[] newCoor = new int[3];
        while (true)
        {
            int branchId = Random.Range(0, 4);
            Vector2Int newRoom = new Vector2Int(x,y) + IntToVector(branchId);
            if (grid[newRoom.x, newRoom.y] == null) 
            {
                newCoor[0] = newRoom[0];
                newCoor[1] = newRoom[1];
                grid[currentRoom[0], currentRoom[1]].CreateDoor(branchId);
                newCoor[2] = ReverseInt(branchId);
                break;
            }
        }
        return newCoor;
    }
    private bool HasFreeBranch(int x, int y)
    {
        foreach (var vector in dirMap)
            if (grid[x + vector[0], y + vector[1]] == null) return true;

        return false;
    }
}