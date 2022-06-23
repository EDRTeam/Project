using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PositionController : MonoBehaviour {
    public Slider XPos;
    public Slider YPos;
    public Slider ZPos;

    public Vector3 Offset;
    //public Transform ControlledObject;
    public void UpdateObjectPosition(Transform ControlledObject)
    {
        Vector3 newPosition = new Vector3(XPos?XPos.value+Offset.x:0, YPos?YPos.value+ Offset.y : 0, ZPos?ZPos.value+ Offset.z : 0);
        ControlledObject.localPosition = newPosition;
    }
}
