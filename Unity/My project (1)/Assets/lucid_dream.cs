using UnityEngine;

public class SerialRGBEffect : MonoBehaviour
{
    public GameObject boxPrefab;
    private GameObject[,] boxes = new GameObject[20, 20];
    private float time;
    private Vector2 direction = Vector2.right;
    private string serialPortName = "COM3"; // Update this based on your setup
    private int baudRate = 9600;
    private System.IO.Ports.SerialPort serialPort;

    void Start()
    {
        // Initialize the grid of boxes
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                boxes[x, y] = Instantiate(boxPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }

        // Initialize the serial port
        try
        {
            serialPort = new System.IO.Ports.SerialPort(serialPortName, baudRate);
            serialPort.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    void Update()
    {
        // Check for arrow key input and update direction
        HandleInput();

        // Read serial data for new color
        string newColor = ReadSerialData();

        // Update colors based on the direction and serial input
        time += Time.deltaTime;
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                float r, g, b;
                if (direction == Vector2.right)
                {
                    r = Mathf.Sin(time + x * 0.1f);
                    g = Mathf.Sin(time + y * 0.1f);
                    b = Mathf.Sin(time + (x + y) * 0.1f);
                }
                else if (direction == Vector2.left)
                {
                    r = Mathf.Sin(time + (19 - x) * 0.1f);
                    g = Mathf.Sin(time + (19 - y) * 0.1f);
                    b = Mathf.Sin(time + (38 - (x + y)) * 0.1f);
                }
                else if (direction == Vector2.up)
                {
                    r = Mathf.Sin(time + y * 0.1f);
                    g = Mathf.Sin(time + x * 0.1f);
                    b = Mathf.Sin(time + (x + y) * 0.1f);
                }
                else if (direction == Vector2.down)
                {
                    r = Mathf.Sin(time + (19 - y) * 0.1f);
                    g = Mathf.Sin(time + (19 - x) * 0.1f);
                    b = Mathf.Sin(time + (38 - (x + y)) * 0.1f);
                }
                else
                {
                    // Handle diagonal directions
                    r = Mathf.Sin(time + (x * direction.x + y * direction.y) * 0.1f);
                    g = Mathf.Sin(time + (x * direction.y + y * direction.x) * 0.1f);
                    b = Mathf.Sin(time + ((x + y) * (direction.x + direction.y)) * 0.1f);
                }

                // Apply new color if valid
                if (!string.IsNullOrEmpty(newColor))
                {
                    Color parsedColor;
                    if (ColorUtility.TryParseHtmlString(newColor, out parsedColor))
                    {
                        r = parsedColor.r;
                        g = parsedColor.g;
                        b = parsedColor.b;
                    }
                }

                boxes[x, y].GetComponent<Renderer>().material.color = new Color(r, g, b);
            }
        }
    }

    void OnDestroy()
    {
        // Close the serial port when the game ends
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }

        // Handle diagonal directions
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            direction = new Vector2(1, 1).normalized;
        }
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            direction = new Vector2(-1, 1).normalized;
        }
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            direction = new Vector2(1, -1).normalized;
        }
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            direction = new Vector2(-1, -1).normalized;
        }
    }

    private string ReadSerialData()
    {
        if (serialPort != null && serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            try
            {
                string data = serialPort.ReadLine().Trim();
                if (IsValidHexColor(data))
                {
                    return data;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Failed to read from serial port: " + e.Message);
            }
        }
        return null;
    }

    private bool IsValidHexColor(string color)
    {
        return !string.IsNullOrEmpty(color) && color.Length == 7 && color.StartsWith("#") && int.TryParse(color.Substring(1), System.Globalization.NumberStyles.HexNumber, null, out _);
    }
}
