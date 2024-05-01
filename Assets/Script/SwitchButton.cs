using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    [SerializeField] private string key; //PlayerPrefs key which we switch
    [SerializeField] private int currentState = 1;
    [SerializeField] private Sprite[] stateOnSprites;
    [SerializeField] private Sprite[] stateOffSprites;

    private Image image;
    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        if (!PlayerPrefs.HasKey(key)) 
        {
            currentState = 1;
            PlayerPrefs.SetInt(key, currentState);
        }
        if (currentState != PlayerPrefs.GetInt(key)) Switch();
    }

    public void Switch()
    {
        var spriteState = button.spriteState;

        if (currentState == 0) //Turn on
        {
            currentState = 1;
            image.sprite = stateOnSprites[0];
            spriteState.pressedSprite = stateOnSprites[1];
        }
        else //Turn off
        {
            currentState = 0;
            image.sprite = stateOffSprites[0];
            spriteState.pressedSprite = stateOffSprites[1];
        }

        button.spriteState = spriteState;
        Debug.Log(key + currentState);
        PlayerPrefs.SetInt(key, currentState);
    }
}