using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle the pages and transitioning
/// </summary>
public class PageManager : MonoBehaviour
{
    public Page defaultPage;                //The page to start on

    private Page currentPage;               //The page currently being used

    /// <summary>
    /// This was an arbitrary place to make sure we have a windowed app. Also set the default page
    /// </summary>
    private void Awake()
    {
        Screen.fullScreen = false;
        SetNewPage(defaultPage);
    }

    /// <summary>
    /// Called by button press 
    /// </summary>
    /// <param name="newPage"></param> Page parameter to activate
    public void SetNewPage(Page newPage)
    {
        //Deactivate the previous page
        if(currentPage != null)
        {
            currentPage.DeactivatePage();
        }
        //Activate new page
        newPage.ActivatePage();
        currentPage = newPage;
    }

    /// <summary>
    /// When we refocus on the app, set the current page and run analytics
    /// </summary>
    /// <param name="focus"></param>
    private void OnApplicationFocus(bool focus)
    {
        SetNewPage(currentPage);
    }


}
