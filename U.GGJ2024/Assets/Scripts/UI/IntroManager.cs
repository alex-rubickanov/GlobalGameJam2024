using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] GameObject instructions;
    [SerializeField] GameObject GameIntro;
    [SerializeField] GameObject GamePlayCanvas;
    // Update is called once per frame

    private void Start()
    {
        Time.timeScale = 0f;
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            instructions.SetActive(false);
            GameIntro.SetActive(true);
            GamePlayCanvas.SetActive(true);
            Time.timeScale = 1f;
            this.enabled = false;
        }
    }
}
