using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{

    Rigidbody2D rigid; //Reference of our component rigidbody
    Vector2 dir = new Vector2(); //Direction of movement
    public float speed = 300.0f; //Velocity force
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); //Cache of the component
        anim = GetComponent<Animator>(); //Cache of the component
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        dir = context.ReadValue<Vector2>();
    }

    private void Animate()
    {
        anim.SetFloat("Magnitude", rigid.velocity.magnitude);
        anim.SetFloat("Hor", dir.x);
        anim.SetFloat("Ver", dir.y);
    }

    private void Movement()
    {
        Vector2 pos = new Vector2();
        pos += (dir.normalized * speed) * Time.fixedDeltaTime;
        rigid.velocity = pos;
    }

    // Update is called once per frame
    void Update()
    {
        //OnMove();
    }

    private void FixedUpdate()
    {
        Animate();
        Movement();
    }
}
