using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// All the relevant info for a timeline section
/// </summary>
public struct TimelineSection
{
    public string appName;
    public Color color;
    public string startTime;
    public string endTime;
    public float secLength;
    public ApplicationType appType;
}

/// <summary>
/// Overall analytics manager
/// </summary>
public class AnalyticsManager : Singleton<AnalyticsManager>
{
    //App Timeline
    public RectTransform appTimelineParent;
    public RectTransform appTypeTimelineParent;
    //Pie Chart 
    public RectTransform pieParentInput;
    public RectTransform pieParentType;

    public GameObject timelineSectionPrefab;                //Prefab for creating timeline section
    public Image wedgePrefab;                               //Prefab for creating pie chart

    //Colors - work = green; browsing = yellow; leisure = green; misc = grey
    public List<Color> activityColors;
    [HideInInspector]
    public float totalTime = 0;

    private List<TimelineSection> appTimeline = new List<TimelineSection>();
    private TimeTypes appTimeInput;
    private TimeTypes appTimeType;




    /// <summary>
    /// Called when page is selected - run all analytics
    /// </summary>
    public void RunAnalytics()
    {
        ApplicationManager.Instance.EndCurrentApplication();
        CreateAppTimeline();
        CreateAppTypeTimeline();
        CreateInputPieChart();
        CreateAppTypePieChart();
    }

    /// <summary>
    /// Called everytime an application loses focus - creates a new timeline section in the list and adjusts total time. 
    /// </summary>
    /// <param name="newSection"></param>
    public void AddTimelineSection(TimelineSection newSection)
    {
        appTimeline.Add(newSection);
        totalTime += newSection.secLength;
    }

    /// <summary>
    /// Create the timeline based on which apps are used
    /// </summary>
    private void CreateAppTimeline()
    {
        //Destroy previous timeline
        foreach(Transform child in appTimelineParent.transform)
        {
            Destroy(child.gameObject);
        }


        float totalWidth = appTimelineParent.rect.width;
        float currentLocation = 0;

        for(int i = 0; i < appTimeline.Count; i++)
        {
            float sectionTimeFraction = appTimeline[i].secLength / totalTime;
            float width = sectionTimeFraction * totalWidth;
            Vector2 sectionPosition = new Vector2(currentLocation, 0);
            
            GameObject newSection = Instantiate(timelineSectionPrefab, appTimelineParent);

            newSection.GetComponent<TimelineUISection>().Initialize(appTimeline[i], sectionPosition, width);
            currentLocation += width;
        }

    }

    /// <summary>
    /// Create the timeline based on app types
    /// </summary>
    public void CreateAppTypeTimeline()
    {
        //Destroy previous timeline
        foreach (Transform child in appTypeTimelineParent.transform)
        {
            Destroy(child.gameObject);
        }


        float totalWidth = appTypeTimelineParent.rect.width;

        float currentLocation = 0;

        for (int i = 0; i < appTimeline.Count; i++)
        {
            float sectionTimeFraction = appTimeline[i].secLength / totalTime;
            float width = sectionTimeFraction * totalWidth;
            Vector2 sectionPosition = new Vector2(currentLocation, 0);

            GameObject newSection = Instantiate(timelineSectionPrefab, appTypeTimelineParent);

            newSection.GetComponent<TimelineUISection>().Initialize(appTimeline[i], sectionPosition, width);
            newSection.GetComponent<TimelineUISection>().ChangeColor(ActivityTypeColor(appTimeline[i].appType));

            currentLocation += width;
        }
    }

    /// <summary>
    /// Create pie chart based on input
    /// </summary>
    public void CreateInputPieChart()
    {
        appTimeInput.work = 0;
        appTimeInput.browse = 0;
        appTimeInput.leisure = 0;

        for (int i = 0; i < ApplicationManager.Instance.usedApplications.Count; i++)
        {
            App app = ApplicationManager.Instance.usedApplications[i];
            appTimeInput.work += app.timeTypes.work;
            appTimeInput.browse += app.timeTypes.browse;
            appTimeInput.leisure += app.timeTypes.leisure;
        }
        for (int i = 0; i < ApplicationManager.Instance.standardApplications.Count; i++)
        {
            App app = ApplicationManager.Instance.standardApplications[i];
            appTimeInput.work += app.timeTypes.work;
            appTimeInput.browse += app.timeTypes.browse;
            appTimeInput.leisure += app.timeTypes.leisure;
        }

        float total = appTimeInput.work + appTimeInput.browse + appTimeInput.leisure;

        float workFraction = appTimeInput.work / total;
        float browseFraction = appTimeInput.browse / total;
        float leisureFraction = appTimeInput.leisure / total;

        List<float> inputFractions = new List<float>();
        inputFractions.Add(workFraction);
        inputFractions.Add(browseFraction);
        inputFractions.Add(leisureFraction);


        MakePie(inputFractions, pieParentInput, activityColors);
    }

