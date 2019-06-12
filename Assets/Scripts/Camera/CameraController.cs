/*
    Copyright (c) 2018, Szymon Jakóbczyk
*/

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour {

    /// <summary>
    /// Controls the camera, uses keyboard and mouse input
    /// </summary>

    public static CameraController main;

    public PostProcessProfile PProcessProfile;



    public float panSpeed = 10f;
    public float smoothTime = 4f;

    public float zoomSpeed = 10f;
    Vector2 rotVel = new Vector2(0f,0f);
    float zoomVel = 0f;
    private DepthOfField DOF;

    void Awake()
    {
        main = this;
        PProcessProfile.TryGetSettings<DepthOfField>(out DOF);
    }

    // Update is called once per frame
    void Update() {
        Vector3 rot = transform.localEulerAngles;
        Vector3 scale = transform.localScale;
        var zoom = scale.z;

        if (Input.GetButton("Plus"))
        {
            zoomVel = -0.01f;
        }

        if (Input.GetButton("Minus"))
        {
            zoomVel = 0.01f;
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");


        zoomVel -= scroll * zoomSpeed * zoom * 0.005f;

        if (Input.touchCount == 2)
        {
            zoomVel -= zoomSpeed * Input.GetAxis("Mouse Y") * (Input.GetAxis("Vertical") + 1) * 0.001f;
        }

        if (Input.touchCount == 1)
        {
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Moved)
            {
                rotVel[0] += panSpeed * touch.deltaPosition.x * 0.004f;
                rotVel[1] -= panSpeed * touch.deltaPosition.y * 0.004f;
            }
        }


        if (Input.GetButton("Fire1"))
        {
            rotVel[0] += panSpeed * Input.GetAxis("Mouse X") * (Input.GetAxis("Horizontal") + 1) * 0.03f;
            rotVel[1] -= panSpeed * Input.GetAxis("Mouse Y") * (Input.GetAxis("Vertical") + 1) * 0.03f;
        }



        zoomVel = Mathf.Lerp(zoomVel, 0, Time.deltaTime * smoothTime);

        scale.x += zoomVel;
        scale.y += zoomVel;
        scale.z += zoomVel;

        rotVel[0] = Mathf.Clamp(rotVel[0], -180, 180);

        rotVel[0] = Mathf.Lerp(rotVel[0], 0, Time.deltaTime * smoothTime);

        rotVel[1] = Mathf.Clamp(rotVel[1], -180, 180);

        rotVel[1] = Mathf.Lerp(rotVel[1], 0, Time.deltaTime * smoothTime);

        rot.x += rotVel[1];
        rot.y += rotVel[0];
        //rot.z = 0;

        rot.x = ClampAngle(rot.x, -80f, 80f);

        scale.x = Mathf.Clamp(scale.x, 0.2f, 1f);
        scale.y = Mathf.Clamp(scale.y, 0.2f, 1f);
        scale.z = Mathf.Clamp(scale.z, 0.2f, 1f);

        transform.localScale = scale;
        transform.localEulerAngles = rot;

        DOF.focusDistance.value = scale.x * 3.4f - 0.4f;

        

    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180)
        {
            inverse = !inverse;
            tmin -= 180;
        }
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        if (max > 180)
        {
            inverse = !inverse;
            tmax -= 180;
        }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
    }
}
