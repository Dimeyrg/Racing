using Mirror;
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
    private float _moveX;
    private float _moveZ;
    private Rigidbody _rb;

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0f, 2f, -6f);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = new Vector3(0, -0.153f, 0.422f);
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        _moveX = Input.GetAxis("Vertical") * _force;


        if (_rb.velocity.magnitude > _maxSpeed)
        {
            _moveX = 0;
        }

        _colliderBL.motorTorque = _moveX;
        _colliderBR.motorTorque = _moveX;

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

        _moveZ = _maxAngle * Input.GetAxis("Horizontal");
        _colliderFL.steerAngle = _moveZ;
        _colliderFR.steerAngle = _moveZ;

        RotateWheel(_colliderFL, _transformFL);
        RotateWheel(_colliderFR, _transformFR);
        RotateWheel(_colliderBL, _transformBL);
        RotateWheel(_colliderBR, _transformBR);

        //transform.Rotate(0, _moveX, 0f);
        //transform.Translate(0, 0f, _moveZ);
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