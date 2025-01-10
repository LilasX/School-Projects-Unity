using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePaddle : MonoBehaviour
{

    public float speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0)
        {
            return; // Interrupt method
        }
        Vector3 transf = new Vector3();
        transf.x = (Input.GetAxis("Mouse X") * speed) * (Time.deltaTime / Time.timeScale);
        transf.z = (Input.GetAxis("Mouse Y") * speed) * (Time.deltaTime / Time.timeScale);

        transform.Translate(transf);
    }
}
