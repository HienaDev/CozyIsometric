using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster", fileName = "Monster")]
public class MonsterStates : ScriptableObject
{
    public enum Age
    {
        Egg,
        Baby,
        Adult,
        Elder
    }

    public enum Job
    {
        Baby,
        Nanny,
        Fighter,
        Farmer,
        Miner
    }

    public GameObject monsterPrefab;

    public string monsterName;
    public Age age;
    public int expToLvlUp;
    public Job[] jobs;
    public int[] jobsLvl;

    public MonsterStates[] evolutions;
    
    
}
