using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : NetworkBehaviour
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
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxAngle;
    private float _motor;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = new Vector3(0, -0.153f, 0.422f);
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        _motor = Input.GetAxis("Vertical") * _force;

        if (_rb.velocity.magnitude > _maxSpeed)
        {
            _motor = 0;
        }

        _colliderBL.motorTorque = _motor;
        _colliderBR.motorTorque = _motor;

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

    }

    private void RotateWheel(WheelCollider collider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        transform.rotation = rotation;
        transform.position = position;
    }

    private void ResetZRotation()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        euler.z = 0f;
        transform.rotation = Quaternion.Euler(euler);

    }

}