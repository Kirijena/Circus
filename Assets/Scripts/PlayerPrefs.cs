using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private int diceRollResult; // Переменная для хранения числа, выпавшего на кубике

    void Start()
    {
        // Загружаем число, которое было сохранено после броска кубика
        diceRollResult = PlayerPrefs.GetInt("DiceRollResult", 1); // Если нет сохранения, по умолчанию 1
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * diceRollResult * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * diceRollResult * Time.deltaTime;

        transform.Translate(moveX, 0, moveZ);
    }

    public void SetDiceRollResult(int rollResult)
    {
        // Сохраняем результат броска кубика
        diceRollResult = rollResult;
        PlayerPrefs.SetInt("DiceRollResult", diceRollResult);
    }
}
