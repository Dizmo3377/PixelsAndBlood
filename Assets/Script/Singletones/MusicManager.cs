using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singletone<MusicManager>
{
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameplayMusic1;
    [SerializeField] private AudioClip gameplayMusic2;
    [SerializeField] private AudioClip finalMusic;
    [SerializeField] private AudioClip bossMusic;

    [SerializeField] private AudioSource audioSource;
    private float startVolume;
    private int currentGameplayMusic = 1;

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
        currentGameplayMusic++;

        switch (sceneName)
        {
            case "MainMenu": 
                audioSource.clip = mainMenuMusic; 
                break;
            case "Final":
                audioSource.clip = finalMusic;
                break;
            case "NewGame":
                currentGameplayMusic = 1;
                audioSource.clip = gameplayMusic1;
                break;
            case "Dungeon_1":
                audioSource.clip = currentGameplayMusic % 2 == 0 ? gameplayMusic2 : gameplayMusic1;
                break;
        }

        audioSource.Play();
    }

    public void PlayBossMusic()
    {
        audioSource.Stop();
        audioSource.clip = bossMusic;
        audioSource.Play();
    }

    public void StopMusic() => instance.audioSource.Stop();
}