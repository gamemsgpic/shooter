using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public readonly int hashMove = Animator.StringToHash("Move");

    public float moveSpeed = 1f;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 dir;
    //private float moveHorizontal;
    //private float moveVertical;
    private bool moveNow;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        moveNow = false;
    }

    private void Update()
    {
        //moveHorizontal = Input.GetAxisRaw("Horizontal");
        //moveVertical = Input.GetAxisRaw("Vertical");
        //var pos = transform.position;
        //pos += moveHorizontal * transform.forward * moveSpeed * Time.deltaTime;
        //
        //rb.MovePosition(pos);

        dir = Vector3.zero;
        moveNow = false;
        animator.SetBool(hashMove, moveNow);

        if (Input.GetKey(KeyCode.A))
        {
            dir += Vector3.left;
            moveNow = true;
            animator.SetBool(hashMove, moveNow);
        }

        if (Input.GetKey(KeyCode.D))
        {
            dir += Vector3.right;
            moveNow = true;
            animator.SetBool(hashMove, moveNow);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir += Vector3.forward;
            moveNow = true;
            animator.SetBool(hashMove, moveNow);
        }

        if (Input.GetKey(KeyCode.S))
        {
            dir += Vector3.back;
            moveNow = true;
            animator.SetBool(hashMove, moveNow);
        }

        // 이동 처리
        transform.position += dir.normalized * moveSpeed * Time.deltaTime;

    }
}
