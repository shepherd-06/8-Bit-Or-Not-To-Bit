using System;
using System.Collections;
using System.Collections.Generic;

using System.IO.Ports;


using UnityEngine;

public class GradientGrid : MonoBehaviour
{
    private bool isActive = false;

    private float timer = 0f;
    public GameObject boxPrefab; // Prefab for the grid cells
    public int gridSize = 20; // Number of rows and columns
    public float cellSize = .1f; // Size of each cell
    // public float colorChangeInterval = 2f; // Interval for color changes
    public float colorChangeInterval = 1.5f; // Interval for color changes
    public float revertToWhiteInterval = 5f; // Interval to revert cells to white
    public TextAsset colorFile; // Text file containing color codes

    private GameObject[,] grid;
    private List<Color> colors = new List<Color>();
    private int colorIndex = 0;
    private float lastColorChangeTime;
    private float lastNewColorTime;

    public string portName = "COM3"; // Change to your Arduino's COM port
    public int baudRate = 9600; // Baud rate must match Arduino

    private SerialPort serialPort;



    void Start()
    {
        // Load colors from file
        // LoadColors();

        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();

        // Initialize the grid
        grid = new GameObject[gridSize, gridSize];
        float yOffset = 1.5f; // Adjust this value to move the grid up
        float xOffset = -0.5f; // Adjust this value to move the grid up


        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // Adjust position using xOffset and yOffset
                GameObject cell = Instantiate(
                    boxPrefab,
                    new Vector3(x * cellSize + xOffset, y * cellSize + yOffset, 0),
                    Quaternion.identity
                );
                cell.GetComponent<Renderer>().material.color = Color.white;
                grid[x, y] = cell;
            }
        }


        lastColorChangeTime = Time.time;
        lastNewColorTime = Time.time;
    }





    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        float currentTime = Time.time;


        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string line = serialPort.ReadLine();
                if (line.StartsWith("#"))
                {
                    if (ColorUtility.TryParseHtmlString(line, out Color color))
                    {
                        colors.Add(color);
                        //log
                        Debug.Log("color: " + color);
                        //array size
                        Debug.Log("array size: " + colors.Count);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error reading from serial port: {e.Message}");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isActive)
        {
            isActive = true;
            GenerateSpiral();
        }

        // Automatically update the spiral every 3 seconds
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= colorChangeInterval)
            {
                GenerateSpiral();
                timer = 0f; // Reset the timer
            }
        }


    }




    void GenerateSpiralNew()
    {
        if (colors.Count < 3)
        {
            Debug.LogError("At least three colors are required.");
            return;
        }

        // Define the colors for the gradient
        Color startColor = colors[0]; // Center color
        Color midColor = colors[1];   // Midway color
        Color endColor = colors[2];   // Edge color

        // Center of the grid
        int centerX = gridSize / 2;
        int centerY = gridSize / 2;

        // Spiral logic
        int x = centerX, y = centerY;
        int dx = 0, dy = -1;

        int totalCells = gridSize * gridSize;

        for (int step = 0; step < totalCells; step++)
        {
            // Check if the current position is within bounds
            if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            {
                // Calculate the distance from the center
                float maxDistance = Mathf.Sqrt(Mathf.Pow(centerX, 2) + Mathf.Pow(centerY, 2));
                float currentDistance = Mathf.Sqrt(Mathf.Pow(x - centerX, 2) + Mathf.Pow(y - centerY, 2));

                // Interpolate the color based on the distance
                float t = currentDistance / maxDistance;
                Color interpolatedColor;
                if (t <= 0.5f)
                {
                    interpolatedColor = Color.Lerp(startColor, midColor, t * 2); // From center to middle
                }
                else
                {
                    interpolatedColor = Color.Lerp(midColor, endColor, (t - 0.5f) * 2); // From middle to edge
                }

                grid[x, y].GetComponent<Renderer>().material.color = interpolatedColor;
            }

            // Spiral movement logic
            if (x == y || (x > 0 && x == 1 - y) || (x < 0 && x == -y))
            {
                int temp = dx;
                dx = -dy;
                dy = temp;
            }

            x += dx;
            y += dy;
        }

        // Reshuffle colors for the next gradient
        ShuffleColorsNew();
    }

    void ShuffleColorsNew()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, colors.Count);
            Color temp = colors[i];
            colors[i] = colors[randomIndex];
            colors[randomIndex] = temp;
        }

        Debug.Log("Colors reshuffled!...................");
    }


















    void GenerateSpiral()
    {
        // Get the first three colors for the gradient
        // if (colors.Count < 3) return;

        // Color startColor = colors[0];
        // Color midColor = colors[1];
        // Color endColor = colors[2];

        // // Create the spiral
        // int left = 0, right = gridSize - 1;
        // int top = 0, bottom = gridSize - 1;

        // float totalSteps = gridSize * gridSize;
        // float colorStep = 1f / totalSteps;
        // float t = 0f; // Interpolation factor

        // while (left <= right && top <= bottom)
        // {
        //     // Top row
        //     for (int i = left; i <= right; i++)
        //     {
        //         grid[top, i].GetComponent<Renderer>().material.color = Color.Lerp(startColor, midColor, t);
        //         t += colorStep;
        //     }
        //     top++;

        //     // Right column
        //     for (int i = top; i <= bottom; i++)
        //     {
        //         grid[i, right].GetComponent<Renderer>().material.color = Color.Lerp(midColor, endColor, t);
        //         t += colorStep;
        //     }
        //     right--;

        //     // Bottom row
        //     for (int i = right; i >= left; i--)
        //     {
        //         grid[bottom, i].GetComponent<Renderer>().material.color = Color.Lerp(endColor, startColor, t);
        //         t += colorStep;
        //     }
        //     bottom--;

        //     // Left column
        //     for (int i = bottom; i >= top; i--)
        //     {
        //         grid[i, left].GetComponent<Renderer>().material.color = Color.Lerp(startColor, midColor, t);
        //         t += colorStep;
        //     }
        //     left++;
        // }

        // // Reshuffle colors
        // ShuffleColors();


        Color startColor = colors[0];
        Color middleColor = colors[1];
        Color endColor = colors[2];

        // Apply the gradient spiral
        ApplyGradientSpiral(startColor, middleColor, endColor);

        // Reshuffle the color array
        ShuffleColors();
    }


    void ApplyGradientSpiral(Color startColor, Color middleColor, Color endColor)
    {
        int top = 0, bottom = gridSize - 1;
        int left = 0, right = gridSize - 1;
        float colorStep = 1f / (gridSize * gridSize); // Gradient step size
        float t = 0f; // Interpolation factor

        while (top <= bottom && left <= right)
        {
            // Top row
            for (int i = left; i <= right; i++)
            {
                grid[top, i].GetComponent<Renderer>().material.color = GetGradientColor(startColor, middleColor, endColor, t);
                t += colorStep;
            }
            top++;

            // Right column
            for (int i = top; i <= bottom; i++)
            {
                grid[i, right].GetComponent<Renderer>().material.color = GetGradientColor(startColor, middleColor, endColor, t);
                t += colorStep;
            }
            right--;

            // Bottom row
            if (top <= bottom)
            {
                for (int i = right; i >= left; i--)
                {
                    grid[bottom, i].GetComponent<Renderer>().material.color = GetGradientColor(startColor, middleColor, endColor, t);
                    t += colorStep;
                }
                bottom--;
            }

            // Left column
            if (left <= right)
            {
                for (int i = bottom; i >= top; i--)
                {
                    grid[i, left].GetComponent<Renderer>().material.color = GetGradientColor(startColor, middleColor, endColor, t);
                    t += colorStep;
                }
                left++;
            }
        }
    }

    Color GetGradientColor(Color start, Color middle, Color end, float t)
    {
        if (t < 0.5f)
        {
            // Interpolate between start and middle
            return Color.Lerp(start, middle, t * 2f);
        }
        else
        {
            // Interpolate between middle and end
            return Color.Lerp(middle, end, (t - 0.5f) * 2f);
        }
    }

    void ShuffleColors()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, colors.Count);
            Color temp = colors[i];
            colors[i] = colors[randomIndex];
            colors[randomIndex] = temp;
        }

        Debug.Log("Colors reshuffled!..........");
    }

    void LoadColors()
    {
        //color file name is arduino_Data.txt
        // TextAsset colorFile = Resources.Load<TextAsset>("arduino_data.txt");

        string colorFile = "Assets/arduino_data.txt";


        //log the color file
        Debug.Log("color file: " + colorFile);


        if (colorFile == null)
        {
            Debug.LogError("Color file not provided!");
            return;
        }

        // string[] lines = colorFile.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        string[] lines = System.IO.File.ReadAllLines(colorFile);


        foreach (string line in lines)
        {
            if (line.StartsWith("#"))
            {
                if (ColorUtility.TryParseHtmlString(line, out Color color))
                {
                    colors.Add(color);
                }
            }
        }

        if (colors.Count == 0)
        {
            Debug.LogError("No valid color codes found in the file.");
        }
    }
}


