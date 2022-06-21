using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPlanerTracker : MonoBehaviour
{

    public string PlaneName="Cutter";

    private Transform _TSCuttingPlanner;

    public bool Invert;

    private Material _MT;
    // Start is called before the first frame update
    void Start()
    {

        _TSCuttingPlanner = GameObject.Find(PlaneName).transform;


        _MT = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Invert)
        { _MT.SetVector("_PlaneNormal", _TSCuttingPlanner.up); }
        else {

            _MT.SetVector("_PlaneNormal", _TSCuttingPlanner.up*-1);

        }
       

        _MT.SetVector("_PlanePosition", _TSCuttingPlanner.position);

    }
}
