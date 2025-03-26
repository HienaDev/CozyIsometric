using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{

    [SerializeField] private CreateIsometricFloor createIsometricFloor;
    [SerializeField] private NPC npc;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        SpawnNpc(createIsometricFloor.map.GetRandomNode(), createIsometricFloor.map.GetRandomNode());
    }

    public void SpawnNpc(IsometricTile startPosition, IsometricTile endPosition)
    {
        NPC NPC = Instantiate(npc, createIsometricFloor.transform);

        NPC.Initalize(startPosition, endPosition, createIsometricFloor.map);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
