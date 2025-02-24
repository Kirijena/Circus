using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnBasedMovement : MonoBehaviour
{
    public GameObject[] playerObjects; // Players
    public GameObject[] platforms; // Platforms (Platform1 to Platform20)
    private int currentPlayerIndex = 0; // To keep track of the current player
    private int[] playerPositions; // Player's current position on the platform grid
    private int totalPlayers = 4; // 1 player + 3 NPCs
    public bool gameOver = false; // Flag to check if the game is over
    public TMP_Text winText; // Text to display winner
    private int winnerIndex = -1; // Index of the winner

    // Reference to the dice roll script
    DiceRollScript diceRollScript;
    TMP_Text rolledNumberText;

    // Smoothing parameters
    public float smoothTime = 0.6f; // Увеличено время сглаживания для уменьшения скорости
    private Vector3[] velocities; // Array to hold velocities for each player
    private bool isMoving = false; // Flag to check if the player is currently moving

    void Awake()
    {
        diceRollScript = FindObjectOfType<DiceRollScript>();
    }

    public void Initialize()
    {
        playerPositions = new int[totalPlayers];
        velocities = new Vector3[totalPlayers]; // Initialize velocities array
        for (int i = 0; i < totalPlayers; i++)
        {
            playerPositions[i] = 0; // Start all players at position 0 (Platform1)
            velocities[i] = Vector3.zero; // Initialize velocity for each player
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
        if (gameOver) return; // Stop the game if someone has won

        if (diceRollScript != null && diceRollScript.isLanded && !isMoving)
        {
            int rolledNumber = int.Parse(diceRollScript.diceFaceNum);
            StartCoroutine(MovePlayer(rolledNumber));
        }

        // Smoothly move players to their target positions
        for (int i = 0; i < totalPlayers; i++)
        {
            if (playerPositions[i] < platforms.Length)
            {
                GameObject player = playerObjects[i];
                Vector3 targetPosition = platforms[playerPositions[i]].transform.position;
                targetPosition.y = player.transform.position.y; // Keep the y position the same
                player.transform.position = Vector3.SmoothDamp(player.transform.position, targetPosition, ref velocities[i], smoothTime);
            }
        }
    }

    IEnumerator MovePlayer(int rolledNumber)
    {
        isMoving = true; // Set the moving flag

        // Get the current player to move
        GameObject currentPlayer = playerObjects[currentPlayerIndex];

        // Find the current position of the player on the platform grid
        int currentPosition = playerPositions[currentPlayerIndex];

        // Calculate the new position
        int newPosition = currentPosition + rolledNumber;

        // Ensure the new position is within bounds (not greater than 19, as we have 20 platforms)
        newPosition = Mathf.Clamp(newPosition, 0, 19);

        // Move through each platform from the current position to the new position
        for (int i = currentPosition; i <= newPosition; i++)
        {
            playerPositions[currentPlayerIndex] = i; // Update the player's position
            Vector3 targetPosition = platforms[i].transform.position;
            targetPosition.y = currentPlayer.transform.position.y; // Keep the y position the same

            // Move to the target position
            while (Vector3.Distance(currentPlayer.transform.position, targetPosition) > 0.1f)
            {
                currentPlayer.transform.position = Vector3.SmoothDamp(currentPlayer.transform.position, targetPosition, ref velocities[currentPlayerIndex], smoothTime);
                yield return null; // Wait for the next frame
            }
        }

        // Check if the player reached the last platform (Platform 20)
        if (newPosition ==  19)
        {
            gameOver = true; // End the game
            winnerIndex = currentPlayerIndex; // Set the winner
            DisplayWinner(); // Display winner message
        }

        // Move to the next player's turn
        currentPlayerIndex = (currentPlayerIndex + 1) % totalPlayers;

        // Reset the dice for the next roll
        diceRollScript.ResetDice();
        isMoving = false; // Reset the moving flag
    }

    void DisplayWinner()
    {
        // Display winner message
        winText.text = playerObjects[winnerIndex].name + " Wins!";
    }
}