using UnityEngine;

public class Room : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject[] branches;

    public void SetCoordinates(int x, int y) => (this.x, this.y) = (x , y);

    public void CreateBranch(int id)
    {
        branches[id].SetActive(true);
        CreateDoor(id);
    }

    public void CreateDoor(int doorIndex)
    {
        Destroy(doors[doorIndex].gameObject);
        doors[doorIndex] = Instantiate(doorPrefab, doors[doorIndex].transform.position, Quaternion.identity);
        doors[doorIndex].transform.parent = transform;
    }
}