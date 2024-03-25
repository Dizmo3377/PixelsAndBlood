using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    [SerializeField] private string key; //variable which we switch
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

        if (currentState == 0) //turn it on
        {
            currentState = 1;
            image.sprite = stateOnSprites[0];
            spriteState.pressedSprite = stateOnSprites[1];
        }
        else //turn it off
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