
using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{
    public TMP_Text pointsText;
    public TMP_Text upgradeCostText;
    public TMP_Text upgradePPSCostText;

    public int points = 0;

    private int pointsPerClick = 1;
    private int upgradeCost = 1;

    public float pointsAddInterval = 1;
    private float timer;

    public int pointsPerSecond;
    public int upgradePointPerSecondCost = 1;

    // IMAGE SCALING
    [Header("Image Scaling")]
    public Transform imageToScale;
    public float minScale = 1f;
    public float maxScale = 3f;
    public int maxPointsForScale = 1000;
    public float scaleSpeed = 5f;

    private float targetScale = 1f;

    void Start()
    {
        Debug.Log("World");

        upgradeCostText.text = upgradeCost.ToString();
        pointsText.text = points.ToString();
        upgradePPSCostText.text = pointsPerSecond.ToString();

        UpdateImageScale();

        if (imageToScale != null)
        {
            imageToScale.localScale = Vector3.one * minScale;
        }
    }

    private void Update()
    {
        // Automatic point generation
        timer += Time.deltaTime;

        if (timer >= pointsAddInterval)
        {
            GeneratePoints(pointsPerSecond);
            timer = 0;
        }

        // Smooth image scaling
        if (imageToScale != null)
        {
            float currentScale = imageToScale.localScale.x;

            float newScale = Mathf.Lerp(
                currentScale,
                targetScale,
                Time.deltaTime * scaleSpeed
            );

            imageToScale.localScale = new Vector3(
                newScale,
                newScale,
                newScale
            );
        }
    }

    public void GeneratePoints(int addedPoints)
    {
        points += addedPoints;
        pointsText.text = points.ToString();

        UpdateImageScale();

        print(points);
    }

    public void AddPointsPerClick()
    {
        GeneratePoints(pointsPerClick);
    }

    public void Upgrade()
    {
        if (points >= upgradeCost)
        {
            points -= upgradeCost;
            pointsText.text = points.ToString();

            upgradeCost++;
            upgradeCostText.text = upgradeCost.ToString();

            pointsPerClick += 1;

            UpdateImageScale();

            Debug.Log("Upgraded successfully");
        }
        else
        {
            Debug.LogError("Not enough points");
        }
    }

    public void UpdatePointsPerSecond()
    {
        if (points >= upgradePointPerSecondCost)
        {
            points -= upgradePointPerSecondCost;
            pointsText.text = points.ToString();

            pointsPerSecond += 1;

            upgradePointPerSecondCost += 2;

            upgradePPSCostText.text = pointsPerSecond.ToString();

            UpdateImageScale();
        }
    }

    private void UpdateImageScale()
    {
        if (imageToScale == null)
            return;

        float progress = Mathf.Clamp01(
            (float)points / maxPointsForScale
        );

        targetScale = Mathf.Lerp(
            minScale,
            maxScale,
            progress
        );
    }

    private bool TrySpendPoints(int amount)
    {
        if (points >= amount)
        {
            points -= amount;
            pointsText.text = points.ToString();

            UpdateImageScale();

            return true;
        }

        return false;
    }
}
