using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement")]
    public bool canMove;
    public float speed;
    public float sprintSpeed;

    [Header("Looking Around")]
    public bool canLook;
    public float lookSpeed;

    private Rigidbody2D myBody;

    // Start is called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(canMove)
            move();

        if (canLook)
            faceMouse();
    }

    private void move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector3 moveDir =  new Vector3(xInput, yInput, 0);
        moveDir.Normalize();

        myBody.velocity = moveDir * (speed + (sprintSpeed * Input.GetAxisRaw("Sprint")));
    }

    private void faceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        transform.right = Vector3.MoveTowards(transform.right, direction, lookSpeed * Time.deltaTime);
        if(transform.rotation.eulerAngles.y == 180)
        {
            Quaternion targetRot = Quaternion.Euler(0, 0, 180);
            transform.rotation = targetRot;
        }
    }
}
