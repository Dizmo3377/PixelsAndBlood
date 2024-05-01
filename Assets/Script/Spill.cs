using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spill : MonoBehaviour
{
    [SerializeField] private bool fire;
    [SerializeField] private bool poison;
    [SerializeField] private float destroyDelay;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        particle.Stop();
        var main = particle.main;
        main.duration = destroyDelay;
        particle.Play();
        audioSource.Play();

        StartCoroutine(Extintion());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (fire) Player.instance.fired = 3;
            if (poison) Player.instance.poisoned = 3;
        }
    }

    public void Extint()
    {
        audioSource.Stop();
        Destroy(gameObject);
    }

    private IEnumerator Extintion()
    {
        yield return new WaitForSeconds(destroyDelay);
        animator.SetTrigger("Destroy");
    }
}
