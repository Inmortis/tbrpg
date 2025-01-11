using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string transitionName;

    void Start()
    {
        if(transitionName == PlayerController.instance.areaTransitionName)
            PlayerController.instance.transform.position = this.transform.position;

        UIFade.instance.FadeFromBlack();

        try
        {
            GameManager.instance.fadingBetweenAreas = false;
        }
        catch
        {
            Debug.Log("GameManager has not been loaded...");
        }
    }

    void Update()
    {
        
    }
}
