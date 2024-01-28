using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCharacterSelector : MonoBehaviour
{
    private NInputHandler inputHandler;

    [SerializeField] private List<GameObject> characters;
    private GameObject currentCharacter;
    private int index;

    public Action OnChangeCharacter;
    
    private void Start()
    {
        inputHandler = GetComponentInParent<NInputHandler>();

        inputHandler.OnCCLeft += OnCCLeft;
        inputHandler.OnCCRight += OnCCRight;
        inputHandler.OnReady += Select;

        index = 0;
        currentCharacter = characters[index];

        index = Random.Range(0, characters.Count);
        ChangeCharacter();
    }

    private void Select()
    {
        MainMenuUIManager.Instance.EnableReadyBtn(GetComponentInParent<NPlayerManager>().InputHandler.playerInput.playerIndex);
        this.enabled = false;
    }

    private void OnCCRight()
    {
        if (characters.Count > index + 1)
        {
            index++;
            ChangeCharacter();
        }
    }

    private void OnCCLeft()
    {
        if (index > 0)
        {
            index--;
            ChangeCharacter();
        }
    }

    private void ChangeCharacter()
    {
        OnChangeCharacter?.Invoke();
        
        currentCharacter.SetActive(false);
        currentCharacter = characters[index];
        currentCharacter.SetActive(true);

        SFX();
    }
    //added SFX
    void SFX()
    {
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlayOneShotSfx(audioManager.characterJoin);
    }
}