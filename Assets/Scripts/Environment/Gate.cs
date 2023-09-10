using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private const string requiredItem = "Key";
    private bool keyFound;
    [SerializeField] private GameObject openGates;
    [SerializeField] private Transform exitTransform;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            /*var itemsList = Inventory.Instance.GetItemsList();
            for (int i = 0; i < itemsList.Count; i++)
            {
                if (itemsList[i].name == requiredItem)
                {
                    keyFound = true;
                    break;
                } 
            }*/

            if (keyFound)
            {
                spriteRenderer.enabled = false;
                openGates.SetActive(true);

                Player.Instance.GetComponent<Player>().enabled = false;  //


                StartCoroutine(ExitTransition());
                GameManager.Instance.WinSequence();
            } else {
                Debug.LogWarning("You do not have the key!!");
            }
        }
    }


    private IEnumerator ExitTransition()
    {
        Vector3 position = Player.Instance.transform.position;
        float duration = 1.0f;
        float timeElapsed = 0.0f;

        //сначала передвигаем призрака из его стартовой позиции в центр коробки
        while (timeElapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, this.exitTransform.position, timeElapsed / duration); //интерполируем из начальной точки призрака в центр коробки с вычисляемым процентажем
            newPosition.z = position.z;  //обнуляем координату z, т.к. она также меняется, а нам этого не нужно!
            Player.Instance.transform.position = newPosition;  //передвигаем призрака в новую точку
            timeElapsed += Time.deltaTime;  //передвижение призрака происходит пофреймово
            yield return null;
        }

        Player.Instance.GetComponent<Animator>().enabled = false;  //

    }
}
