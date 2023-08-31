using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] private float transitionTime;
    [SerializeField] private CanvasGroup blackScreen;

    private bool inTransition;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public static void StartSceneTransition(string newSceneName)
    {
        if(instance.inTransition) return;

        instance.inTransition = true;
        instance.StartCoroutine(SceneTransition(newSceneName));
    }

    static IEnumerator SceneTransition(string newSceneName)
    {
        instance.blackScreen.gameObject.SetActive(true);
        FindObjectOfType<EventSystem>().enabled = false;

        yield return instance.StartCoroutine(Fade(1f));
        SceneManager.LoadScene(newSceneName);
        yield return instance.StartCoroutine(Fade(0f));

        instance.blackScreen.gameObject.SetActive(false);

        instance.inTransition = false;
    }

    static IEnumerator Fade(float finalAlpha)
    {
        float fadeSpeed = 1 / instance.transitionTime;
        while (!Mathf.Approximately(instance.blackScreen.alpha, finalAlpha))
        {
            instance.blackScreen.alpha = Mathf.MoveTowards(instance.blackScreen.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        instance.blackScreen.alpha = finalAlpha;
    }
}