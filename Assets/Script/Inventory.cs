using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Weapon[] slots = new Weapon[2];
    public static int currentWeapon = 0;
    private static float throwForce = 5;
    public static int primaryDamage => slots[currentWeapon].damage;

    private void Start()
    {
        UpdateWeaponIcon();

        if (FindObjectOfType<InitialScene>())
        {
            slots[0] = null;
            slots[1] = null;
            Weapon startWeapon = Instantiate(GetWeapon("Glock18")).GetComponent<Weapon>();
            startWeapon.Equiep();
        }
    }

    private void Update()
    {
        if (PauseMenu.isPaused) return;
        if (Input.GetKeyDown(KeyCode.F)) ThrowPrimary();
    }

    public static void ChangeWeapon()
    { 
        if (Player.instance.isDead) return;
        currentWeapon = (currentWeapon + 1) % slots.Length;
        UpdateWeaponIcon();
    }

    public static void UpdateWeaponIcon()
    {
        if (slots[currentWeapon] != null) PlayerController.weaponSprite.sprite = slots[currentWeapon].icon;
        else PlayerController.weaponSprite.sprite = null;
        UI.instance.UpdateWeapon();
    }
    private static GameObject GetWeapon(string name) => Resources.Load<GameObject>("Weapons/" + name);

    public static void ThrowPrimary()
    {
        Weapon primary = slots[currentWeapon];
        if (primary == null) return;

        Weapon weapon = Instantiate(GetWeapon(primary.name), (Vector2)Player.instance.transform.position + Vector2.down * 0.1f, Quaternion.identity).GetComponent<Weapon>();
        weapon.Throw(throwForce);
        slots[currentWeapon] = null;

        UpdateWeaponIcon();
    }

}