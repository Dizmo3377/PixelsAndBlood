using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour, IPushable
{
    public Animator animator;
    public SpriteRenderer weaponSprite;
    public AudioSource walkingSound;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float speed;
    [SerializeField] private int meeleDamage;
    [SerializeField] private float meeleSpeed;
    [SerializeField] private float closeCombatDistance;

    private bool isShooting = false;
    private Player player;
    private Enemy aimTarget;
    private Vector3 mousePos = Vector2.zero;
    private Vector2 moveVector;
    private Vector2 pushVectorValue;

    private Vector3 shootingTargetPos => aimTarget == null ? mousePos : aimTarget.transform.position;

    public float pushTime { get; set; }

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
        walkingSound.Play();
        walkingSound.Pause();
    }

    private void Start()
    {
        player = GetComponent<Player>();
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

        if (CanShoot()) StartCoroutine(Shoot());

        animator.SetBool("isMoving", moveVector != Vector2.zero ? true : false);
        if (moveVector == Vector2.zero) walkingSound.Pause();
        else walkingSound.UnPause();

        FlipFace(isShooting ? shootingTargetPos - transform.position : moveVector);
        RotateWeapon(isShooting ? shootingTargetPos : mousePos);
    }

    private bool CanShoot() => Input.GetMouseButton(0) && !isShooting && !RaycastUtilities.PointerIsOverUI(Input.mousePosition);

    private IEnumerator Shoot()
    {
        if (CloseToEnemy())
        {
            isShooting = true;
            yield return StartCoroutine(MeeleAttack());
        }
        else
        {
            Weapon weapon = Inventory.slots[Inventory.currentWeapon];
            if (weapon == null || player.manaPoints < weapon.manaCost) yield break;

            isShooting = true;
            yield return weapon.OnShoot();
            weapon.Shoot(shootingTargetPos);
            player.manaPoints -= weapon.manaCost;
            weapon.AfterShoot();

            yield return new WaitForSeconds(weapon.shootSpeed);
        }

        isShooting = false;
    }

    private IEnumerator MeeleAttack()
    {
        if (aimTarget != null) aimTarget.GetDamage(meeleDamage);

        Effects.instance.Slash(transform, shootingTargetPos);
        SoundManager.instance.PlayRandomRange("punch", 1, 2);

        yield return new WaitForSeconds(meeleSpeed);
    }

    public void RotateWeapon(Vector3 target)
    {
        Vector3 rotateVector = target - transform.position;
        float angle = Mathf.Atan2(rotateVector.y, rotateVector.x) * Mathf.Rad2Deg;
        weaponSprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (rotateVector.x > 0) weaponSprite.flipY = false;
        else weaponSprite.flipY = true;
    }

    public void FlipFace(Vector3 target)
    {
        if (!isShooting && target.x == 0) return;
        sprite.flipX = (target.x >= 0) ? false : true;
    }

    public void Disable()
    {
        walkingSound.Stop();
        rb.velocity = Vector2.zero;
        animator.SetBool("isMoving", false);
        enabled = false;
    }

    private void GetInputData()
    {
        moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimTarget = player.attack.ClosestEnemy();
    }

    private bool CloseToEnemy()
    {
        if (aimTarget == null) return false;

        Vector2 direction = shootingTargetPos - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);

        if (!hit.collider.CompareTag("Enemy")) return false;
        else
        {
            Collider2D enemyCollider = aimTarget.GetComponent<Collider2D>();

            Vector2 closestPoint = enemyCollider.ClosestPoint(transform.position);
            float distanceToEnemy = Vector2.Distance(transform.position, closestPoint);
            return distanceToEnemy <= closeCombatDistance;
        }
    }
}