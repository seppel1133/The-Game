using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;

   // public Rigidbody2D rb;
   

    // Update is called once per frame
    void Update()
    {
      //  ProcessInputs();

    }

    void FixedUpdate()
    {
    //    Move();
    }

   // void ProcessInputs()
   // {
       // float moveX = Input.GetAxisRaw("Horizontal");
       // float moveY = Input.GetAxisRaw("Vertical");

       // movedir = new Vector2(moveX, moveY);
    //}

   // void Move()
    //{
        //rb.velocity = new Vector2(movedir.x * moveSpeed, movedir.y * moveSpeed);
    //}
}
