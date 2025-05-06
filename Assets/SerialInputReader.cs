using System;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class SerialInputReader : MonoBehaviour
{
    public static SerialInputReader Instance;

    private SerialPort serialPort;
    private Thread serialThread;
    private bool isRunning = false;

    private ConcurrentQueue<string> serialQueue = new ConcurrentQueue<string>();

    public PlayerData p1Data;
    public PlayerData p2Data;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        serialPort = new SerialPort("COM7", 9600);
        serialPort.ReadTimeout = 50;
        serialPort.DtrEnable = true;
        serialPort.RtsEnable = true;

        try
        {
            serialPort.Open();
            isRunning = true;

            serialThread = new Thread(ReadSerialLoop);
            serialThread.Start();
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to open serial port: " + ex.Message);
        }
    }

    private void ReadSerialLoop()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string line = serialPort.ReadLine();
                serialQueue.Enqueue(line);
            }
            catch (TimeoutException)
            {
                // Ignore timeouts
            }
            catch (Exception ex)
            {
                Debug.LogError("Serial thread error: " + ex.Message);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        while (serialQueue.TryDequeue(out string line))
        {
            Debug.Log("Received: " + line);
            ParseData(line);
        }
    }

    void ParseData(string data)
    {
        // Same as before
        string[] players = data.Split(';');
        foreach (string player in players)
        {
            string[] parts = player.Split(':');
            if (parts.Length != 2) continue;

            string playerId = parts[0];
            int[] values = Array.ConvertAll(parts[1].Split(','), int.Parse);

            if (playerId == "P1")
                p1Data = new PlayerData(values);
            else if (playerId == "P2")
                p2Data = new PlayerData(values);
        }
    }

    private void OnApplicationQuit()
    {
        isRunning = false;
        serialThread?.Join();

        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
