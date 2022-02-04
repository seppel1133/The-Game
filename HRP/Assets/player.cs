using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Rigidbody rb;

    public Animator animator;

    public float moveSpeed = 5f;
    public float adjustFrontSpeed = 1f;

    // Start is called before the first frame update

    // Update is called once per frame

    void FixedUpdate()
    {
        Vector3 m_Input = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")*adjustFrontSpeed);
        animator.SetFloat("Horizontal",Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("Vertical",Input.GetAxisRaw("Vertical"));
        animator.SetFloat("Speed",m_Input.sqrMagnitude);

        rb.MovePosition(transform.position + m_Input * Time.deltaTime*moveSpeed);
    }
}

