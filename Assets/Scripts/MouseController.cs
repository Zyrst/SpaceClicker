using UnityEngine;
using System.Collections;

public class MouseController {

    /// <summary>
    /// lock the clicks
    /// </summary>
    public bool _setLocked = false;
    public bool _locked = false;
    public bool locked
    {
        get { return _locked; }
        set
        {
            if (!value)
            {
                _setLocked = true;
            }
            else
            {
                _locked = true;
            }
        }
    }

    public Vector3 position
    {
        get
        {
            return Input.mousePosition;
        }
    }

    public Vector3 worldPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(position);
        }
    }

    public bool buttonDown
    {
        get
        {
            return Input.GetMouseButton(0);
        }
    }

    public bool clickButtonDown
    {
        get
        {
            return buttonDown && !_locked;
        }
    }


    public void Update()
    {
    }

    public void LateUpdate()
    {
        if (_setLocked && !buttonDown)
        {
            _locked = false;
            _setLocked = false;
        }
    }

    private static MouseController _instance = null;
    public static MouseController Instance
    {
        get
        {
            if (_instance == null)
                _instance = new MouseController();

            return _instance;
        }
    }
}
