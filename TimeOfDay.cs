using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
    [Header("Settings")]
    public Light directionalLight; // Sun
    public Light moonLight;        // Moon
    public float dayLength = 120f; // Full day cycle duration
    public float timeOfDay = 0f;   // Time progression (0-1)

    [Header("Sun Settings")]
    public float sunMaxIntensity = 1f;

    [Header("Sun Colors")]
    public Color dawnColor = new Color(1f, 0.7f, 0.3f);
    public Color noonColor = new Color(1f, 1f, 1f);
    public Color duskColor = new Color(0.5f, 0.2f, 0.1f);

    [Header("Moon Settings")]
    public Color moonColor = new Color(0.6f, 0.6f, 1f);
    public float moonMaxIntensity = 0.4f;

    private const string TimeOfDayKey = "TimeOfDay";

    private void Start()
    {
        LoadTimeOfDay();
    }

    private void Update()
    {
        timeOfDay += Time.deltaTime / dayLength;
        if (timeOfDay >= 1f)
            timeOfDay = 0f;

        // Rotate sun
        float sunAngle = timeOfDay * 360f;
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f);

        // Rotate moon (opposite direction)
        float moonAngle = (timeOfDay + 0.5f) * 360f;
        moonLight.transform.rotation = Quaternion.Euler(moonAngle - 90f, 170f, 0f);

        // Sun color & intensity
        if (timeOfDay < 0.25f) // Dawn
        {
            directionalLight.color = Color.Lerp(dawnColor, noonColor, timeOfDay / 0.25f);
            directionalLight.intensity = Mathf.Lerp(0.2f, sunMaxIntensity, timeOfDay / 0.25f);
        }
        else if (timeOfDay < 0.75f) // Day to dusk
        {
            directionalLight.color = Color.Lerp(noonColor, duskColor, (timeOfDay - 0.25f) / 0.5f);
            directionalLight.intensity = sunMaxIntensity;
        }
        else // Night fall
        {
            directionalLight.color = Color.Lerp(duskColor, dawnColor, (timeOfDay - 0.75f) / 0.25f);
            directionalLight.intensity = Mathf.Lerp(sunMaxIntensity, 0.2f, (timeOfDay - 0.75f) / 0.25f);
        }

        // Moon intensity
        float moonIntensity = Mathf.Clamp01(Mathf.Cos(timeOfDay * 2f * Mathf.PI)) * moonMaxIntensity;
        moonLight.intensity = moonIntensity;
        moonLight.color = moonColor;

        SaveTimeOfDay();
    }

    private void SaveTimeOfDay()
    {
        PlayerPrefs.SetFloat(TimeOfDayKey, timeOfDay);
        PlayerPrefs.Save();
    }

    private void LoadTimeOfDay()
    {
        timeOfDay = PlayerPrefs.HasKey(TimeOfDayKey) ? PlayerPrefs.GetFloat(TimeOfDayKey) : 0.5f;
    }
    public string GetTimeOfDay24Hour()
    {
        float totalMinutesInDay = 24f * 60f;
        float currentMinutes = timeOfDay * totalMinutesInDay;

        int hours = Mathf.FloorToInt(currentMinutes / 60f);
        int minutes = Mathf.FloorToInt(currentMinutes % 60f);

        return $" Hours:{hours} Mins:{minutes}";
    }
    public string GetTimeOfDay12Hour()
    {
        float totalMinutesInDay = 24f * 60f;
        float currentMinutes = timeOfDay * totalMinutesInDay;

        int hours24 = Mathf.FloorToInt(currentMinutes / 60f);
        int minutes = Mathf.FloorToInt(currentMinutes % 60f);

        int hours12 = hours24 % 12;
        if (hours12 == 0) hours12 = 12; // 12 AM or 12 PM

        string ampm = hours24 < 12 ? "AM" : "PM";

        return $"{hours12:D2}:{minutes:D2} {ampm}";
    }

}
