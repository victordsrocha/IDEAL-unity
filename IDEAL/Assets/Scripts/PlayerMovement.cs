using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Walk,
    Attack,
    Interact
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private VisionMultipleRays fieldOfView;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    private int direction = 4;
    public PlayerState currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.Walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        // se preferir com aceleração basta trocar o GetAxisRaw por GetAxis
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        direction = SetDirection();
        fieldOfView.SetAimDirection(SetVectorDirection());
        fieldOfView.SetOrigin(transform.position);

        if (Input.GetButtonDown("Attack") && currentState != PlayerState.Attack)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.Walk)
        {
            UpdateAnimationAndMove();
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.Attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.Walk;
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }

    float SetVectorDirection()
    {
        float a = 0f;
        if (this.direction == 0)
        {
            a = 90f;
        }
        else if (this.direction == 1)
        {
            a = 45f;
        }
        else if (this.direction == 2)
        {
            a = 0f;
        }
        else if (this.direction == 3)
        {
            a = -45f;
        }
        else if (this.direction == 4)
        {
            a = -90f;
        }
        else if (this.direction == 5)
        {
            a = -135f;
        }
        else if (this.direction == 6)
        {
            a = 180f;
        }
        else if (this.direction == 7)
        {
            a = 135f;
        }

        return a;
    }

    int SetDirection()
    {
        int dir = -1;
        if (change.x > 0)
        {
            if (change.y > 0)
            {
                dir = 1;
            }
            else if (change.y == 0)
            {
                dir = 2;
            }
            else
            {
                dir = 3;
            }
        }
        else if (change.x == 0)
        {
            if (change.y > 0)
            {
                dir = 0;
            }
            else if (change.y == 0)
            {
                // retorna get direction
                dir = this.direction;
            }
            else
            {
                dir = 4;
            }
        }
        else if (change.x < 0)
        {
            if (change.y > 0)
            {
                dir = 7;
            }
            else if (change.y == 0)
            {
                dir = 6;
            }
            else
            {
                dir = 5;
            }
        }

        return dir;
    }
}