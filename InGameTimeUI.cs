using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inGameDateTimeTxt;
    private InGameCalendar inGameCalendar;
    private TimeOfDay tod;
    private int totalMonths;
    // Start is called before the first frame update
    void Start()
    {
        inGameCalendar = FindAnyObjectByType<InGameCalendar>();
        tod = FindAnyObjectByType<TimeOfDay>();
    }

    // Update is called once per frame
    void Update()
    {
        totalMonths = inGameCalendar.monthNames.Length;
        inGameDateTimeTxt.text = "In Game Date & Time," + "\n"
            + " Year:" + inGameCalendar.year + "\n"
            + " Month:" + inGameCalendar.month + "/" + totalMonths + " (" + inGameCalendar.monthNames[inGameCalendar.month] + ") " + "\n"
            + " Day:" + inGameCalendar.day + "/" + inGameCalendar.daysPerMonth + "\n"
            + tod.GetTimeOfDay12Hour();


    }
}
