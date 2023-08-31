using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spill : MonoBehaviour
{
    [SerializeField] private bool fire;
    [SerializeField] private bool poison;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float destroyDelay;

    private void Start()
    {
        particle.Stop();
        var main = particle.main;
        main.duration = destroyDelay;
        particle.Play();

        StartCoroutine(ExtintCor());
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (fire) { Player.instance.fired = 3; }
            if (poison) { Player.instance.poisoned = 3; }
        }
    }

    public void Extint()
    {
        Destroy(gameObject);
    }

    private IEnumerator ExtintCor()
    {
        yield return new WaitForSeconds(destroyDelay);
        animator.SetTrigger("Destroy");
    }
}
