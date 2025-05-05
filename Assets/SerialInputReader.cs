using System;
using System.IO.Ports;
using UnityEngine;

public class SerialInputReader : MonoBehaviour
{
    public static SerialInputReader Instance;

    private SerialPort serialPort = new SerialPort("COM7", 9600);

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
        string[] ports = SerialPort.GetPortNames();
        Debug.Log("Available COM ports:");
        foreach (string port in ports)
        {
            Debug.Log(port);
        }
        
        serialPort.Open();
        serialPort.ReadTimeout = 1;
        serialPort.DtrEnable = true;
        serialPort.RtsEnable = true;
    }

    private void FixedUpdate()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            Debug.Log("Serialport exists and is Open");
            try
            {
                // Attempt to read the line
                string inputLine = serialPort.ReadLine();

                // If ReadLine succeeds, log and parse
                Debug.Log("Received: " + inputLine);
                ParseData(inputLine);
            }
            catch (TimeoutException)
            {
                // A timeout occurred. Log it so you know.
                // This is expected if data isn't arriving FROM UNITY'S PERSPECTIVE
                // (e.g., wrong port, port conflict, or temporary connection issue)
                // You don't necessarily need to stop anything, just be aware.
                Debug.LogWarning("Serial Read Timeout - No full line received within 5 seconds.");
            }
            catch (Exception ex)
            {
                // Catch other potential errors (e.g., IOExceptions if port disconnects)
                Debug.LogError("Error reading from serial port: " + ex.Message);
                // Consider potentially closing/reopening the port here if errors persist
            }
        }
        else
        {
            if (serialPort != null) {
                Debug.LogWarning("Serial port is not open.");
            } else {
                Debug.LogWarning("Serial port object is null.");
            }
            // Maybe add a delay or attempt to reopen if needed
        }
    }

    void ParseData(string data)
    {
        // "P1:100,200,150,300;P2:90,180,160,250"
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
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
