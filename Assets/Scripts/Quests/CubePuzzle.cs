using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePuzzle : MonoBehaviour
{
    [SerializeField] private List<StoneCube> cubes;
    private List<Vector3> initialPositions;

    [SerializeField] private GameObject buttonOn;
    [SerializeField] private GameObject buttonOff;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform deadendReturnPointTransform;

    private void Start()
    {
        initialPositions = new List<Vector3>();
        foreach(StoneCube cube in cubes)
        {
            initialPositions.Add(cube.transform.position);
        }
    }

    private void OnTriggerEnter2D()
    {
        RefreshPositions();
        buttonOn.SetActive(true);
        buttonOff.SetActive(false);
    }
    private void OnTriggerExit2D()
    {
        buttonOn.SetActive(false);
        buttonOff.SetActive(true);
    }

    private void RefreshPositions()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            if (Vector3.Distance(initialPositions[i], playerTransform.position) < 0.1f)
            {
                // Вычисляем ближайшее свободное место вокруг игрока
                Vector3 closestFreePosition = CalculateClosestFreePosition(initialPositions[i], playerTransform.position);

                // Перемещаем игрока на ближайшее свободное место
                playerTransform.position = closestFreePosition;
            }

            cubes[i].transform.position = initialPositions[i];

            /*Пытаемся получить rigidbody куба, чтобы обнулить его velocity
            if (cubes[i].TryGetComponent(out Rigidbody2D rigidbody))
            {
                rigidbody.velocity = Vector2.zero;
            }
            Нет необходимости, т.к. rigidbody статичное!!
            */
        }
    }

    private Vector3 CalculateClosestFreePosition(Vector3 boxPosition, Vector3 playerPosition)
    {
        Vector3 newPlayerPosition = playerPosition;
        float offsetX = 0.15f;
        float offsetY = 0.2f;
        Vector3[] potentialPositions = new Vector3[]{new Vector3(boxPosition.x, boxPosition.y + offsetY, boxPosition.z),
                                                    new Vector3(boxPosition.x + offsetX, boxPosition.y, boxPosition.z),
                                                    new Vector3(boxPosition.x, boxPosition.y - offsetY, boxPosition.z),
                                                    new Vector3(boxPosition.x - offsetX, boxPosition.y, boxPosition.z)
                                                    };  //Векторный массив потециально свободных позиций
        bool foundPosition = false;

        //Ищем свободную позицию из 4-х потенциально возможных (сверху, снизу, слева и справа от места появления куба)
        for (int i = 0; i < potentialPositions.Length; i++)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(potentialPositions[i], 0.075f);

            if (colliders.Length == 0)
            {
                newPlayerPosition = potentialPositions[i]; // Позиция свободна
                foundPosition = true;
                break;
            }
        }
        
        if (!foundPosition) //если все четыре потенциальные позиции заняты, возвращаем игрока на особую позицию за пределами пазла
        {
            newPlayerPosition = deadendReturnPointTransform.position;
        }

        return newPlayerPosition;
    }


}
