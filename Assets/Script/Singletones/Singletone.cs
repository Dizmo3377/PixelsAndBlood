using UnityEngine;

public abstract class Singletone<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}