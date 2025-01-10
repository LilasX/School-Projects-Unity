using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviors : MonoBehaviour
{

    private Rigidbody rigid; //Component Rigidbody of the ball
    public float minTranslation = 0.05f; //Minimum movement of the ball is 0.05f unit
    private GameManager manager; //Reference of the GameManager (singleton) 

    private AudioSource audio; // My audio source

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent <AudioSource>(); // Hide Audio Source
        manager = GameManager.instance; //Recuperate the instance of the GameManager in my scene
        rigid = GetComponent<Rigidbody>(); //Recuperate the component Rigidbody
    }

    void Move(Vector3 colPos)
    {
        float diffX = minTranslation + (colPos.x - transform.position.x) * Random.Range(1, 10); //Position X of the collision and the position X of the ball
        float diffZ = minTranslation + (colPos.z - transform.position.z) * Random.Range(1, 10); //Position Z of the collision and the position Z of the ball
        rigid.velocity = new Vector3(-diffX, 8, -diffZ); //Change direction of the ball
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 colPos = collision.transform.position; //Position of the object that collides with the ball
        if(collision.gameObject.tag == "Paddle")
        {
            audio.Play(); //Play audio
            Move(colPos);
            manager.AddScore(); //Call the method AddScore() in the GameManager
            //Debug.Log("X: " + diffX + ", Z: " + diffZ);
        }
        else //if the ball to any other collider than the paddle
        {
            manager.GameOver(this.gameObject); //We lose
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
