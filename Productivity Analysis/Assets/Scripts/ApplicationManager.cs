using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ApplicationManager : MonoBehaviour
{
    //timeline of applications
    public List<App> usedApplications = new List<App>();
    public List<App> standardApplications = new List<App>();

    private App currentApplication;


    public void SetApplication(string appName)
    {
        //deactivate previous application
        if (currentApplication != null)
        {
            currentApplication.Deactivate();
        }
        currentApplication = null;

        currentApplication = ApplicationFromUsedList(appName);
        if(currentApplication == null)
        {
            currentApplication = ApplicationFromStandardList(appName);
        }
        if(currentApplication ==  null)
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
            if (_appName.Contains(usedApplications[i].appName))
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
            if (_appName.Contains(standardApplications[i].appName))
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
        usedApplications.Add(newApplication);
        print(_appName);

        return newApplication;
    }


}
