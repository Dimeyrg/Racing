using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField] private Transform _transformFL;
    [SerializeField] private Transform _transformFR;
    [SerializeField] private Transform _transformBL;
    [SerializeField] private Transform _transformBR;

    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderBL;
    [SerializeField] private WheelCollider _colliderBR;

    [SerializeField] private float _force;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _minAngle;

    [SerializeField] private TrailRenderer TrailrendererBL;
    [SerializeField] private TrailRenderer TrailrendererBR;

    //[SerializeField] private Transform trackedObjectBL;
    //[SerializeField] private Transform trackedObjectBR;

    private void FixedUpdate()
    {
        //if (trackedObjectBL != null && trackedObjectBR != null)
        //{
        //    TrailrendererBR.AddPosition(trackedObjectBR.position);
        //    TrailrendererBL.AddPosition(trackedObjectBL.position);
        //}

        _colliderBL.motorTorque = Input.GetAxis("Vertical") * _force;
        _colliderBR.motorTorque = Input.GetAxis("Vertical") * _force;

        if (Input.GetKey(KeyCode.Space))
        {
            _colliderBL.brakeTorque = 800f;
            _colliderBR.brakeTorque = 800f;
            _colliderFL.brakeTorque = 800f;
            _colliderFR.brakeTorque = 800f;
       
        }   
        else
        {
            _colliderFL.brakeTorque = 0f;
            _colliderFR.brakeTorque = 0f;
            _colliderBL.brakeTorque = 0f;
            _colliderBR.brakeTorque = 0f;
        }

        _colliderFL.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
        _colliderFR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");

        RotateWheel(_colliderFL, _transformFL);
        RotateWheel(_colliderFR, _transformFR);
        RotateWheel(_colliderBL, _transformBL);
        RotateWheel(_colliderBR, _transformBR);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _colliderBL.steerAngle = -_minAngle * Input.GetAxis("Horizontal");
            _colliderBR.steerAngle = -_minAngle * Input.GetAxis("Horizontal");
        }
        else
        {
            _colliderBL.steerAngle = 0f;
            _colliderBR.steerAngle = 0f;
        }

    }

    private void RotateWheel(WheelCollider collider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        transform.rotation = rotation;
        transform.position = position;
    }

}   