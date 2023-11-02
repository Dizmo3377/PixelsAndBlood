using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Weapon[] slots = new Weapon[2];
    public static int currentWeapon = 0;
    private static float throwForce = 5;
    public static int primaryDamage => slots[currentWeapon].damage;

    private void Start() => UpdateWeaponIcon();

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

        Rigidbody2D rb = Instantiate(GetWeapon(primary.name), Player.instance.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        rb.GetComponent<Weapon>().dropSpeed = 1f;
        rb.velocity = new Vector2(Random.Range(-throwForce, throwForce), Random.Range(-throwForce, throwForce));
        Destroy(slots[currentWeapon]);
        slots[currentWeapon] = null;

        UpdateWeaponIcon();
    }

}