using Kirurobo;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WindowController : MonoBehaviour
{

    private UniWindowController uniwinc;

    [SerializeField] private Toggle transparentToggle;

    [SerializeField] private Toggle topmostToggle;

    private int screenNumber;

    

    // Start is called before the first frame update
    void Start()
    {

        

        // UniWindowController を探す
        uniwinc = UniWindowController.current;

        UpdateUI();

        if (uniwinc)
        {

            uniwinc.isTransparent = true;
            uniwinc.isTopmost = true;

            // UIを操作された際にはウィンドウに反映されるようにする
            transparentToggle?.onValueChanged.AddListener(val => uniwinc.isTransparent = val);

            topmostToggle?.onValueChanged.AddListener(val => uniwinc.isTopmost = val);
        }

        UpdateScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        if (uniwinc)
        {
            if (transparentToggle)
            {
                transparentToggle.SetIsOnWithoutNotify(uniwinc.isTransparent);
            }


            if (topmostToggle)
            {
                topmostToggle.SetIsOnWithoutNotify(uniwinc.isTopmost);
            }
        }
    }

    public void UpdateScreen()
    {
        screenNumber++;

        if (screenNumber >= uniwinc.monitorCount)
        {   
            screenNumber = 0;
        }

        uniwinc.monitorToFit = screenNumber;
    }
}
