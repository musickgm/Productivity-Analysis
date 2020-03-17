using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ApplicationManager : Singleton<ApplicationManager>
{
    //timeline of applications
    public List<App> usedApplications = new List<App>();
    public List<App> standardApplications = new List<App>();

    private App currentApplication;


    public void SetApplication(string appName)
    {
        App newApp = ApplicationFromStandardList(appName);
        if(newApp == currentApplication)
        {
            return;
        }

        //deactivate previous application
        if (currentApplication != null && currentApplication.active)
        {
            currentApplication.Deactivate();
        }
        currentApplication = null;

        currentApplication = newApp;


        if(currentApplication == null)
        {
            currentApplication = ApplicationFromUsedList(appName);
        }
        if (currentApplication ==  null)
        {
            currentApplication = MakeNewApp(appName);
        }

        //activate new application
        currentApplication.Activate();
    }

    private App ApplicationFromUsedList(string _appName)
    {
        for (int i = 0; i < usedApplications.Count; i++)
        {
            if (_appName.Contains(usedApplications[i].searchKey))
            {
                return usedApplications[i];

            }
        }

        return null;
    }

    private App ApplicationFromStandardList(string _appName)
    {
        for (int i = 0; i < standardApplications.Count; i++)
        {
            if (_appName.Contains(standardApplications[i].searchKey))
            {
                usedApplications.Add(standardApplications[i]);
                return standardApplications[i];
            }
        }
        return null;
    }

    private App MakeNewApp(string _appName)
    {
        App newApplication = gameObject.AddComponent<App>();
        newApplication.appName = _appName;
        newApplication.searchKey = _appName;
        newApplication.appType = ApplicationType.misc;
        newApplication.appColor = AnalyticsManager.Instance.activityColors[3];
        usedApplications.Add(newApplication);
        print(_appName);

        return newApplication;
    }


    public void EndCurrentApplication()
    {
        if(currentApplication != null && currentApplication.active)
        {
            currentApplication.Deactivate();
        }
    }

    public float CurrentApplicationRuntime()
    {
        return currentApplication.sectionActive;
    }

}
