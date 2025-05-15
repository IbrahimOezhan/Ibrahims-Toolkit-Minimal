using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Client : MonoBehaviour
{
#if !UNITY_WEBGL

    private UdpClient udpClient;
    private IPEndPoint serverEndPoint;
    private Thread listenThread;

    public Action<string> OnMessageRecieved;

    public static Client Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;

            try
            {
                udpClient = new UdpClient(8888);
                serverEndPoint = new IPEndPoint(IPAddress.Any, 0);

                listenThread = new Thread(new ThreadStart(ListenForMessages));
                listenThread.IsBackground = true;
                listenThread.Start();
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
            if (listenThread != null) listenThread.Abort();
            if (udpClient != null) udpClient.Close();
        }
    }

    private void ListenForMessages()
    {
        while (true)
        {
            byte[] data = udpClient.Receive(ref serverEndPoint);
            string message = Encoding.UTF8.GetString(data);
            Debug.Log("Received message from server: " + message);
            OnMessageRecieved?.Invoke(message);
        }
    }

    public void SendResponse(string message)
    {
        UdpClient responseClient = new UdpClient();
        byte[] data = Encoding.UTF8.GetBytes(message);
        responseClient.Send(data, data.Length, new IPEndPoint(IPAddress.Parse("255.255.255.255"), 8889));
        responseClient.Close();
    }
#endif
}