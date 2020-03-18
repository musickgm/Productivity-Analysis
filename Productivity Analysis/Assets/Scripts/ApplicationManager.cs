using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds all the apps
/// </summary>
public class ApplicationManager : Singleton<ApplicationManager>
{
    public List<App> usedApplications = new List<App>();                //List of "new" apps that have been used
    public List<App> standardApplications = new List<App>();            //Predefined list of standard app likely to be used

    private App currentApplication;                                     //The current app being used
        

    /// <summary>
    /// Called by "Determine Application" - sets the current app and makes new apps if necessary
    /// </summary>
    /// <param name="appName"></param>
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

    /// <summary>
    /// Checks to see if the app has already been used
    /// </summary>
    /// <param name="_appName"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Checks to see if the app is in the standard list
    /// </summary>
    /// <param name="_appName"></param>
    /// <returns></returns>
    private App ApplicationFromStandardList(string _appName)
    {
        for (int i = 0; i < standardApplications.Count; i++)
        {
            if (_appName.Contains(standardApplications[i].searchKey))
            {
                return standardApplications[i];
            }
        }
        return null;
    }

    /// <summary>
    /// If the app hasn't been used and not in the standard list, make a new app
    /// </summary>
    /// <param name="_appName"></param>
    /// <returns></returns>
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


    /// <summary>
    /// End current application
    /// </summary>
    public void EndCurrentApplication()
    {
        if(currentApplication != null && currentApplication.active)
        {
            currentApplication.Deactivate();
        }
    }

    /// <summary>
    /// Returns how long the current application has been running. 
    /// </summary>
    /// <returns></returns>
    public float CurrentApplicationRuntime()
    {
        return currentApplication.sectionActive;
    }

}
