using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singletone<MusicManager>
{
    [SerializeField] private AudioClip mainMenu;
    [SerializeField] private AudioClip gameplay1;
    [SerializeField] private AudioClip gameplay2;
    [SerializeField] private AudioClip final;
    [SerializeField] private AudioClip boss;

    [SerializeField] private AudioSource audioSource;
    private float startVolume;
    private int gameplayMusic = 1;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        startVolume = audioSource.volume;
        audioSource.Play();
    }

    private void Update()
    {
        audioSource.volume = startVolume * PlayerPrefs.GetInt("Music");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        gameplayMusic++;

        switch (sceneName)
        {
            case "MainMenu": 
                audioSource.clip = mainMenu; 
                break;
            case "Final":
                audioSource.clip = final;
                break;
            case "NewGame":
                gameplayMusic = 1;
                audioSource.clip = gameplay1;
                break;
            case "Dungeon_1":
                audioSource.clip = gameplayMusic % 2 == 0 ? gameplay2 : gameplay1;
                break;
        }

        audioSource.Play();
    }

    public void PlayBossMusic()
    {
        audioSource.Stop();
        audioSource.clip = boss;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}