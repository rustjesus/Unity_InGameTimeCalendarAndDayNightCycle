using UnityEngine;

[RequireComponent(typeof(TimeOfDay))]
public class InGameCalendar : MonoBehaviour
{
    private TimeOfDay timeOfDaySource;

    [Header("Calendar Settings")]
    public string[] monthNames = new string[]
    {
        "Auroran", "Brimleaf", "Cindermoon", "Duskfall", "Emberdeep", "Frostwake",
        "Glimmereve", "Hallowshade", "Ivorywind", "Jadestone", "Kindlebark", "Lunaris"
    };

    public int daysPerMonth = 30;

    [Header("Current Date")]
    public int day = 1;
    public int month = 1; // Index in monthNames
    public int year = 0;

    private float lastTimeOfDay = 0f;
    private void Awake()
    {

    }
    private void Start()
    {
        timeOfDaySource = GetComponent<TimeOfDay>();
        LoadDate();
    }

    private void Update()
    {
        if (timeOfDaySource == null) return;

        // Detect start of new day (timeOfDay wrapped around)
        if (timeOfDaySource.timeOfDay < lastTimeOfDay)
        {
            AdvanceDay();
        }

        lastTimeOfDay = timeOfDaySource.timeOfDay;


        SaveDate();
    }

    private void AdvanceDay()
    {
        day++;

        if (day > daysPerMonth)
        {
            day = 1;
            month++;

            if (month > monthNames.Length)
            {
                month = 1;
                year++;
            }
        }



        Debug.Log($"New Day: {GetDateString()}");
    }

    public string GetDateString()
    {
        return $"{monthNames[month]} {day}, Year {year}";
    }

    private void SaveDate()
    {
        Debug.Log("Saving in game date and time");
        PlayerPrefs.SetInt("Calendar_Day", day);
        PlayerPrefs.SetInt("Calendar_Month", month);
        PlayerPrefs.SetInt("Calendar_Year", year);
        PlayerPrefs.Save();
    }

    private void LoadDate()
    {
        day = PlayerPrefs.GetInt("Calendar_Day", 1);
        month = PlayerPrefs.GetInt("Calendar_Month", 1);
        year = PlayerPrefs.GetInt("Calendar_Year", 0);
    }
}
