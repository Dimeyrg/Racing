using Mirror;
using UnityEngine;

public class NetworkCameraController : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -4f);
    [SerializeField] private float speed = 10f;

    private NetworkTransform playerNetworkTransform;

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        // ������� ������ ������ �� �������
        GameObject playerInstance = Instantiate(playerPrefab);

        // �������� ��������� NetworkTransform � ������� ������
        playerNetworkTransform = playerInstance.GetComponent<NetworkTransform>();

        // ��������� ������ ��� �������� ����������
        NetworkServer.AddPlayerForConnection(connectionToClient, playerInstance);

        // ������������� ������ ������ � NetworkTransform ������� "Car"
        GameObject carObject = GameObject.Find("Car");
        NetworkTransform carNetworkTransform = carObject.GetComponent<NetworkTransform>();
        carNetworkTransform.target = playerInstance.transform;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer || playerNetworkTransform == null)
            return;

        // �������� ������� � ������� ������ � ������� NetworkTransform
        Vector3 targetPosition = playerNetworkTransform.transform.position + offset;
        Quaternion targetRotation = playerNetworkTransform.transform.rotation;

        // ������ ���������� � ������������ ������
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }
}