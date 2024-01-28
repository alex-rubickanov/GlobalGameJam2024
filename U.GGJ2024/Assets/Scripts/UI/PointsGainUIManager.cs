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
    [SerializeField] public RectTransform playerUI;

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



    public void ShowUIPoints(Transform worldPoint, int value)
    {
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
                Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPoint.position);

                rectTransform.position = new Vector3(screenPos.x, screenPos.y, 0f);

                point.SetActive(true);

                StartCoroutine(MoveToPlayerUI(rectTransform, playerUI));
                StartCoroutine(SetPointsActiveToFalse(point));
                break;
            }
        }
    }

    IEnumerator MoveToPlayerUI(RectTransform rectTransform, Transform targetPosition)
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