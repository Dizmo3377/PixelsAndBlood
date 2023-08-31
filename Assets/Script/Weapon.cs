using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class Weapon : MonoBehaviour
{
    [HideInInspector] public Sprite icon;
    [HideInInspector] public float dropSpeed = 0;
    private Text nameUI;
    private Rigidbody2D rb;
    private float startTime;
    private GameObject canvas;

    [SerializeField] private AnimationCurve curve;
    [SerializeField] protected GameObject bulletPrefab;

    public Color nameColor;
    public new string name;
    public float shootSpeed;
    public int bulletForce;
    public float range;
    public int damage;
    public float accuracy;
    public int manaCost;

    private void Start()
    {
        icon = GetComponent<SpriteRenderer>().sprite;
        rb = GetComponent<Rigidbody2D>();
        canvas = GetComponentInChildren<Canvas>().gameObject;
        nameUI = GetComponentInChildren<Text>();

        Highlight(false);

        nameUI.text = name;
        nameUI.color = nameColor;
        startTime = Time.time;
    }
    private void FixedUpdate()
    {
        if (dropSpeed == 0) return;

        rb.velocity = rb.velocity * dropSpeed;
        dropSpeed = curve.Evaluate(Time.time - startTime);
    }

    public void Equiep()
    {
        if (Inventory.slots[Inventory.currentWeapon] != null) Inventory.ThrowPrimary();
        Inventory.slots[Inventory.currentWeapon] = (Weapon)this.Clone();
        Destroy(this.gameObject);
    }

    public virtual void Shoot(Vector3 target)
    {
        Vector2 dir = (target - Player.position);
                dir.Normalize();
        Vector2 bulletSpawn = Player.position + (Vector3)dir / 2;

        Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn, Quaternion.identity).GetComponent<Rigidbody2D>();
        bullet.velocity = (dir + Scatter()) * bulletForce;

        Vector3 rotateVector = target - Player.position;
        float angle = Mathf.Atan2(rotateVector.y, rotateVector.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private Vector2 Scatter() => new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy));
    public void Highlight(bool state) => canvas.SetActive(state);
    public object Clone() => MemberwiseClone();
    public virtual IEnumerator OnShoot() => null;
    public virtual void AfterShoot() { }
}