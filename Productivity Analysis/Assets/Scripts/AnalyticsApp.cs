using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Analytics (graphs, texts, etc) for a specific app are displayed with this
/// </summary>
public class AnalyticsApp : MonoBehaviour
{
    public App associatedApp;                   //Associated app that has the data needed
    public RectTransform pieParentInput;        //Transform where the pie chart will be displayed
    public RectTransform pieParentOverall;      //Transform where the pie chart will be displayed

    //Specific text boxes for different analytics
    public Text enterText;                      
    public Text spaceText;
    public Text overallUsageText;
    public Text workText;
    public Text browsingText;
    public Text idleText;

    /// <summary>
    /// Called when a page is selected - analyze or reanalyze all the data and create charts. 
    /// </summary>
    public void Analyze()
    {
        CreateInputPie();

        //Create pie fraction of overall time
        CreateOverallPie();

        //Stats on enter
        if(enterText != null)
        {
            enterText.text = associatedApp.enterHits.ToString();
        }

        //Stats on space
        if (spaceText != null)
        {
            spaceText.text = associatedApp.spaceHits.ToString();
        }

        //Overall usage
        if (overallUsageText != null)
        {
            overallUsageText.text = ConvertToAppropriateTime(associatedApp.secondsActive);
        }

        if (workText != null)
        {
            workText.text = ConvertToAppropriateTime(associatedApp.timeTypes.work);
        }

        if (browsingText != null)
        {
            browsingText.text = ConvertToAppropriateTime(associatedApp.timeTypes.browse);
        }

        if(idleText.text != null)
        {
            idleText.text = ConvertToAppropriateTime(associatedApp.timeTypes.leisure);
        }


    }


    /// <summary>
    /// Create the pie chart based on input used in this app
    /// </summary>
    private void CreateInputPie()
    {
        List<float> inputFractions = new List<float>();

        inputFractions.Add(associatedApp.timeTypes.work / associatedApp.secondsActive);
        inputFractions.Add(associatedApp.timeTypes.browse / associatedApp.secondsActive);
        inputFractions.Add(associatedApp.timeTypes.leisure / associatedApp.secondsActive);

        AnalyticsManager.Instance.MakePie(inputFractions, pieParentInput, AnalyticsManager.Instance.activityColors);
    }

    /// <summary>
    /// Create a pie chart based on usage of this app compared to overall usage of everything
    /// </summary>
    private void CreateOverallPie()
    {
        List<float> inputFractions = new List<float>();
        List<Color> colors = new List<Color>();

        inputFractions.Add(associatedApp.secondsActive / AnalyticsManager.Instance.totalTime);
        inputFractions.Add((AnalyticsManager.Instance.totalTime - associatedApp.secondsActive) / AnalyticsManager.Instance.totalTime);

        colors.Add(associatedApp.appColor);
        colors.Add(Color.white);

        AnalyticsManager.Instance.MakePie(inputFractions, pieParentOverall, colors);
    }

    /// <summary>
    /// Convert time in seconds to something more readable if over 60 seconds
    /// </summary>
    /// <param name="time"></param>time in seconds to be converted
    /// <returns></returns>
    public string ConvertToAppropriateTime(float time)
    {
        string timeString;

        if (time < 60)
        {
            timeString = time.ToString("F1") + " s";
        }
        else if (time < (60*60))
        {
            timeString = (time / 60).ToString("F1") + " min";
        }
        else
        {
            timeString = (time / (60 * 60)).ToString("F1") + "hour";
        }

        return timeString;
    }
}


