// using UnityEngine;

// public class RGBEffect : MonoBehaviour
// {
//     public GameObject boxPrefab;
//     private GameObject[,] boxes = new GameObject[20, 20];
//     private float colorChangeSpeed = 1.0f;
//     private float time;

//     void Start()
//     {
//         // Initialize the grid of boxes
//         for (int x = 0; x < 20; x++)
//         {
//             for (int y = 0; y < 20; y++)
//             {
//                 boxes[x, y] = Instantiate(boxPrefab, new Vector3(x, y, 0), Quaternion.identity);
//             }
//         }
//     }

//     void Update()
//     {
//         time += Time.deltaTime * colorChangeSpeed;
//         for (int x = 0; x < 20; x++)
//         {
//             for (int y = 0; y < 20; y++)
//             {
//                 float r = Mathf.Sin(time + x * 0.1f);
//                 float g = Mathf.Sin(time + y * 0.1f);
//                 float b = Mathf.Sin(time + (x + y) * 0.1f);
//                 boxes[x, y].GetComponent<Renderer>().material.color = new Color(r, g, b);
//             }
//         }

//         // Change direction based on arrow key input
//         if (Input.GetKeyDown(KeyCode.UpArrow))
//         {
//             colorChangeSpeed = Mathf.Abs(colorChangeSpeed);
//         }
//         else if (Input.GetKeyDown(KeyCode.DownArrow))
//         {
//             colorChangeSpeed = -Mathf.Abs(colorChangeSpeed);
//         }
//         else if (Input.GetKeyDown(KeyCode.LeftArrow))
//         {
//             colorChangeSpeed = -Mathf.Abs(colorChangeSpeed);
//         }
//         else if (Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             colorChangeSpeed = Mathf.Abs(colorChangeSpeed);
//         }
//     }
// }




// using UnityEngine;

// public class RGBEffect : MonoBehaviour
// {
//     public GameObject boxPrefab;
//     private GameObject[,] boxes = new GameObject[20, 20];
//     private float time;

//     void Start()
//     {
//         // Initialize the grid of boxes
//         for (int x = 0; x < 20; x++)
//         {
//             for (int y = 0; y < 20; y++)
//             {
//                 boxes[x, y] = Instantiate(boxPrefab, new Vector3(x, y, 0), Quaternion.identity);
//             }
//         }
//     }

//     // void Update()
//     // {
//     //     // Check for arrow key input and update colors accordingly
//     //     if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
//     //         Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
//     //     {
//     //         time += Time.deltaTime;
//     //         for (int x = 0; x < 20; x++)
//     //         {
//     //             for (int y = 0; y < 20; y++)
//     //             {
//     //                 float r = Mathf.Sin(time + x * 0.1f);
//     //                 float g = Mathf.Sin(time + y * 0.1f);
//     //                 float b = Mathf.Sin(time + (x + y) * 0.1f);
//     //                 boxes[x, y].GetComponent<Renderer>().material.color = new Color(r, g, b);
//     //             }
//     //         }
//     //     }
//     // }


//     void Update()
//     {
//         // Check for arrow key input and update colors accordingly
//         if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
//             Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             time += Time.deltaTime;
//             Debug.Log("time: " + time);
//             float speedFactor = 1.0f; // Increase this value to make the colors shift faster
//             for (int x = 0; x < 20; x++)
//             {
//                 for (int y = 0; y < 20; y++)
//                 {
//                     float r = Mathf.Sin(speedFactor * (time * 8 + x * 0.1f));
//                     float g = Mathf.Sin(speedFactor * (time * 8 + y * 0.1f));
//                     float b = Mathf.Sin(speedFactor * (time * 8 + (x + y) * 0.1f));
//                     //log the value of r g b
//                     // Debug.Log("r: " + r + " g: " + g + " b: " + b);
//                     boxes[x, y].GetComponent<Renderer>().material.color = new Color(r, g, b);
//                 }
//             }
//         }
//     }
// }




// using UnityEngine;

// public class RGBEffect : MonoBehaviour
// {
//     public GameObject boxPrefab;
//     private GameObject[,] boxes = new GameObject[20, 20];
//     private float time;
//     private bool leftToRight = true;

//     void Start()
//     {
//         // Initialize the grid of boxes
//         for (int x = 0; x < 20; x++)
//         {
//             for (int y = 0; y < 20; y++)
//             {
//                 boxes[x, y] = Instantiate(boxPrefab, new Vector3(x, y, 0), Quaternion.identity);
//             }
//         }
//     }

//     void Update()
//     {
//         // Check for arrow key input and update direction
//         if (Input.GetKeyDown(KeyCode.LeftArrow))
//         {
//             leftToRight = true;
//         }
//         else if (Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             leftToRight = false;
//         }

//         // Update colors based on the direction
//         time += Time.deltaTime;
//         for (int x = 0; x < 20; x++)
//         {
//             for (int y = 0; y < 20; y++)
//             {
//                 float r, g, b;
//                 if (leftToRight)
//                 {
//                     r = Mathf.Sin(time + x * 0.1f);
//                     g = Mathf.Sin(time + y * 0.1f);
//                     b = Mathf.Sin(time + (x + y) * 0.1f);
//                 }
//                 else
//                 {
//                     r = Mathf.Sin(time + (19 - x) * 0.1f);
//                     g = Mathf.Sin(time + (19 - y) * 0.1f);
//                     b = Mathf.Sin(time + (38 - (x + y)) * 0.1f);
//                 }
//                 boxes[x, y].GetComponent<Renderer>().material.color = new Color(r, g, b);
//             }
//         }
//     }
// }




using UnityEngine;

public class RGBEffect : MonoBehaviour
{
    public GameObject boxPrefab;
    private GameObject[,] boxes = new GameObject[20, 20];
    private float time;
    private Vector2 direction = Vector2.right;

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
    }

    void Update()
    {
        // Check for arrow key input and update direction
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

        // Update colors based on the direction
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
                boxes[x, y].GetComponent<Renderer>().material.color = new Color(r, g, b);
            }
        }
    }
}