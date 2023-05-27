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

        // Создаем префаб игрока на клиенте
        GameObject playerInstance = Instantiate(playerPrefab);

        // Получаем компонент NetworkTransform с префаба игрока
        playerNetworkTransform = playerInstance.GetComponent<NetworkTransform>();

        // Добавляем игрока для текущего соединения
        NetworkServer.AddPlayerForConnection(connectionToClient, playerInstance);

        // Устанавливаем префаб игрока в NetworkTransform объекта "Car"
        GameObject carObject = GameObject.Find("Car");
        NetworkTransform carNetworkTransform = carObject.GetComponent<NetworkTransform>();
        carNetworkTransform.target = playerInstance.transform;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer || playerNetworkTransform == null)
            return;

        // Получаем позицию и поворот игрока с помощью NetworkTransform
        Vector3 targetPosition = playerNetworkTransform.transform.position + offset;
        Quaternion targetRotation = playerNetworkTransform.transform.rotation;

        // Плавно перемещаем и поворачиваем камеру
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }
}