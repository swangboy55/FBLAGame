using UnityEngine;
using System.Collections;

public abstract class PlatformScript : MonoBehaviour
{
    protected Vector2 normalCompare;

    public void Start()
    {
        Vector3 rotation = GetComponent<Transform>().rotation.eulerAngles;
        normalCompare = (new Vector2(Mathf.Cos(rotation.z), Mathf.Sin(rotation.z))).normalized;
    }

    protected bool shouldApplyEffect(Vector2 hitNormal)
    {
        Vector2 hnn = hitNormal.normalized;
        //large tolerance to account for float inacc in normalcompare calculation
        //as well as there being only 4 possible states for the hitNormal = normalCompare test
        return (Mathf.Abs(hnn.x - normalCompare.x) < 0.07f) && (Mathf.Abs(hnn.y - normalCompare.y) < 0.07f);
    }

    public abstract void OnPlayerCollide(GameObject player, Vector2 hitNormal, Vector2 hitVelocity);
}
