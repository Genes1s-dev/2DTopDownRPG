using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWithGrass : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private const string GROUND_SORTING_LAYER = "Ground";
    private const string TREES_SORTING_LAYER = "Buildings&Trees";

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D()
    {
        spriteRenderer.sortingLayerName = GROUND_SORTING_LAYER;
    }

    private void OnTriggerExit2D()
    {
        spriteRenderer.sortingLayerName = TREES_SORTING_LAYER;
    }
}
