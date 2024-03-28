using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : Singletone<Effects>
{
    [SerializeField] private GameObject slashPrefab;

    public void Slash(Transform spawnPoint)
    {
        Slash slash = Instantiate(slashPrefab, spawnPoint).GetComponent<Slash>();
        slash.transform.localPosition = Vector3.zero;
        slash.Play();
    }

    public void Slash(Transform spawnPoint, Vector3 target)
    {
        Slash slash = Instantiate(slashPrefab, spawnPoint).GetComponent<Slash>();
        slash.Rotate(target);
        slash.Play();
    }
}