    /// <summary>
    /// Create pie chart based on app type
    /// </summary>
    public void CreateAppTypePieChart()
    {
        appTimeType.work = 0;
        appTimeType.browse = 0;
        appTimeType.leisure = 0;
        appTimeType.misc = 0;

        for (int i = 0; i < ApplicationManager.Instance.usedApplications.Count; i++)
        {
            float appTime = ApplicationManager.Instance.usedApplications[i].secondsActive;
            ApplicationType type= ApplicationManager.Instance.usedApplications[i].appType;
            switch(type)
            {
                case ApplicationType.work:
                    appTimeType.work += appTime;
                    break;
                case ApplicationType.browsing:
                    appTimeType.browse += appTime;
                    break;
                case ApplicationType.leisure:
                    appTimeType.leisure += appTime;
                    break;
                case ApplicationType.misc:
                    appTimeType.misc += appTime;
                    break;
                default:
                    print("catch 1");
                    break;
            }
        }
        for (int i = 0; i < ApplicationManager.Instance.standardApplications.Count; i++)
        {
            float appTime = ApplicationManager.Instance.standardApplications[i].secondsActive;
            ApplicationType type = ApplicationManager.Instance.standardApplications[i].appType;
            switch (type)
            {
                case ApplicationType.work:
                    appTimeType.work += appTime;
                    break;
                case ApplicationType.browsing:
                    appTimeType.browse += appTime;
                    break;
                case ApplicationType.leisure:
                    appTimeType.leisure += appTime;
                    break;
                case ApplicationType.misc:
                    appTimeType.misc += appTime;
                    break;
                default:
                    print("catch 2");
                    break;
            }
        }

        float total = appTimeType.work + appTimeType.browse + appTimeType.leisure + appTimeType.misc;

        float workFraction = appTimeType.work / total;
        float browseFraction = appTimeType.browse / total;
        float leisureFraction = appTimeType.leisure / total;
        float miscFraction = appTimeType.misc / total;


        List<float> typeFractions = new List<float>();
        typeFractions.Add(workFraction);
        typeFractions.Add(browseFraction);
        typeFractions.Add(leisureFraction);
        typeFractions.Add(miscFraction);

        MakePie(typeFractions, pieParentType, activityColors);

    }


    /// <summary>
    ///Pie chart adapted from "Board to bits games youtube series"
    ///Called by any function desiring to make a pie chart
    /// </summary>
    /// <param name="values"></param> list of values (fractions)
    /// <param name="parent"></param> parent housing the pie chart
    /// <param name="pieColors"></param> list of colors to be used in the chart
    public void MakePie(List<float> values, RectTransform parent, List<Color> pieColors)
    {
        float zRotation = 0f;
        //Destroy previous pie chart
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }

        //For each fraction, make a wedge, set its parent, color, fill, and rotation.
        for (int i = 0; i < values.Count; i++)
        {
            if(values[i] > 0)
            {
                Image newWedge = Instantiate(wedgePrefab) as Image;
                newWedge.transform.SetParent(parent, false);
                newWedge.color = pieColors[i];
                newWedge.fillAmount = values[i];
                newWedge.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRotation));
                //Increment rotation for next piece
                zRotation -= newWedge.fillAmount * 360f;
            }

        }
    }

    /// <summary>
    /// Determine correct color based on application type (work, browsing, leisure, misc)
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Color ActivityTypeColor(ApplicationType type)
    {
        switch(type)
        {
            case ApplicationType.work:
                return activityColors[0];
            case ApplicationType.browsing:
                return activityColors[1];
            case ApplicationType.leisure:
                return activityColors[2];
            case ApplicationType.misc:
                return activityColors[3];
            default:
                return activityColors[3];
        }
    }

}


