using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Windows;

/// <summary>
/// Mouse data x,y
/// </summary>
public struct Point
{
    public int x;
    public int y;
}

/// <summary>
/// Tracks mouse position even outside of unity
/// </summary>
public class MouseInput : Singleton<MouseInput>
{
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out Point pos);

    Point cursorPos = new Point();
    Point previousPos = new Point();
    bool mouseMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        previousPos = cursorPos;
    }

    // Update is called once per frame
    void Update()
    {
        GetCursorPos(out cursorPos);

        if(previousPos.x == cursorPos.x && previousPos.y == cursorPos.y)
        {
            mouseMovement = false;
        }
        else
        {
            mouseMovement = true;
            previousPos = cursorPos;
        }

    }


    public bool IsMouseMoving()
    {
        return mouseMovement;
    }
}
