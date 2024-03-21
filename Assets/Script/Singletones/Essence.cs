using UnityEngine;

public class Essence : Singletone<Essence>
{
    private static GameObject manaPrefab;
    public override void Awake() { base.Awake(); manaPrefab = Resources.Load<GameObject>("Essence/Mana"); }
    public static void SplashMana(Transform transform, int count, float force)
    {
        Rigidbody2D[] mana = new Rigidbody2D[count];
        for (int i = 0; i < mana.Length; i++)
        {
            mana[i] = Instantiate(manaPrefab.GetComponent<Rigidbody2D>(), transform.position, Quaternion.identity);
            mana[i].velocity = new Vector2(Random.Range(-force, force), Random.Range(-force, force));
        }
    }
}