using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private MonsterStates monster;
    public MonsterStates Monster { get { return monster; } }
    private MonsterStates[] evolution;

    private float exp;
    private bool readyToEvolve;

    [SerializeField] private GameObject popUp;

    // Start is called before the first frame update
    void Start()
    {
        readyToEvolve = false;

        evolution = monster.evolutions;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddExp(float value)
    { 
        if(exp < monster.expToLvlUp)
        { 
            exp += value; 

            
        }

        return CheckForLvlUp();
    }

    public bool CheckForLvlUp()
    {
        if (exp >= monster.expToLvlUp)
        {
            readyToEvolve = true;
            popUp.SetActive(true);
            return true;
        }

        return false;
    }

    public void Evolve()
    {
        // Trigger animation of next monster
        if(readyToEvolve) 
        { 
            monster = evolution[Random.Range(0, evolution.Length)];

            
            Instantiate(monster.monsterPrefab);

           
            Destroy(gameObject);
        }
    }
}
