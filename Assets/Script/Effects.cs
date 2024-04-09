using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;

public class Effects : Singletone<Effects>
{
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private GameObject damageNumberPrefab;

    public void Slash(Transform spawnPoint)
    {
        Slash slash = Instantiate(slashPrefab, spawnPoint).GetComponent<Slash>();
        slash.transform.localPosition = Vector3.zero;
        slash.Play();
    }

    public void Slash(Transform spawnPoint, Vector3 target)
    {
        Slash slash = Instantiate(slashPrefab, spawnPoint).GetComponent<Slash>();
        slash.Rotate(target);
        slash.Play();
    }

    public void DamageNumber(Transform spawnPoint, int number)
    {
        DamageNumber damageNumber = Instantiate(damageNumberPrefab, spawnPoint).GetComponent<DamageNumber>();

        Vector3 spawnPosition = new Vector3(Random.Range(-0.4f,0.4f), 0.4f, -5f);
        damageNumber.transform.localPosition = spawnPosition;

        damageNumber.SetRightScale();
        damageNumber.SetNumber(number);
        damageNumber.Tween();
    }
}