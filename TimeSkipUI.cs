using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSkipUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider skipSlider;
    public Button confirmButton;
    public Button[] otherButtons;
    public TextMeshProUGUI label;

    [Header("Settings")]
    public float hourStep = 1f; // 1 hour per step
    public float realSecondsPerHour = 1f; // Real-world seconds per in-game hour

    private TimeOfDay timeOfDay;

    private void Start()
    {
        timeOfDay = FindAnyObjectByType<TimeOfDay>();

        if (skipSlider != null)
        {
            skipSlider.minValue = 1;
            skipSlider.maxValue = 24;
            skipSlider.wholeNumbers = true;
            skipSlider.onValueChanged.AddListener(UpdateLabel);
            UpdateLabel(skipSlider.value);
        }

        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(() =>
            {
                float hoursToSkip = skipSlider.value;
                StartCoroutine(SkipTime(hoursToSkip));
            });
        }
    }

    private void UpdateLabel(float value)
    {
        if (label != null)
        {
            label.text = $"Skip {value} hour(s)";
        }
    }

    private System.Collections.IEnumerator SkipTime(float hours)
    {
        // Disable UI interactivity
        confirmButton.interactable = false;
        skipSlider.interactable = false;
        foreach (var button in otherButtons)
        {
            if (button != null)
                button.interactable = false;
        }

        float totalSteps = hours / hourStep;
        float stepProgress = hourStep / 24f;

        for (int i = 0; i < totalSteps; i++)
        {
            timeOfDay.timeOfDay += stepProgress;
            if (timeOfDay.timeOfDay >= 1f)
                timeOfDay.timeOfDay -= 1f;

            skipSlider.value = Mathf.Max(skipSlider.minValue, skipSlider.value - hourStep);
            yield return new WaitForSecondsRealtime(realSecondsPerHour);
        }

        // Reset slider and label
        skipSlider.value = skipSlider.minValue;
        UpdateLabel(skipSlider.value);

        // Re-enable UI interactivity
        confirmButton.interactable = true;
        skipSlider.interactable = true;
        foreach (var button in otherButtons)
        {
            if (button != null)
                button.interactable = true;
        }
    }
}
