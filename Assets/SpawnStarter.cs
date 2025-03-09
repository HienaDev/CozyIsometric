using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStarter : MonoBehaviour
{

    [SerializeField] private GameObject leftStarter;
    [SerializeField] private GameObject middleStarter;
    [SerializeField] private GameObject rightStarter;


    public void InstantiateLeftStarter()
    {
        Instantiate(leftStarter);
    }

    public void InstantiateMiddleStarter()
    {
        Instantiate(middleStarter);
    }

    public void InstantiateRightStarter()
    {
        Instantiate(rightStarter);
    }
}
