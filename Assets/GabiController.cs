using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabiController : MonoBehaviour
{
    public Animator animator;

    private Rigidbody2D rb;

    [SerializeField] private float speed = 5f;

    private bool rotating = false;

    [SerializeField] private float timeWalking = 5f;
    private float startedWalking;
    [SerializeField] private float timeIdling = 5f;
    private float startedIdling;
    int speedDirection = 1;
    public enum State
    {
        Idle,
        Walk,
        Flying,
        Grabbed,
        None
    }

    public State currentState;
    private Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Idle;
        startedWalking = 0f;
        startedIdling = 0f;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rb.velocity.magnitude);
        if(rb.velocity.magnitude < 6f && currentState == State.Flying )
        {
            RotateToDefault();
        }

        if(!rotating && currentState == State.Idle && Time.time - startedIdling > timeIdling)
        {
            ChangeState((int)State.Walk);
            speedDirection = Random.Range(0, 2) == 0 ? -1 : 1;
            GetComponent<SpriteRenderer>().flipX = speedDirection == -1;
            startedWalking = Time.time;
        }

        if(!rotating && State.Walk == currentState)
        {
            rb.velocity = new Vector2(speed * speedDirection, rb.velocity.y);
        }

        if(!rotating && currentState == State.Walk && Time.time - startedWalking > timeWalking)
        {
            ChangeState((int)State.Idle);
            startedIdling = Time.time;
        }
    }

    public void RotateToDefault()
    {
        if (currentState != State.Idle)
        {
            rb.velocity = Vector2.zero;
            currentCoroutine = StartCoroutine(LerpRotationToZero());
            ChangeState((int)State.Idle);
        }
    }

    private IEnumerator LerpRotationToZero()
    {
        rotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0f, 0f, 0f);
        float time = 0f;


        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * 20f;

        while (time < 0.5f)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time / 0.5f);
            transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure it's exactly zero at the end
        transform.rotation = endRotation;
        rotating = false;
    }

    public void ChangeState(int state)
    {

        switch (state)
        {
            case (int)State.Idle:
                animator.SetTrigger("isIdle");


                currentState = State.Idle;
                break;
            case (int)State.Walk:

                animator.SetTrigger("isWalk");
 
                currentState = State.Walk;
                break;
            case (int)State.Flying:
                animator.SetTrigger("isIdle");

                currentState = State.Flying;
                break;
            case (int)State.Grabbed:

                animator.SetTrigger("isFlying");
                if(rotating)
                {
                    StopCoroutine(LerpRotationToZero());
                    rotating = false;
                }
                currentState = State.Grabbed;
                break;
            default:
                break;
        }
    }
}
