using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCommands : MonoBehaviour {

    public GameObject cube1;
    Vector3 originalPosition;

    // Use this for initialization
    void Start()
    {
        // Grab the original local position of the sphere when the app starts.
        originalPosition = this.transform.localPosition;
    }
    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {

        //// Do a raycast into the world based on the user's
        //// head position and orientation.
        //var headPosition = Camera.main.transform.position;
        //var gazeDirection = Camera.main.transform.forward;

        //RaycastHit hitInfo;
        //if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        //{
        //    // If the raycast hit a hologram, use that as the focused object.
        //    //FocusedObject = hitInfo.collider.gameObject;
        //    Debug.Log("hitInfo.point = " + hitInfo.point);
        //    cube1.transform.position = hitInfo.point;
        //}
        //else
        //{
        //    // If the raycast did not hit a hologram, clear the focused object.
        //    //FocusedObject = null;
        //}
        // Do a raycast into the world that will only hit the Spatial Mapping mesh.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        // Ensure you specify a length as a best practice. Shorter is better as
        // performance hit goes up roughly linearly with length.
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
            15.0f))
        {
            // Move this object to where the raycast hit the Spatial Mapping mesh.

            //this.transform.position = hitInfo.point;

            // Rotate this object to face the user.
            Quaternion rotation = Camera.main.transform.localRotation;
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = rotation;

            Debug.Log("instantiate at " + hitInfo.point + "  with rotation " + rotation);
            Instantiate(cube1, hitInfo.point, rotation);


        }



        //// If the sphere has no Rigidbody component, add one to enable physics.
        //if (!this.GetComponent<Rigidbody>())
        //{
        //    var rigidbody = this.gameObject.AddComponent<Rigidbody>();
        //    rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        //}
    }
}
