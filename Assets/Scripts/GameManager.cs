using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private Player player;
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        } else {
            Instance  = this;
        }
    }

    private void Start()
    {
        player = Player.Instance;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void WinSequence()
    {
        Debug.LogWarning("You won!!");
    }

    private void AnimatePlayerToExit()
    {

    }
}
