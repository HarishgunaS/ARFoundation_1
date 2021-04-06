using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CubeInteraction : MonoBehaviour
{
    [SerializeField]
    GameObject cube;
    [SerializeField]
    Camera arCamera;
    [SerializeField]
    Material onTouch;
    [SerializeField]
    Material normal;
    [SerializeField]
    GameObject device;
    MeshRenderer mesh;
    Vector3 offset;
    Vector3 lastEuler;
    bool interacting = false;
    // Start is called before the first frame update
    void Start()
    {
        mesh =  cube.GetComponent<MeshRenderer>();
        offset = Vector3.zero;
        lastEuler = device.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        //assume no interaction
        bool justTouched = false;
        
        Ray ray;

        //only check if touch on screen
        if (Input.touchCount > 0)
        {
            ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.gameObject == cube)
                {
                    justTouched = true;
                    if (!interacting)
                    {
                        //if touching, interacting
                        interacting = true;
                        //record values at beginning of interaction
                        offset = cube.transform.position -  device.transform.position;
                        lastEuler = device.transform.rotation.eulerAngles;
                    }
                    //move and color if interacting (touched)
                    mesh.material = onTouch;
                    cube.transform.position =  device.transform.position + offset;
                    Vector3 currentEuler = device.transform.rotation.eulerAngles;
                    Vector3 rotation = currentEuler - lastEuler;
                    //cube.transform.RotateAround(device.transform.position, Vector3.up, rotation[0]);
                    cube.transform.RotateAround(device.transform.position, Vector3.up, rotation[1]);
                    //cube.transform.RotateAround(device.transform.position, Vector3.forward, rotation[2]);
                    lastEuler = currentEuler;
                    offset = cube.transform.position -  device.transform.position;
                }
                
            }
        }
        
        //if not touching, interaction is over
        if(!justTouched)
        {
            interacting = false;
            mesh.material = normal;
        }
        

        
        
    }

    void HandleRaycast(ARRaycastHit hit)
    {
        if ((hit.hitType & TrackableType.All) != 0)
        {
            return;
        }
    }
}
