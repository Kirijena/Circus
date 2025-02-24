using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость перемещения
    public float smoothTime = 0.1f; // Время сглаживания
    private int diceRollResult; // Переменная для хранения числа, выпавшего на кубике
    private Animator animator; // Добавляем переменную для анимации

    private Vector3 targetPosition; // Целевая позиция для перемещения
    private Vector3 velocity = Vector3.zero; // Вектор скорости для SmoothDamp

    void Start()
    {
        // Загружаем число, которое было сохранено после броска кубика
        diceRollResult = PlayerPrefs.GetInt("DiceRollResult", 1); // Если нет сохранения, по умолчанию 1

        // Получаем компонент Animator
        animator = GetComponent<Animator>();
        targetPosition = transform.position; // Изначально персонаж стоит на месте
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // Получаем движение по осям
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * diceRollResult * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * diceRollResult * Time.deltaTime;

        // Обновляем целевую позицию
        targetPosition = transform.position + new Vector3(moveX, 0, moveZ);

        // Плавно перемещаем персонажа к целевой позиции
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Проверяем, достиг ли персонаж целевой позиции
        if (Vector3.Distance(transform.position, targetPosition) > 0.01f) // Установите порог, чтобы избежать дребезга
        {
            animator.SetBool("isMoving", true); // Включаем анимацию движения
            animator.SetBool("isStanding", false); // Отключаем анимацию стояния
        }
        else
        {
            animator.SetBool("isMoving", false); // Останавливаем анимацию движения
            animator.SetBool("isStanding", true); // Включаем анимацию стояния
        }
    }

    public void SetDiceRollResult(int rollResult)
    {
        // Сохраняем результат броска кубика
        diceRollResult = rollResult;
        PlayerPrefs.SetInt("DiceRollResult", diceRollResult);
    }
}