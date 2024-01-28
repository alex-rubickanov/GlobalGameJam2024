using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsGainUIManager : MonoBehaviour
{
    public static PointsGainUIManager instance;

    [SerializeField] GameObject pointsGainUI;
    [SerializeField] Transform parent;

    [Header("Object Pooling")] [SerializeField]
    float numberOfObj = 10f;

    [SerializeField] public List<GameObject> points;

    [SerializeField] Transform playerTransform;
    [SerializeField] public RectTransform player1UI;
    [SerializeField] public RectTransform player2UI;
    [SerializeField] public RectTransform player3UI;
    
    private int player1Points = 0;
    private int player2Points = 0;
    private int player3Points = 0;
    
    [SerializeField] private TextMeshProUGUI player1PointsText;
    [SerializeField] private TextMeshProUGUI player2PointsText;
    [SerializeField] private TextMeshProUGUI player3PointsText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitObjectPool();
    }

    private void Update()
    {
        player1PointsText.text = player1Points.ToString();
        player2PointsText.text = player2Points.ToString();
        player3PointsText.text = player3Points.ToString();
    }


    public void ShowUIPoints(Transform playerTransform, int value)
    {
        NPlayerManager playerManager = playerTransform.GetComponentInParent<NPlayerManager>();
                
        int index = NCoopManager.Instance.players.FindIndex(x => x == playerManager);
        
        
        if (points.Count <= 0)
        {
            return;
        }

        foreach (GameObject point in points)
        {
            if (!point.activeSelf)
            {
                TextMeshProUGUI textMesh = point.GetComponentInChildren<TextMeshProUGUI>();
                if (value < 0)
                {
                    textMesh.color = Color.red;
                    textMesh.SetText($"{value}");
                }
                else
                {
                    textMesh.color = Color.green;
                    textMesh.SetText($"+{value}");
                }

                RectTransform rectTransform = point.GetComponent<RectTransform>();

                // Convert world position to screen space
                Vector2 screenPos = Camera.main.WorldToScreenPoint(playerTransform.position);

                rectTransform.position = new Vector3(screenPos.x, screenPos.y, 0f);

                point.SetActive(true);
                
                switch (index)
                {
                    case 0:
                        StartCoroutine(MoveToPlayerUI(rectTransform, player1UI, index,value));
                        break;
                    case 1:
                        StartCoroutine(MoveToPlayerUI(rectTransform, player2UI, index,value));
                        break;
                    case 2:
                        StartCoroutine(MoveToPlayerUI(rectTransform, player3UI, index,value));
                        break;
                }
                
                StartCoroutine(SetPointsActiveToFalse(point));
                break;
            }
        }
    }

    IEnumerator MoveToPlayerUI(RectTransform rectTransform, Transform targetPosition, int index,int value)
    {
        float duration = 1f;
        float elapsedTime = 0f;

        Vector3 startPosition = rectTransform.position;

        while (elapsedTime < duration)
        {
            rectTransform.position = Vector3.Lerp(startPosition, targetPosition.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetPosition.GetComponent<Animator>().SetTrigger("ReceivePoints");
        rectTransform.position = targetPosition.position;
        yield return null;
        switch (index)
        {
            case 0:
                player1Points += value;
                break;
            case 1:
                player2Points += value;
                break;
            case 2:
                player3Points += value;
                break;
        }
    }

    IEnumerator SetPointsActiveToFalse(GameObject point)
    {
        yield return new WaitForSeconds(2);
        point.SetActive(false);
    }

    void InitObjectPool()
    {
        for (int i = 0; i < numberOfObj; i++)
        {
            GameObject pointsUI = Instantiate(pointsGainUI, parent.transform.position, Quaternion.identity);
            pointsUI.transform.SetParent(parent);
            pointsUI.SetActive(false);
            points.Add(pointsUI);
        }
    }
}