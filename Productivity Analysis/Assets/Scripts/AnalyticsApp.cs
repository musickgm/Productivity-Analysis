using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalyticsApp : MonoBehaviour
{
    public App associatedApp;
    public RectTransform pieParentInput;
    public RectTransform pieParentOverall;

    public Text enterText;
    public Text spaceText;
    public Text overallUsageText;
    public Text workText;
    public Text browsingText;
    public Text idleText;

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


    private void CreateInputPie()
    {
        List<float> inputFractions = new List<float>();

        inputFractions.Add(associatedApp.timeTypes.work / associatedApp.secondsActive);
        inputFractions.Add(associatedApp.timeTypes.browse / associatedApp.secondsActive);
        inputFractions.Add(associatedApp.timeTypes.leisure / associatedApp.secondsActive);

        AnalyticsManager.Instance.MakePie(inputFractions, pieParentInput, AnalyticsManager.Instance.activityColors);
    }

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


