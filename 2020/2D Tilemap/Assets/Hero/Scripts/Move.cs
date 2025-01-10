using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

namespace UnityEngine.Tutorials
{
    public class Move : MonoBehaviour
    {
        Rigidbody2D rigid;
        Animator anim;
        Vector2 dir = new Vector2();
        public float speed = 300.0f;

        // Start is called before the first frame update
        void Start() {
            rigid = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }
        //public void OnMove(InputAction.CallbackContext context) 
        //{
        //  dir = context.ReadValue<Vector2>();
	    //}
        public void OnMove() 
        {
            dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
	    }
        private void Animate()
        {
            anim.SetFloat("Hor", dir.x);
            anim.SetFloat("Ver", dir.y);
            anim.SetFloat("Magnitude", rigid.velocity.magnitude);
        }
	    private void Movement()
        {
		    Vector2 pos = new Vector2();
            pos += (dir.normalized*speed)*Time.fixedDeltaTime;
            rigid.velocity = pos;
	    }
	    private void Update()
        {
		     OnMove();
	    }
	    void FixedUpdate() {
            Animate();
            Movement();
        }
    }
}
