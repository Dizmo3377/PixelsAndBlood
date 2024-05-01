using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject[] doors;

    [SerializeField] protected GameObject[] branches;

    [HideInInspector] public bool canDestroyObjects = true;

    public int x { get; private set; } = 0;
    public int y { get; private set; } = 0;

    public void SetCoordinates(int x, int y) => (this.x, this.y) = (x , y);

    public void CreateBranch(int id) => branches[id].SetActive(true);

    public void CreateDoor(int doorIndex)
    {
        Destroy(doors[doorIndex].gameObject);
        doors[doorIndex] = Instantiate(doorPrefab, doors[doorIndex].transform.position, Quaternion.identity);
        doors[doorIndex].transform.parent = transform;
    }
}