using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveIfOthers : MonoBehaviour
{

    [SerializeField] private GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.SetActive(false);

        foreach (GameObject obj in objects)
        {
            if (obj.activeSelf)
                gameObject.SetActive(true);
        }
    }
}
