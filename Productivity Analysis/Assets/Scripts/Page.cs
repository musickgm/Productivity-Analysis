using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to account for different pages for navigation
/// </summary>
public class Page : MonoBehaviour
{
    public RectTransform parentRect;            //Parent rect of the page
    public Image iconBG;                        //BG color for button - black for active page
    public AnalyticsApp myAnalytics;            //Analytics to run when page becomes active

    /// <summary>
    /// Called on button press - activates the apge
    /// </summary>
    public void ActivatePage()
    {
        iconBG.enabled = true;
        parentRect.gameObject.SetActive(true);
        if(myAnalytics != null)
        {
            myAnalytics.Analyze();
        }
        else
        {
            AnalyticsManager.Instance.RunAnalytics();
        }
    }

    /// <summary>
    /// Called by page manager when a new button/page is selected
    /// </summary>
    public void DeactivatePage()
    {
        iconBG.enabled = false;
        parentRect.gameObject.SetActive(false);
    }

}
