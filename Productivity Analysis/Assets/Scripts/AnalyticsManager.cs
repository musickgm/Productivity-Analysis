using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TimelineSection
{
    public string appName;
    public Color color;
    public string startTime;
    public string endTime;
    public float secLength;
    public ApplicationType appType;
}

public class AnalyticsManager : Singleton<AnalyticsManager>
{
    public Text overallWork;
    public Text overallBrowse;
    public Text overallLeisure;

    //App Timeline
    public RectTransform appTimelineParent;
    public RectTransform appTypeTimelineParent;
    //Pie Chart 
    public RectTransform pieParentInput;
    public RectTransform pieParentType;

    public GameObject timelineSectionPrefab;
    public Image wedgePrefab;

    //Colors - work = green; browsing = yellow; leisure = green; misc = grey
    public List<Color> activityColors;
    [HideInInspector]
    public float totalTime = 0;


    private List<TimelineSection> appTimeline = new List<TimelineSection>();
    private TimeTypes appTimeInput;
    private TimeTypes appTimeType;



    private void Start()
    {
        
    }

    public void RunAnalytics()
    {



        ApplicationManager.Instance.EndCurrentApplication();
        CreateAppTimeline();
        CreateAppTypeTimeline();
        CreateInputPieChart();
        CreateAppTypePieChart();
    }

    public void AddTimelineSection(TimelineSection newSection)
    {
        appTimeline.Add(newSection);
        totalTime += newSection.secLength;
    }

    private void CreateAppTimeline()
    {
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

    public void CreateAppTypeTimeline()
    {
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

        float workFraction = appTimeInput.work / totalTime;
        float browseFraction = appTimeInput.browse / totalTime;
        float leisureFraction = appTimeInput.leisure / totalTime;

        List<float> inputFractions = new List<float>();
        inputFractions.Add(workFraction);
        inputFractions.Add(browseFraction);
        inputFractions.Add(leisureFraction);


        MakePie(inputFractions, pieParentInput, activityColors);

        overallWork.text = appTimeInput.work.ToString();
        overallBrowse.text = appTimeInput.browse.ToString();
        overallLeisure.text = appTimeInput.leisure.ToString();
    }

    public void CreateAppTypePieChart()
    {
        appTimeType.work = 0;
        appTimeType.browse = 0;
        appTimeType.leisure = 0;

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
                    appTimeType.misc += appTime;
                    break;
            }
        }

        float workFraction = appTimeType.work / totalTime;
        float browseFraction = appTimeType.browse / totalTime;
        float leisureFraction = appTimeType.leisure / totalTime;
        float miscFraction = appTimeType.misc / totalTime;


        List<float> typeFractions = new List<float>();
        typeFractions.Add(workFraction);
        typeFractions.Add(browseFraction);
        typeFractions.Add(leisureFraction);
        typeFractions.Add(miscFraction);

        MakePie(typeFractions, pieParentType, activityColors);

    }


    /// <summary>
    ///Pie chart adapted from "Board to bits games youtube series"
    /// </summary>
    /// <param name="values"></param>
    /// <param name="parent"></param>
    public void MakePie(List<float> values, RectTransform parent, List<Color> pieColors)
    {
        float zRotation = 0f;
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < values.Count; i++)
        {
            if(values[i] > 0)
            {
                Image newWedge = Instantiate(wedgePrefab) as Image;
                newWedge.transform.SetParent(parent, false);
                newWedge.color = pieColors[i];
                newWedge.fillAmount = values[i];
                newWedge.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRotation));
                zRotation -= newWedge.fillAmount * 360f;
            }

        }
    }

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


