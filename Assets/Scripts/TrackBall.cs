using UnityEngine;
using System.Collections;

public class TrackBall : MonoBehaviour {

    public GameObject referenceObject;
    public float DampingFactor = 0;
    private Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        velocity = new Vector3(0, 0, 0);
        //destination = new Vector3(referenceObject.transform.position.x, referenceObject.transform.position.y, -10);
    }

    // Update is called once per frame
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
