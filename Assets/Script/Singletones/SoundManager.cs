using UnityEngine;
using System;

public class SoundManager : Singletone<SoundManager>
{
    private static AudioClip[] gameSounds;
    private void Start()
    {
        GetData();
        SetVolume(true);
    }

    public static void Play(string clipName, bool loop = false, float delayTime = 0f)
    {
        if (PlayerPrefs.GetInt("Volume") == 0) return;

        Stop(clipName);
        AudioSource audioSource = new GameObject(clipName).AddComponent<AudioSource>();
        audioSource.transform.SetParent(instance.transform);
        audioSource.clip = Array.Find(gameSounds, sound => sound.name == clipName);
        audioSource.PlayDelayed(delayTime);

        if (loop) audioSource.loop = true;
        else Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public static void PlayRandomRange(string clipName, int minInclusive, int maxInclusive, bool dontRepeat = true)
    {
        if (PlayerPrefs.GetInt("Volume") == 0) return;

        clipName += UnityEngine.Random.Range(minInclusive, maxInclusive + 1).ToString();

        if(dontRepeat) Stop(clipName);
        AudioSource audioSource = new GameObject(clipName).AddComponent<AudioSource>();
        audioSource.transform.SetParent(instance.transform);
        audioSource.clip = Array.Find(gameSounds, sound => sound.name == clipName);
        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public static void Stop(string clipName)
    {
        Transform toDestry = instance.transform.Find(clipName);
        if (toDestry == null) return;
        Destroy(toDestry.gameObject);
    }

    public void SetVolume(bool state) => PlayerPrefs.SetInt("Volume", Convert.ToInt32(state));
    public void SetMusic(bool state) => PlayerPrefs.SetInt("Music", Convert.ToInt32(state));

    private void GetData()
    {
        try { gameSounds = Resources.LoadAll<AudioClip>("Audio"); }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Error loading audio assets in SoundManager. Folder is missing or something, idk");
            throw;
        }
    }
}