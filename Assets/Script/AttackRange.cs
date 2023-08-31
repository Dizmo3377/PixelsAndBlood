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

    public Vector3 ClosestEnemy()
    {
        Vector3 currentPos = transform.position;
        Vector3 closestEnemy = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float minDist = Mathf.Infinity;

        Transform[] enemiesArray = enemies.ToArray();

        foreach (Transform enemy in enemiesArray)
        {
            float dist = Vector3.Distance(enemy.position, currentPos);
            if (dist < minDist)
            {
                closestEnemy = enemy.position;
                minDist = dist;
            }
        }

        return closestEnemy;
    }
}