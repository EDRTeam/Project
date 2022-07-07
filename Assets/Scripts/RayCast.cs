using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : SceneSingleton<RayCast>
{
    public RaycastHit CastRay(Camera camera)
    {
        //将射线长度限制在摄像机内
        Vector3 screenFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.farClipPlane);
        Vector3 screenNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane);

        Vector3 far = camera.ScreenToWorldPoint(screenFar);
        Vector3 near = camera.ScreenToWorldPoint(screenNear);

        RaycastHit hit;
        Physics.Raycast(near, far - near, out hit);
        return hit;
    }
}
