using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using System.Text;

public class DetermineApplication : MonoBehaviour
{
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    public ApplicationManager applicationManager;

    private string currentApplication;
    private string activeApplication;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        activeApplication = GetActiveWindowTitle();
        if(activeApplication == null)
        {
            return;
        }
        if(currentApplication != null)
        {
            if (activeApplication.Contains(currentApplication))
            {
                return;
            }
        }


        currentApplication = activeApplication;
        applicationManager.SetApplication(currentApplication);
        //print(currentApplication);


    }

    //https://stackoverflow.com/questions/115868/how-do-i-get-the-title-of-the-current-active-window-using-c
    private string GetActiveWindowTitle()
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }
        return null;
    }
}
