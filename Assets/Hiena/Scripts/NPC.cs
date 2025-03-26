using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private IsometricTile headingToTile;
    private IsometricTile starTile;
    private IsometricTile endTile;
    private IsometricTile currentTile;

    [SerializeField] private float speed;

    private List<IsometricTile> path;

    private Pathfinding pathfinding;

    [SerializeField] private SO_NPC npcInfo;

    private SpriteRenderer sr;

    private MapManager map;

    public enum State
    {
        IDLE,
        MOVING
    }

    public enum Direction
    {
        UPRIGHT, // -16 -8 -0.01
        UPLEFT, // 16 -8 -0.01
        DOWNRIGHT, // -16 8 0.01
        DOWNLEFT // 16 8 0.01
    }
    // Start is called before the first frame update
    void Awake()
    {

        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(path.Count == 0)
            {
                StartMovement(currentTile, map.GetRandomNode());
            }
            else
                UpdatePath(currentTile, map.GetRandomNode());
        }
    }

    public void Initalize(IsometricTile startPosition, IsometricTile endPosition, MapManager map)
    {
        this.map = map;

        StartMovement(startPosition, endPosition);
    }

    public void StartMovement(IsometricTile startTile, IsometricTile endTile)
    {
        currentTile = startTile;
        UpdatePath(startTile, endTile);
        MoveToTile();
    }

    public void UpdatePath(IsometricTile startTile, IsometricTile endTile)
    {
        Debug.Log(startTile);
        Debug.Log(endTile);
        Debug.Log(map);
        
        if(pathfinding == null)
        {
            pathfinding = new Pathfinding();
        }
        Debug.Log(pathfinding);
        path = pathfinding.FindPath(startTile, endTile, map);

    }

    public void UpdatePathAfterNewBlock()
    {
        if(path.Count > 0)
            UpdatePath(currentTile, endTile);
    }

    private void MoveToTile()
    {
        if (path.Count == 0)
        {
            return;
        }

        StartCoroutine(MoveToTileCR(path[0]));
        path.RemoveAt(0);
    }

    private IEnumerator MoveToTileCR(IsometricTile tile)
    {
        Vector3 startPosition = currentTile.transform.localPosition + Vector3.up * 16f;
        Vector3 endPosition = tile.transform.localPosition + Vector3.up * 16f;

        Debug.Log("start end diff:" + (endPosition - startPosition));

        Vector2 diff = endPosition - startPosition;


        switch(diff)
        {
            case Vector2 v when v.x > 0 && v.y > 0:
                sr.sprite = npcInfo.backSprite;
                sr.flipX = true;
                Debug.Log("UPRIGHT");
                break;
            case Vector2 v when v.x < 0 && v.y > 0:
                sr.sprite = npcInfo.backSprite;
                sr.flipX = false;
                Debug.Log("UPLEFT");
                break;
            case Vector2 v when v.x > 0 && v.y < 0:
                sr.sprite = npcInfo.frontSprite;
                sr.flipX = true;
                Debug.Log("DOWNRIGHT");
                break;
            case Vector2 v when v.x < 0 && v.y < 0:
                sr.sprite = npcInfo.frontSprite;
                sr.flipX = false;
                Debug.Log("DOWNLEFT");
                break;
        }

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }
        transform.localPosition = endPosition;

        currentTile = tile;

        MoveToTile();
    }
}
