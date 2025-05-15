using Animancer.Examples.FineControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSkipUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider skipSlider;
    public Button confirmButton;
    public TextMeshProUGUI confirmBtnTxt;
    public Button backButton;
    public Button[] otherButtons;
    public TextMeshProUGUI label;

    [Header("Settings")]
    public float hourStep = 1f; // 1 hour per step
    public float realSecondsPerHour = 1f; // Real-world seconds per in-game hour

    private TimeOfDay timeOfDay;
    [HideInInspector] public bool isSleeping = false;

    public Button cancelButton; // <- NEW: Cancel Button
    private Coroutine skipCoroutine; // <- NEW: Keep reference to running coroutine
    private bool isSkipping = false; // <- NEW: Track skipping state

    private void Awake()
    {
        isSleeping = false; 
        
        if (cancelButton != null)
            cancelButton.gameObject.SetActive(false); // Hide cancel button initially

    }
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

        confirmButton.onClick.AddListener(() =>
        {
            float hoursToSkip = skipSlider.value;

            if (skipCoroutine != null) // Stop any existing coroutine first
                StopCoroutine(skipCoroutine);

            skipCoroutine = StartCoroutine(SkipTime(hoursToSkip));
        });

    }
    private void Update()
    {
        // Toggle Cancel Button visibility
        if (cancelButton != null)
            cancelButton.gameObject.SetActive(isSkipping);

        if (isSleeping)
        {
            confirmBtnTxt.text = "SLEEP FOR TIME";
            label.text = $"Sleep " + skipSlider.value + " hour(s)";
            return;
        }
        else
        {
            confirmBtnTxt.text = "WAIT FOR TIME";
            label.text = $"Wait " + skipSlider.value + " hour(s)";
            return;
        }

    }
    private void UpdateLabel(float value)
    {
        if (label != null)
        {
            if(isSleeping)
            {
                label.text = $"Sleep {value} hour(s)";
            }
            else
            {
                label.text = $"Wait {value} hour(s)";
            }
        }
    }

    private System.Collections.IEnumerator SkipTime(float hours)
    {
        isSkipping = true;
        // Disable UI interactivity
        SetButtonInteraction(false);

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
        SetButtonInteraction(true);

        //ensure back button stays off if sleeping
        if (isSleeping == true)
        {
            backButton.interactable = false;
        }
        isSkipping = false;
    }
    public void CancelSkip()
    {
        if (skipCoroutine != null)
        {
            StopCoroutine(skipCoroutine);
            skipCoroutine = null;
        }

        isSkipping = false;
        SetButtonInteraction(true);

        //ensure back button stays off if sleeping
        if (isSleeping == true)
        {
            backButton.interactable = false;
        }

        skipSlider.value = skipSlider.minValue;
        UpdateLabel(skipSlider.value);
    }
    public void SetButtonInteraction(bool interactable)
    {
        confirmButton.gameObject.SetActive(interactable);
        skipSlider.interactable = interactable;
        backButton.interactable = interactable;
        foreach (var button in otherButtons)
        {
            if (button != null)
                button.interactable = interactable;
        }
    }
}
