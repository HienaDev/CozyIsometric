using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Station))]
public class Hatchery : MonoBehaviour
{

    private Station station;

    [SerializeField] private int expPerSecond;



    // Start is called before the first frame update
    void Start()
    {
        station = GetComponent<Station>();  
    }

    // Update is called once per frame
    void Update()
    {
        GiveExp(expPerSecond);
    }

    private void GiveExp(int exp)
    {
        station.PopUp.SetActive(false);

        foreach (MonsterController monster in station.MonstersWorking)
        {
            station.PopUp.SetActive(monster.AddExp(exp * Time.deltaTime));
        }
        
    }
}
