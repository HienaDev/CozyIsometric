using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Station))]
public class EvolutionChamber : MonoBehaviour
{

    private Station station;

    // Start is called before the first frame update
    void Start()
    {
        station = GetComponent<Station>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (MonsterController monster in station.MonstersWorking)
        {
            if(monster == null)
            {
                station.CheckForNull();
                break;
            }
            else
                monster.Evolve();
        }
    }
}
