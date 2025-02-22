using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class DiceRollScript : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 position;
    [SerializeField]
    private float maxRandForceVal, startRollingForce;
    float forceX, forceY, forceZ;
    public string diceFaceNum;
    public bool isLanded = false;
    public bool firstThrow = false;

    // Add a list of possible dice faces (for simplicity assuming a six-sided dice)
    private List<int> diceFaces = new List<int>{1, 2, 3, 4, 5, 6};

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        position = transform.position;
        transform.rotation = new Quaternion(Random.Range(0, 360),
           Random.Range(0, 360), Random.Range(0, 360), 0);
    }

    public void RollDice()
    {
        rigidbody.isKinematic = false;
        forceX = Random.Range(0, maxRandForceVal);
        forceY = Random.Range(0, maxRandForceVal);
        forceZ = Random.Range(0, maxRandForceVal);
        rigidbody.AddForce(Vector3.up * Random.Range(800, startRollingForce));
        rigidbody.AddTorque(forceX, forceY, forceZ);
    }

    public void ResetDice()
    {
        rigidbody.isKinematic = true;
        firstThrow = false;
        isLanded = false;
        transform.position = position;
    }

    private void Update()
    {
        if (rigidbody != null)
        {
            if (Input.GetMouseButton(0) && isLanded || Input.GetMouseButton(0) && !firstThrow)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                    {
                        if (!firstThrow)
                            firstThrow = true;

                        RollDice();  
                    }
                }
            }

            if (!isLanded && Mathf.Abs(rigidbody.velocity.magnitude) < 0.1f)
            {
                // Assuming dice has landed when velocity is nearly zero
                isLanded = true;
                DetermineFace();
            }
        }
    }

    private void DetermineFace()
    {
        // Here you can detect the top face of the dice based on its rotation.
        // For simplicity, this assumes a 6-sided die where we check which side is on top
        Vector3 upVector = transform.up;

        // You might need to adjust the threshold values depending on your dice setup.
        if (Vector3.Dot(upVector, Vector3.up) > 0.9f) // Top face is the one pointing upwards
        {
            diceFaceNum = "6"; // This is just an example, adjust according to your needs
        }
        else if (Vector3.Dot(upVector, Vector3.down) > 0.9f) // Bottom face
        {
            diceFaceNum = "1";
        }
        else if (Vector3.Dot(upVector, Vector3.right) > 0.9f) // Right face
        {
            diceFaceNum = "4";
        }
        else if (Vector3.Dot(upVector, Vector3.left) > 0.9f) // Left face
        {
            diceFaceNum = "3";
        }
        else if (Vector3.Dot(upVector, Vector3.forward) > 0.9f) // Forward face
        {
            diceFaceNum = "2";
        }
        else if (Vector3.Dot(upVector, Vector3.back) > 0.9f) // Back face
        {
            diceFaceNum = "5";
        }

        Debug.Log("Dice landed on face: " + diceFaceNum);
    }
}
