using UnityEngine;

public class Effects : Singletone<Effects>
{
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private GameObject manaPrefab;

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
        damageNumber.FadeMoveAnimation();
    }

    public void SplashMana(Transform transform, int count, float force)
    {
        Rigidbody2D[] mana = new Rigidbody2D[count];
        for (int i = 0; i < mana.Length; i++)
        {
            mana[i] = Instantiate(manaPrefab.GetComponent<Rigidbody2D>(), transform.position, Quaternion.identity);
            mana[i].velocity = new Vector2(Random.Range(-force, force), Random.Range(-force, force));
        }
    }
}