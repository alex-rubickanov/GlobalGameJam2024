using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsGainUIManager : MonoBehaviour
{
    public static PointsGainUIManager instance;

    [SerializeField] GameObject pointsGainUI;
    [SerializeField] Transform parent;

    [Header("Object Pooling")]
    [SerializeField] float numberOfObj = 10f;
    [SerializeField] public List<GameObject> points;

    [SerializeField] Transform playerTransform;
    [SerializeField] RectTransform playerUI;

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowUIPoints(playerTransform, 5);
        }
    }

    public void ShowUIPoints(Transform worldPoint, float value)
    {
        if (points.Count <= 0) { return; }

        foreach (GameObject point in points)
        {
            if (!point.activeSelf)
            {
                TextMeshProUGUI textMesh = point.GetComponentInChildren<TextMeshProUGUI>();
                textMesh.SetText($"+{value}");

                RectTransform rectTransform = point.GetComponent<RectTransform>();

                // Use the player's position as the UI position (adjust y if needed)
                rectTransform.position = Camera.main.WorldToScreenPoint(worldPoint.position);

                point.SetActive(true);

                //while(Vector3.Distance(rectTransform.position, playerUI.position) > 1)
                //{
                //    rectTransform.position = Vector3.MoveTowards(rectTransform.position, playerUI.position, 5 * Time.deltaTime);
                //}
                StartCoroutine(SetPointsActiveToFalse(point));
                break;
            }
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
