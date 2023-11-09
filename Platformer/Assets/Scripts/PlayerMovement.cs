using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private Vector2 v2_playerPosition = new Vector2(1, 1);
    private bool b_jump;
    private float f_jumpTime;
    private readonly float f_buttontime;
    [SerializeField] private LayerMask Ground;
    [SerializeField] Vector3 v3_boxsize;

    private enum MovementState { idle, running, jumping, falling }
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    //Checks using boxcast the a spacific layermask. It returns to true boolean if it is range.
    private void CharacterJump()
    {
        rb.velocity = new Vector2(v2_playerPosition.x, 2f);
    }

    private void CharacterJumpImpulse()
    {
        rb.AddForce(new Vector2(0, 400f));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * 0.55f, v3_boxsize);
    }
    private bool GroundCheck()
    {
        bool b_check = Physics2D.BoxCast(transform.position, v3_boxsize, 0,  -transform.up, 0.53f, Ground);
        return b_check;
    }
    // Update is called once per frame
    private void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && GroundCheck())
        {
            b_jump = true;
            f_jumpTime = 0;
        }
        if (b_jump)
        {
            CharacterJumpImpulse();
            f_jumpTime += Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") | f_jumpTime > f_buttontime)
        {
            b_jump = false;
        }
        UpdateAnimationState();
    }
   

    private float dirX = 0f;
    private void UpdateAnimationState()
    {

        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Death");
        transform.position = new Vector2(0, 2);
    }

}