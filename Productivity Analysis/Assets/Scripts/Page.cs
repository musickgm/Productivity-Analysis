using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public RectTransform parentRect;
    public Image iconBG;
    public AnalyticsApp myAnalytics;

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

    public void DeactivatePage()
    {
        iconBG.enabled = false;
        parentRect.gameObject.SetActive(false);
    }

}
