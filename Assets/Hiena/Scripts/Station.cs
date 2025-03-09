using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Station : MonoBehaviour
{

    [SerializeField] private MonsterStates.Job[] jobsThatCanWorkHere;
    private List<MonsterController> monstersWorking;
    public List<MonsterController> MonstersWorking {  get { return monstersWorking; } }

    [SerializeField] private GameObject popUp;
    public GameObject PopUp { get { return popUp; } }

    private void Start()
    {
        monstersWorking = new List<MonsterController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        MonsterController monsterController = other.gameObject.GetComponent<MonsterController>();

        if (monsterController != null)
        {
            //Debug.Log(monsterController.Monster.name);
            if (jobsThatCanWorkHere.Intersect(monsterController.Monster.jobs).Count() > 0)
            {
                monstersWorking.Add(monsterController);
                Debug.Log(monsterController.Monster.monsterName + " was added to " + gameObject.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MonsterController monsterController = other.gameObject.GetComponent<MonsterController>();
        if (monsterController != null)
        {
            if (monstersWorking.Contains(monsterController))
            {
                monstersWorking.Remove(monsterController);
                Debug.Log(monsterController.Monster.monsterName + " was removed from " + gameObject.name);
            }
        }
    }

    private void OnDisable()
    {
        monstersWorking = new List<MonsterController>();
    }

    public void CheckForNull()
    {
        List<MonsterController> listToRemove = new List<MonsterController>();

        foreach (MonsterController controller in monstersWorking)
        {
            if (controller == null)
            {
                listToRemove.Add(controller);
            }
        }

        foreach(MonsterController controller in listToRemove)
        {
            monstersWorking.Remove(controller);
        }
    }
}
