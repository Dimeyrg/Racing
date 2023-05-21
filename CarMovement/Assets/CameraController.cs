using Mirror;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    private NetworkTransform _carTransform;
    private Vector3 _offset = new Vector3(0f, 2f, -4f);
    private float _speed = 10f;

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        // Находим объект с помощью кода
        GameObject carObject = GameObject.Find("Car");
        _carTransform = carObject.GetComponent<NetworkTransform>();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer || _carTransform == null)
            return;

        var targetPosition = _carTransform.transform.TransformPoint(_offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);

        var direction = _carTransform.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _speed * Time.deltaTime);
    }
}