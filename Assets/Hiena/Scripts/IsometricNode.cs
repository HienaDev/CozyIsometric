using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricNode : MonoBehaviour
{

    public int G;
    public int H;
    public int F => G + H;

    public bool isWalkable = true;

    public IsometricNode previous;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
