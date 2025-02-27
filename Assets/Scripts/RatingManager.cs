using UnityEngine;

public class RatingManager : MonoBehaviour
{
    [SerializeField] private UI uiScript;

    private float currentRating = 50f;
    private float maxRating = 100f;

    public float GetNormalizedRating()
    {
        return currentRating / maxRating;
    }
    public void IncreaseRating(float increase)
    {
        currentRating += increase;
        currentRating = Mathf.Min(currentRating, maxRating);
    }
    public void DecreaseRating(float decrease)
    {
        currentRating -= decrease;
        currentRating = Mathf.Max(currentRating, 0);
    }

    void Update()
    {
        float normalizedRating = GetNormalizedRating();
        uiScript.SetRating(normalizedRating);
    }
}
