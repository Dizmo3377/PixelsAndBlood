using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class Weapon : MonoBehaviour
{
    private float dropSpeed = 0;
    private float startTime;
    private new Rigidbody2D rigidbody;
    private GameObject canvas;
    private Text nameText;

    [SerializeField] private AnimationCurve curve;
    [SerializeField] protected GameObject bulletPrefab;

    [field: SerializeField] private Color nameColor { get; set; }
    [field: SerializeField] private float shake { get; set; }
    [field: SerializeField] private int bulletForce { get; set; }
    [field: SerializeField] private float accuracy { get; set; }
    [field:SerializeField] public new string name { get; private set; }
    [field: SerializeField] public float shootSpeed { get; private set; }
    [field: SerializeField] public int damage { get; protected set; }
    [field: SerializeField] public int manaCost { get; protected set; }

    public Sprite icon { get; private set; }

    private void Awake()
    {
        icon = GetComponent<SpriteRenderer>().sprite;
        rigidbody = GetComponent<Rigidbody2D>();

        canvas = GetComponentInChildren<Canvas>().gameObject;
        nameText = GetComponentInChildren<Text>();
        nameText.text = name;
        nameText.color = nameColor;

        startTime = Time.time;

        Highlight(false);
    }
    private void FixedUpdate()
    {
        if (dropSpeed == 0) return;

        rigidbody.velocity = rigidbody.velocity * dropSpeed;
        dropSpeed = curve.Evaluate(Time.time - startTime);
    }

    public void Highlight(bool state) => canvas.SetActive(state);
    private Vector2 Scatter() => new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy));

    public virtual IEnumerator OnShoot() => null;
    public virtual void AfterShoot() => CameraShaker.Shake(0.1f,shake,1);

    public void Throw(float force)
    {
        Vector2 direction, spawnPoint, startPoint = transform.position;
        int maxTries = 1000, tries = 0;

        //Ensure that we will not spawn weapon in other object
        do
        {
            float radiusAdjustment = 1.3f;
            direction = Random.insideUnitCircle.normalized * radiusAdjustment;
            spawnPoint = new Vector2(startPoint.x + direction.x, startPoint.y + direction.y);
            tries++;

            //If there is no space to spawn a weapon, then spawn it in players position
            if (tries >= maxTries)
            {
                direction = Vector2.zero;
                spawnPoint = transform.position;
                break;
            }
        } 
        while (spawnPoint.BoxOverlapsSomething(Vector2.one, transform.rotation.z));

        transform.position = spawnPoint;

        dropSpeed = 1f;
        rigidbody.velocity = direction * force;
    }

    public void Equiep()
    {
        if (Inventory.slots[Inventory.currentWeapon] != null) Inventory.ThrowPrimary();

        Inventory.slots[Inventory.currentWeapon] = (Weapon)MemberwiseClone();
        Inventory.UpdateWeaponIcon();
        Destroy(gameObject);
    }

    public virtual void Shoot(Vector3 target)
    {
        Vector2 dir = target - Player.position;
        dir.Normalize();

        Vector2 bulletSpawn = Player.position + (Vector3)dir / 2;

        Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn, Quaternion.identity).GetComponent<Rigidbody2D>();
        bullet.velocity = (dir + Scatter()) * bulletForce;

        Vector3 rotateVector = target - Player.position;
        float angle = Mathf.Atan2(rotateVector.y, rotateVector.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}