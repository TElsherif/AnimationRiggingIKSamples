using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int rows = 5;
    public int columns = 5;
    public float spacing = 2.0f;

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 spawnPosition = new Vector3(col * spacing, 0.0f, row * spacing);
                Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}