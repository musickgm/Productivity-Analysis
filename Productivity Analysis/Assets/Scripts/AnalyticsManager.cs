using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalyticsManager : MonoBehaviour
{
    public ApplicationManager applicationManager;
    public Text overallWork;
    public Text overallBrowse;
    public Text overallLeisure;

    private TimeTypes appTimeTypes;

    public void RunAnalytics()
    {
        appTimeTypes.work = 0;
        appTimeTypes.browse = 0;
        appTimeTypes.leisure = 0;

        for(int i = 0; i < applicationManager.usedApplications.Count; i++)
        {
            App app = applicationManager.usedApplications[i];
            appTimeTypes.work += app.timeTypes.work;
            appTimeTypes.browse += app.timeTypes.browse;
            appTimeTypes.leisure += app.timeTypes.leisure;
        }

        overallWork.text = appTimeTypes.work.ToString();
        overallBrowse.text = appTimeTypes.browse.ToString();
        overallLeisure.text = appTimeTypes.leisure.ToString();

    }
}


//TODO
//Display overall analytics for work/browsing/leisure based on input
//Display overall analytics for work/browsing/leisure based on apps used

//Display overall timeline for productivity based on app type
//Display overall timeline for productivity based on input

//Display analytics for each app broken down by input (work/browsing/leisure)
//Display timeline for app productivity based on input