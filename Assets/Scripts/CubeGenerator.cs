using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab of the cube to be instantiated
    public int numberOfCubes = 10; // Number of cubes to be generated
    public float size = 5f; // Size of the square area where cubes will be placed

    public bool execute = false;
    private void OnValidate()
    {
        if (execute)
        {
            GenerateCubes();
            execute = false;
        }
    }

    private void GenerateCubes()
    {
        // Calculate the gap between each cube based on the number of cubes
        float gap = size / (numberOfCubes - 1);

        // Calculate the maximum size of the cube to fit in the grid without overlapping
        float maxCubeSize = gap * 0.9f; // Use 0.9f to leave a small gap between cubes

        for (int i = 0; i < numberOfCubes; i++)
        {
            for (int j = 0; j < numberOfCubes; j++)
            {
                // Calculate the position of the cube in the grid
                float xPos = -size / 2 + i * gap;
                float zPos = -size / 2 + j * gap;

                // Calculate the position relative to the transform of the parent GameObject (CubeGenerator)
                Vector3 spawnPosition = transform.TransformPoint(new Vector3(xPos, 0f, zPos));

                // Instantiate the cube
                GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity, transform);

                // Set the size of the cube
                cube.transform.localScale = new Vector3(maxCubeSize, maxCubeSize, maxCubeSize);
            }
        }
    }
}
