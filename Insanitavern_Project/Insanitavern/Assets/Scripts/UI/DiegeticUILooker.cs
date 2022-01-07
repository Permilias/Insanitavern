using UnityEngine;

public class DiegeticUILooker : MonoBehaviour
{
    public bool looks;
    public bool scales;
    public Transform lookingTransform;
    public Transform scalingTransform;
    Camera cam;
    public float distanceMultiplier;
    float dist;
    float scale;
    public float maxSize;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        //face player
        if(looks)
        lookingTransform.transform.LookAt(cam.transform);

        //correct size depending on distance
        if(scales)
        {
            dist = Vector3.Distance(lookingTransform.position, cam.transform.position);
            scale = dist * distanceMultiplier;
            if (scale > maxSize) scale = maxSize;
            scalingTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
