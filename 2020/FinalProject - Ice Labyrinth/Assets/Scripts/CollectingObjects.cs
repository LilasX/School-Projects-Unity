using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectingObjects : MonoBehaviour
{

    [SerializeField] private GameObject[] capsules; //Reference to the 4 capsules in each corner
    [SerializeField] private GameObject[] spheres; //Reference to all the spheres to be collected
    [SerializeField] private GameObject[] dangerObjects; //Reference to the pink objects that can cause death to the character
    private GameManager manager; //Reference to our Game Manager
    private Camera cameraMap; //Reference to our mapCamera
    private int spheresCollected = 0; //spheres collected
    private int capsulesCollected = 0; //capsules collected
    private const string predanger = "Danger Off for "; //pretext before showing the time until DangerObjects are back on
    private float timerSafe; //time 
    private bool timerOn = false; //bool for update
    [SerializeField] private Text timer; //Reference for the title
    [SerializeField] private Text warning; //Reference for the warning when the deadly objects are back on
    [SerializeField] private Text disappeared; //Reference for the warning when the deadly objects are off
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.instance; //Cache of our game manager
        cameraMap = manager.mapCamera.GetComponent<Camera>(); //cache of mapCamera
        timer.enabled = false; //only enable when a capsule was collected
        warning.enabled = false; //when the deadly object are back
        disappeared.enabled = false; //when the deadly objects disappear
    }

    private void OnCollisionEnter(Collision collision)
    {
        for (var i = 0; i < capsules.Length; i++) //get the size of array
        {
            //var fourCapsules = capsules[i]; //Reference to the 4 capsules

            if (collision.gameObject.tag == "Capsule") //if objects have Capsule tag
            {
                collision.transform.parent = gameObject.transform; //capsule becomes child of character
                collision.gameObject.transform.localPosition = new Vector3(0, 1, 0); //put the capsule so it is centered on the character
                collision.collider.enabled = false; //remove collider or it will affect the number of spheres collected counted (+ 2 instead of + 1 because there are 2 colliders)
                timerOn = true; //timer is active
                timerSafe = 30f; //20 secs
                timer.enabled = true; //show timer
                disappeared.enabled = true;
                Invoke("DisappearOff", 3);//deactivate message
                Invoke("DangerObjectsOff", 0); //deactivate objects with Death tag
                Invoke("DangerObjects", 30); //activate objects with Death tag
                Destroy(collision.gameObject, 30); //destroy capsule after 20 secs on the character
            }
        }
        if (collision.gameObject.tag == "Capsule")
        {
            capsulesCollected++; //capsules collected +1
        }

        for (var j = 0; j < spheres.Length; j++) //get the size of array
        {
            //var spherecollect = spheres[j]; //Reference to the spheres to collect

            if (collision.gameObject.tag == "Sphere") //if objects have Sphere tag
            {
                if (cameraMap.isActiveAndEnabled) //if the map is already showing
                {
                    CancelInvoke("TimeUpMap"); //map off
                    manager.ExposeMap(); //showing the map
                    Invoke("TimeUpMap", 5); //5 secs before the map goes off
                }
                else
                {
                    manager.ExposeMap(); //showing the map
                    Invoke("TimeUpMap", 5); //5 secs before the map goes off
                }
                Destroy(collision.gameObject); //destroy sphere collected

            }
        }
        if (collision.gameObject.tag == "Sphere")
        {
            spheresCollected++; //spheres collected + 1
        }

        if (spheresCollected == spheres.Length && capsulesCollected == capsules.Length) //all capsules and spheres collected
        {
            manager.Win(); //call this when all collected
            timer.enabled = false; //only enable when a capsule was collected
            warning.enabled = false; //when the deadly object are back
            disappeared.enabled = false; //when the deadly objects disappear
        }

        if(collision.gameObject.tag == "Death")
        {
            manager.Dead(); //when character collides with pink objects, call this method
        }

    }

    public void WarningOff()
    {
        warning.enabled = false; //deactive
    }

    public void DisappearOff()
    {
        disappeared.enabled = false; //deactivate
    }

    public void DangerObjects()
    {
        for(var k = 0; k < dangerObjects.Length; k++)
        {
            var pinkObjects = dangerObjects[k]; //Reference to deadly objects

            pinkObjects.SetActive(true); //deadly objects are active again
        }
        timerOn = false; //timer is gone
        timer.enabled = false; //disable timer text
        warning.enabled = true;
        Invoke("WarningOff", 3); //deactivate message
    }

    public void DangerObjectsOff()
    {
        for (var k = 0; k < dangerObjects.Length; k++)
        {
            var pinkObjects = dangerObjects[k]; //Reference to deadly objects

            pinkObjects.SetActive(false); //deadly objects are inactive
        }
    }

    public void TimeUpMap()
    {
        cameraMap.enabled = false; //map off
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            if (Mathf.FloorToInt(timerSafe % 60) > 0) //transform float in int value
            {
                timerSafe -= Time.deltaTime; //countdown 
                int secs = Mathf.FloorToInt(timerSafe % 60); //transform float in int secs
                timer.text = predanger + secs.ToString("D2") + "s"; //show timer
            }
            else
            {
                timerOn = false; //timer is gone
                timer.enabled = false; //disable timer text
            }
        }
    }
}
