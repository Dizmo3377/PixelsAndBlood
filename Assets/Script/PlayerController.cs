using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IPushable
{
    [SerializeField] public static Animator animator;
    [SerializeField] public static SpriteRenderer weaponSprite;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;

    private Player playerData;
    private bool isShooting = false;
    private Vector3 aimTarget = Vector2.zero;
    private Vector3 mousePos = Vector2.zero;
    private Vector2 moveVector;

    public float pushTime { get; set; }
    private Vector2 pushVectorValue;
    public Vector2 pushVector 
    {
        get 
        {
            pushVectorValue = (pushTime <= 0) ? Vector2.zero : pushVectorValue * 0.9f;
            return pushVectorValue;
        }
        set 
        {
            pushTime = Time.time;
            pushVectorValue = value * 10;
        }
    }

    private void Awake()
    {
        playerData = Player.instance;
        animator = GetComponent<Animator>();
        weaponSprite = transform.Find("Weapon").GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveVector * speed + pushVector;
        pushTime -= (pushTime <= 0) ? 0 : Time.unscaledDeltaTime;
    }

    private void Update()
    {
        if (Player.instance.isDead) return;

        GetInputData();

        if (Input.GetMouseButton(0) && !isShooting && !RaycastUtilities.PointerIsOverUI(Input.mousePosition))
            StartCoroutine(Shoot());

        animator.SetBool("isMoving", moveVector != Vector2.zero ? true : false);
        RotateFace(isShooting ? aimTarget - transform.position : moveVector);
        RotateWeapon(isShooting ? aimTarget : mousePos);
    }

    private IEnumerator Shoot()
    {
        Weapon weapon = Inventory.slots[Inventory.currentWeapon];
        if (weapon == null || playerData.manaPoints < weapon.manaCost) yield break;

        isShooting = true;
        yield return weapon.OnShoot();

        weapon.Shoot(aimTarget);
        playerData.ChangeMana(-weapon.manaCost);
        weapon.AfterShoot();

        yield return new WaitForSeconds(weapon.shootSpeed);
        isShooting = false;
    }

    public void RotateWeapon(Vector3 target)
    {
        Vector3 rotateVector = target - transform.position;
        float angle = Mathf.Atan2(rotateVector.y, rotateVector.x) * Mathf.Rad2Deg;
        weaponSprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (rotateVector.x > 0) weaponSprite.flipY = false;
        else weaponSprite.flipY = true;
    }

    public void RotateFace(Vector3 target)
    {
        if (!isShooting && target.x == 0) return;
        sprite.flipX = (target.x >= 0) ? false : true;
    }

    private void GetInputData()
    {
        moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimTarget = playerData.attack.ClosestEnemy();
    }
}