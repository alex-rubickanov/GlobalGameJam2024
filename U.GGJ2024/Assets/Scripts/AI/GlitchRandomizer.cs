using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchRandomizer : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] bugs;
    [SerializeField] float maxTimeToRandomize = 5f;
    float maxTime;
    public Hide hide => GetComponent<Hide>();
    private void Start()
    {
        maxTime = maxTimeToRandomize;
    }
    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        if (maxTimeToRandomize > 0 && !hide.isHiding)
        {
            maxTimeToRandomize -= Time.deltaTime;
        }
        else if (maxTimeToRandomize <= 0)
        {
            int randomIndex = Random.Range(0, 5);
            if (randomIndex > 2)
            {
                ChooseABugToActivate();
            }

            maxTimeToRandomize = maxTime;
        }

        if (hide.isHiding)
        {
            maxTimeToRandomize = maxTime;
        }
    }

    void ChooseABugToActivate()
    {
        //int randomIndex = Random.Range(0, bugs.Length);
        int randomIndex = 0;
        bugs[randomIndex].enabled = true;
    }
}
