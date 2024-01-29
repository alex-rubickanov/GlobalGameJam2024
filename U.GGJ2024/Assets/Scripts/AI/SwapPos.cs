using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPos : MonoBehaviour
{
    [SerializeField] GameObject[] players;
    [SerializeField] Transform bugTransform;
    [SerializeField] GoToHidingSpot goToHidingSpot;
    Vector3 chosenplayerPos;
    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        this.enabled = false;
    }
    private void OnEnable()
    {
        SwapPositionWithAPlayer();
    }
    private void SwapPositionWithAPlayer()
    {
        int randomIndex = Random.Range(0, players.Length);
        chosenplayerPos = players[randomIndex].transform.position;
        players[randomIndex].transform.position = bugTransform.position;
        goToHidingSpot.RandomizeHidingSpot();
        StartCoroutine(SwapCurrentPosToPlayer());
    }

    IEnumerator SwapCurrentPosToPlayer()
    {
        yield return new WaitForSeconds(.1f);
        bugTransform.position = chosenplayerPos;
        this.enabled = false;
    }
}
