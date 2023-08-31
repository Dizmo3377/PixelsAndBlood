using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveVector;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] public static Animator animator;

    [SerializeField] private SpriteRenderer weaponSprite;
    private Player playerData;

    private void Start()
    {
        playerData = Player.instance;
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate() => rb.velocity = moveVector * speed;

    private void Update()
    {
        moveVector = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")).normalized;

        if (moveVector.x != 0)
            FaceRotate((Vector2)transform.position + moveVector);

        if (Input.GetMouseButton(0) && playerData.canShoot && !RaycastUtilities.PointerIsOverUI(Input.mousePosition))
            StartCoroutine(Shooting());

        if (moveVector != Vector2.zero) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);

        RotateWeapon(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //Pod voprosom vot eta hueta
        if (Inventory.slots[Inventory.currentWeapon] != null)
            weaponSprite.sprite = Inventory.slots[Inventory.currentWeapon].icon;
        else weaponSprite.sprite = null;
    }

    private IEnumerator Shooting()
    {
        Weapon weapon = Inventory.slots[Inventory.currentWeapon];
        if (weapon == null || playerData.manaPoints < weapon.manaCost) yield break;

        playerData.canShoot = false;
        yield return weapon.OnShoot();

        Vector3 target = playerData.attack.ClosestEnemy();
        weapon.Shoot(target);
        FaceRotate(target);
        RotateWeapon(target);
        playerData.ChangeMana(-weapon.manaCost);
        weapon.AfterShoot();

        yield return new WaitForSeconds(weapon.shootSpeed);
        playerData.canShoot = true;
    }

    public void RotateWeapon(Vector3 target)
    {
        Vector3 rotateVector = target - transform.position;
        float angle = Mathf.Atan2(rotateVector.y, rotateVector.x) * Mathf.Rad2Deg;
        weaponSprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (rotateVector.x > 0) weaponSprite.flipY = false;
        else weaponSprite.flipY = true;
    }

    public void FaceRotate(Vector3 target)
    {
        Vector2 rotateVector = target - transform.position;
        if (rotateVector.x > 0) sprite.flipX = false;
        else sprite.flipX = true;
    }

}