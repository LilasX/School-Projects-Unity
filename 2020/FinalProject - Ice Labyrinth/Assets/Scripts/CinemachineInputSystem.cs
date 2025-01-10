using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CinemachineInputSystem : MonoBehaviour
{

    private CinemachineFreeLook vCam; //reference to our Cinemachine Free Look

    // Start is called before the first frame update
    void Start()
    {
        vCam = GetComponent<CinemachineFreeLook>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 v = context.ReadValue<Vector2>();
        vCam.m_XAxis.m_InputAxisValue = v.x; //passes at Axis X
        vCam.m_YAxis.m_InputAxisValue = v.y; //passes at Axis Y
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
// Reference
// 1- Copied from the CinemachineInputSysten script from class project Integration de personnage