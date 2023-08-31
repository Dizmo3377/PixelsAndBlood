using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    private List<Transform> weaponsTransform = new List<Transform>();
    private List<Weapon> weaponsScript = new List<Weapon>();
    private Weapon currentWeapon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            weaponsTransform.Add(collision.transform);
            weaponsScript.Add(collision.GetComponent<Weapon>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            for (int i = 0; i < weaponsTransform.Count; i++)
            {
                if (weaponsTransform[i].gameObject == collision.gameObject)
                {
                    weaponsTransform.RemoveAt(i);
                    weaponsScript[i].Highlight(false);
                    weaponsScript.RemoveAt(i);
                }
            }
        }
    }

    private void Update()
    {
        if (weaponsTransform.Count >= 1)
        {
            if (weaponsTransform.Count == 1)
            {
                weaponsScript[0].Highlight(true);
                currentWeapon = weaponsScript[0];
            }
            else
            {
                int id = GetClosestWeapon(weaponsTransform.ToArray());
                currentWeapon = weaponsScript[id];
                for (int i = 0; i < weaponsScript.Count; i++)
                {
                    if (i == id) { weaponsScript[i].Highlight(true); }
                    else{ weaponsScript[i].Highlight(false); }
                }

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentWeapon.Equiep();
                Destroy(currentWeapon);
            }
        }
    }

    int GetClosestWeapon(Transform[] weapon)
    {
        int minimumT = 0; //T means transform
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        for (int i = 0; i < weapon.Length; i++)
        {
            float dist = Vector3.Distance(weapon[i].position, currentPos);
            if (dist < minDist)
            {
                minimumT = i;
                minDist = dist;
            }
        }
        return minimumT;
    }
}
