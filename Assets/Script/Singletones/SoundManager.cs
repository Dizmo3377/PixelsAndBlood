using UnityEngine;
using System;

public class SoundManager : Singletone<SoundManager>
{
    private static AudioClip[] sounds;

    private void Start() => LoadSounds();

    private void LoadSounds()
    {
        try 
        { 
            sounds = Resources.LoadAll<AudioClip>("Audio");
        }
        catch (NullReferenceException)
        {
            Debug.LogError("Error loading audio assets in SoundManager. Folder is missing or invalid. Use Resources/Audio.");
            throw;
        }
    }

    public void Play(string clipName, bool loop = false, float delayTime = 0f, bool dontRepeat = true)
    {
        if (PlayerPrefs.GetInt("Volume") == 0) return;

        Stop(clipName);
        AudioSource audioSource = new GameObject(clipName).AddComponent<AudioSource>();
        audioSource.transform.SetParent(instance.transform);
        audioSource.clip = Array.Find(sounds, sound => sound.name == clipName);
        audioSource.PlayDelayed(delayTime);

        if (loop) audioSource.loop = true;
        else Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void PlayRandomRange(string clipName, int minInclusive, int maxInclusive, bool dontRepeat = true)
    {
        if (PlayerPrefs.GetInt("Volume") == 0) return;

        clipName += UnityEngine.Random.Range(minInclusive, maxInclusive + 1).ToString();

        Play(clipName, false, 0f, dontRepeat);
    }

    public void Stop(string clipName)
    {
        Transform toDestry = instance.transform.Find(clipName);
        if (toDestry != null) Destroy(toDestry.gameObject);
    }
}