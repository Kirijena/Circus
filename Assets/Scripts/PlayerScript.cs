using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    int characterIndex;
    public GameObject spawnPoint;

    private const string textFileName = "playerNames";
    private TurnBasedMovement turnBasedMovement;

    private List<GameObject> players = new List<GameObject>();

    private void Awake()
    {
        turnBasedMovement = FindObjectOfType<TurnBasedMovement>();
    }

    void Start()
    {
        int totalPlayers = 4; // 1 игрок + 3 NPC
        PlayerPrefs.SetInt("PlayerCount", totalPlayers - 1); // NPC = 3

        // Спавн главного игрока
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject mainCharacter = Instantiate(playerPrefabs[characterIndex], 
            spawnPoint.transform.position, Quaternion.identity);
        mainCharacter.GetComponent<NameScript>().SetPlayerName(
            PlayerPrefs.GetString("PlayerName"));
        
        players.Add(mainCharacter);

        string[] nameArray = ReadLinesFromFile(textFileName);

        // Создаём список доступных персонажей (без главного героя)
        List<GameObject> availablePrefabs = new List<GameObject>(playerPrefabs);
        availablePrefabs.RemoveAt(characterIndex); // Удаляем спрайт игрока из списка

        // Настройка сетки для NPC
        int rows = 2; 
        int cols = Mathf.CeilToInt((totalPlayers - 1) / (float)rows); 
        float spacing = 2f; // Расстояние между NPC

        Vector3 npcStartPosition = spawnPoint.transform.position + new Vector3(-spacing, 0, -spacing);

        for (int i = 0; i < totalPlayers - 1; i++)
        {
            int row = i / cols;
            int col = i % cols;
            Vector3 spawnPosition = npcStartPosition + 
                new Vector3(col * spacing, 0, row * spacing);

            // Выбираем случайного NPC и удаляем его из списка, чтобы не повторялся
            int randomIndex = Random.Range(0, availablePrefabs.Count);
            GameObject npcPrefab = availablePrefabs[randomIndex];
            availablePrefabs.RemoveAt(randomIndex);

            GameObject character = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            character.GetComponent<NameScript>().SetPlayerName(nameArray[Random.Range(0, nameArray.Length)]);
            
            players.Add(character);
        }
        
        turnBasedMovement.playerObjects = players.ToArray();
        turnBasedMovement.Initialize();
    }

    string[] ReadLinesFromFile(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);

        if (textAsset != null)
        {
            return textAsset.text.Split(new[] { '\r', '\n' }, 
                System.StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            Debug.LogError("File not found: " + fileName);
            return new string[0];
        }
    }
}
