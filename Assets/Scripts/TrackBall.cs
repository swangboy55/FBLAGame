using UnityEngine;
using System.Collections;

//Script to have the camera track the player ingame, delays camera movement to make it smoother/more challening.
public class TrackBall : MonoBehaviour {

    public GameObject referenceObject;
    public float DampingFactor = 0;
    private Vector3 velocity;

    // Use this for initialization.
    //initial velocity of the moving camera to zero
    void Start()
    {
        velocity = new Vector3(0, 0, 0);    
    }


    /// <summary>
    ///  Update is called once per frame
    ///  track the character object(referenceObject) with the camera, and smoothdamp movement so it flows nicely.
    /// </summary>

    void FixedUpdate()
    {
        if (referenceObject != null)
        {
            Camera cam = GetComponent<Camera>();
            Vector3 point = cam.WorldToViewportPoint(referenceObject.transform.position);
            Vector3 delta = referenceObject.transform.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 dest = cam.transform.position + delta;
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, dest, ref velocity, DampingFactor);

        }
    }
}
