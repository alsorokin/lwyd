using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayermoveScript : MonoBehaviour {
    public float speed = 6.0F;
    private Vector2 moveDirection = Vector2.zero;

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection *= speed;

        rb.AddForce(moveDirection);
        //.Move(moveDirection * Time.deltaTime);
    }
}
