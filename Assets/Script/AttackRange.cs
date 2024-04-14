using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private List<Transform> enemies = new List<Transform>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemies.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemies.Remove(collision.transform);
    }

    private bool CanReachEnemy(Vector2 from, Vector2 to)
    {
        RaycastHit2D hit = Physics2D.Raycast(from, to - from);
        if (hit.collider == null) return false;

        return hit.collider.CompareTag("Enemy");
    }

    public Enemy ClosestEnemy()
    {
        Vector3 currentPos = transform.position;
        Transform closestEnemy = null;
        float minDist = Mathf.Infinity;

        Transform[] enemiesArray = enemies.ToArray();

        foreach (Transform enemy in enemiesArray)
        {
            float dist = Vector3.Distance(enemy.position, currentPos);
            if (dist < minDist && CanReachEnemy(currentPos, enemy.position))
            {
                closestEnemy = enemy;
                minDist = dist;
            }
        }

        if (enemiesArray.Length < 1 || closestEnemy == null) return null;
        else return closestEnemy.gameObject.GetComponent<Enemy>();
    }
}