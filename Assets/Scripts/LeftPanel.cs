using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeftPanel : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float maxYPosition = 4.5f;
    private bool goUp = false;
    private bool goDown = false;

    void Update()
    { 
        CheckInput();     
        MovePanel();
    }

    private void CheckInput()
    {
        
        goUp = Input.GetKey(KeyCode.W);
        goDown = Input.GetKey(KeyCode.S);

    }

    private void MovePanel()
    {   
        Rigidbody rb = GetComponent<Rigidbody>();

        Vector3 movement = Vector3.zero;
        if (goUp)
            movement = Vector3.up;
        else if (goDown)
            movement = Vector3.down;

        rb.velocity = movement * moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border") || collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
    }
}