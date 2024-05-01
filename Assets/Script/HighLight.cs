using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    private List<Weapon> weapons = new List<Weapon>();
    private Weapon currentWeapon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Weapon")) return;

        weapons.Add(collision.GetComponent<Weapon>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Weapon")) return;

        Weapon weapon = weapons.First(w => w.gameObject == collision.gameObject);
        weapon.Highlight(false);
        weapons.Remove(weapon);
    }

    private void Update()
    {
        if (weapons.Count < 1) return;

        if (weapons.Count == 1)
        {
            weapons[0].Highlight(true);
            currentWeapon = weapons[0];
        }
        else
        {
            Weapon closestWeapon = GetClosestWeapon();
            currentWeapon = closestWeapon;

            weapons.ForEach(w => w.Highlight(false));
            closestWeapon.Highlight(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentWeapon.Equiep();
            Destroy(currentWeapon);
        }
    }

    Weapon GetClosestWeapon()
    {
        Weapon closestWeapon = weapons[0];
        float closestDistace = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Weapon weapon in weapons)
        {
            float distance = Vector3.Distance(weapon.transform.position, currentPosition);

            if (distance < closestDistace)
            {
                closestWeapon = weapon;
                closestDistace = distance;
            }
        }

        return closestWeapon;
    }
}
