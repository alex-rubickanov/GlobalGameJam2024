using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RandomizeViewerNumbers : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI viewersNum;
    int count;

    private void Start()
    {
        viewersNum.SetText($"0");
        StartCoroutine(RandomizeNumber());
    }

    IEnumerator RandomizeNumber()
    {
        while (true)
        {
            int randomTime = Random.Range(3, 7);

            count++;

            yield return new WaitForSeconds(randomTime);

            if (count < 4)
            {
                int randomView = Random.Range(2, 50);
                viewersNum.SetText($"{randomView}");
            }
            else
            {
                int randomView = Random.Range(10, 90);
                viewersNum.SetText($"{randomView}k");
            }
        }
    }
}
