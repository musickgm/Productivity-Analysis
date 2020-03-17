using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public Page defaultPage;

    private Page currentPage;

    private void Awake()
    {
        Screen.fullScreen = false;
        SetNewPage(defaultPage);
    }

    public void SetNewPage(Page newPage)
    {
        if(currentPage != null)
        {
            currentPage.DeactivatePage();
        }
        newPage.ActivatePage();
        currentPage = newPage;
    }

    private void OnApplicationFocus(bool focus)
    {
        SetNewPage(currentPage);
    }


}
