using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Server : MonoBehaviour
{
#if !UNITY_WEBGL

    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;
    private Thread receiveThread;

    public Action<string> OnMessageRecieved;

    public static Server Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;

            try
            {
                udpClient = new UdpClient();
                udpClient.EnableBroadcast = true;
                remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, 8888);

                receiveThread = new Thread(new ThreadStart(ReceiveResponses));
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch
            {

            }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            receiveThread.Abort();
            udpClient.Close();
        }
    }

    public new void BroadcastMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, remoteEndPoint);
    }

    private void ReceiveResponses()
    {
        try
        {
            UdpClient responseClient = new UdpClient(8889);
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                byte[] data = responseClient.Receive(ref clientEndPoint);
                string response = Encoding.UTF8.GetString(data);
                Debug.Log("Received response from client: " + response);
                OnMessageRecieved?.Invoke(response);
            }
        }
        catch
        {

        }
    }

#endif
}