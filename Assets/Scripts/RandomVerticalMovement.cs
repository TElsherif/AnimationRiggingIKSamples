using UnityEngine;
using System.Collections.Generic;

public class RandomVerticalMovement : MonoBehaviour
{
    public List<GameObject> objectsToMove; // List of GameObjects to move vertically

    [System.Serializable]
    public class MovementSettings
    {
        public float minHeight = 0f; // Minimum height of movement for this object
        public float maxHeight = 0.1f; // Maximum height of movement for this object
        public float speed = 2f; // Speed of vertical movement for this object

        [HideInInspector] public bool movingUp = true; // Flag to track the direction of movement for this object
        [HideInInspector] public float randomOffset; // Random offset to ensure unique movements for each object
    }

    public List<MovementSettings> movementSettingsList = new List<MovementSettings>();

    void Awake()
    {
        foreach (Transform child in transform)
        {
            objectsToMove.Add(child.gameObject);
        }
    }

    private void Start()
    {
        // Assign random offsets to ensure unique movements for each object
        for (int i = 0; i < objectsToMove.Count; i++)
        {
            MovementSettings settings = new MovementSettings
            {
                randomOffset = Random.Range(0f, 2f * Mathf.PI) // Random offset between 0 and 2 * PI
            };
            movementSettingsList.Add(settings);
        }
    }

    private void Update()
    {
        for (int i = 0; i < objectsToMove.Count; i++)
        {
            GameObject obj = objectsToMove[i];
            MovementSettings settings = movementSettingsList[i];

            // Get the current position of the object
            Vector3 position = obj.transform.position;

            // Calculate the new vertical position based on the current direction and random offset
            float newYPosition = position.y + Mathf.Sin(Time.time * settings.speed + settings.randomOffset) * Time.deltaTime;

            // Clamp the new position within the height range
            newYPosition = Mathf.Clamp(newYPosition, settings.minHeight, settings.maxHeight);

            // Update the object's position
            obj.transform.position = new Vector3(position.x, newYPosition, position.z);
        }
    }
}