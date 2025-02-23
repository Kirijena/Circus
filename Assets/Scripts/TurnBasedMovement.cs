using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class TurnBasedMovement : MonoBehaviour
{
    public GameObject[] playerObjects; // Players
    public GameObject[] platforms; // Platforms (Platform1 to Platform20)
    private int currentPlayerIndex = 0; // To keep track of the current player
    private int[] playerPositions; // Player's current position on the platform grid
    private int totalPlayers = 4; // 1 player + 3 NPCs

    // Reference to the dice roll script
    DiceRollScript diceRollScript;
    TMP_Text rolledNumberText;

    void Awake()
    {
        diceRollScript = FindObjectOfType<DiceRollScript>();
    }

    public void Initialize()
    {
        playerPositions = new int[totalPlayers];
        for (int i = 0; i < totalPlayers; i++)
        {
            playerPositions[i] = 0; // Start all players at position 0 (Platform1)
        }
    }

    void Start()
    {
        platforms = new GameObject[20]; // Assume these are assigned in the inspector or set up in code.
        // Initialize platforms array with references to Platform1 through Platform20
        for (int i = 0; i < 20; i++)
        {
            platforms[i] = GameObject.Find("Platform" + (i + 1));
        }
    }

    void Update()
    {
        if (diceRollScript != null && diceRollScript.isLanded)
        {
            int rolledNumber = int.Parse(diceRollScript.diceFaceNum);
            
            MovePlayer(rolledNumber);
        }
    }

    void MovePlayer(int rolledNumber)
    {
        // Get the current player to move
        GameObject currentPlayer = playerObjects[currentPlayerIndex];

        // Find the current position of the player on the platform grid
        int currentPosition = playerPositions[currentPlayerIndex];

        // Calculate the new position
        int newPosition = currentPosition + rolledNumber;

        // Ensure the new position is within bounds (not greater than 19, as we have 20 platforms)
        newPosition = Mathf.Clamp(newPosition, 0, 19);

        // Move the player to the new position
        float y = currentPlayer.transform.position.y;
        currentPlayer.transform.position = platforms[newPosition].transform.position;
        
        currentPlayer.transform.position = new Vector3(currentPlayer.transform.position.x, y, currentPlayer.transform.position.z);

        // Update the player's position in the array
        playerPositions[currentPlayerIndex] = newPosition;

        // Move to the next player's turn
        currentPlayerIndex = (currentPlayerIndex + 1) % totalPlayers;

        // Reset the dice for the next roll
        diceRollScript.ResetDice();
    }
}
