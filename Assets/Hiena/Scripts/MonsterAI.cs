using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MonsterAI : MonoBehaviour
{

    private Rigidbody rb;

    //private System.Random rng;

    [SerializeField] private Vector2 timersForMovement;
    private float timer;
    private float justChangedState;

    [SerializeField] private float movSpeed;
    private Vector3 velocity;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
        //rng = new System.Random();

        timer = Random.Range(timersForMovement.x, timersForMovement.y);
        Debug.Log(timer);
        justChangedState = 0f;

        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y = rb.velocity.y;

        if (Time.time - justChangedState > timer)
        {
            justChangedState = Time.time;
            
            
            velocity.x = Random.Range(-1, 2) * movSpeed;

            Debug.Log(velocity.x);

            animator.SetFloat("MovSpeed", Mathf.Abs(velocity.x));

            if(velocity.x != 0)
                timer = Random.Range(timersForMovement.x / 3, timersForMovement.y / 3);
            else
                timer = Random.Range(timersForMovement.x, timersForMovement.y);
        }

        if(!rb.isKinematic)
            rb.velocity = velocity;

        FlipPlayer();
    }

    private void FlipPlayer()
    {
        if (rb.velocity.x < 0 && transform.right.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        }
        else if (rb.velocity.x > 0 && transform.right.x < 0)
        {
            transform.rotation = Quaternion.identity;

        }

    }
}
