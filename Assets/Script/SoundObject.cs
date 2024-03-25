using UnityEngine;

public class SoundObject : MonoBehaviour
{
    void Start() => GetComponent<AudioSource>().enabled = PlayerPrefs.GetInt("Volume") == 1 ? true : false ;
}