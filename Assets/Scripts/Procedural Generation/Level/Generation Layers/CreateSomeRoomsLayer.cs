using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Create Some Rooms Layer", menuName = "Generation Layers/Create Some Rooms", order = 3)]
public class CreateSomeRoomsLayer : GenerationLayer
{
    [Header("Count")]
    public int count = 10;

    public override void Layer(LevelTemplate levelTemplate)
    {
        for (int i = 0; i < count; i++)
        {
            HashSet<Vector2Int> vacantPlaces = levelTemplate.GetVacantPlaces();

            if (vacantPlaces == null)
                Debug.Log("Vacant places void");

            // Debug.Log("For iteration: " +  i);
            // Debug.Log("Vacant places count: " + vacantPlaces.Count);

            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
            levelTemplate.CreateRoomAtPosition(position.x, position.y);
        }
        
    }
}
