using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLayoutBuilder : MonoBehaviour
{
    [SerializeField] private float secondsToUpdate;

    [Header("Rooms")]
    [SerializeField] private RoomLayout roomPrefab;
    [SerializeField] private RoomLayout endRoomPrefab;
    [SerializeField] private RoomLayout startRoomPrefab;

    [Header("Transitions")]
    [SerializeField] private TransitionLayout transitionPrefab;

    [Header("Settings")]
    [SerializeField] private float xOffset = 0;
    [SerializeField] private float yOffset = 0;
    [SerializeField] private float spaceBetweenRooms = 1.5f;

    [Header("Level Generator")]
    [SerializeField] private LevelGenerator levelGenerator;

    private void OnValidate()
    {
        if (levelGenerator == null)
            levelGenerator = GetComponent<LevelGenerator>();
    }

    private IEnumerator Start()
    {
        LevelTemplate levelTemplate = levelGenerator.GenerateNewLevel();
        BuildLevel(levelTemplate);

        if (secondsToUpdate > 0)
        {
            yield return new WaitForSecondsRealtime(secondsToUpdate);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
            
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void BuildLevel(LevelTemplate level)
    {
        if (level.startRoom != null)
            BuildStartRoom(level.startRoom);

        foreach (var roomPos in level.levelRoomsPositions)
        {
            RoomTemplate room = level.levelRooms[roomPos.x, roomPos.y];

            if (room == level.startRoom || room == level.endRoom)
                continue;

            BuildRoom(room);
        }

        if (level.endRoom != null)
            BuildEndRoom(level.endRoom);

        foreach (var transition in level.levelTransitions)
        {
            BuildTransition(transition);
        }
    }

    void SetRoomPosition(RoomLayout room, Vector2Int position)
    {
        room.transform.position = new Vector3(position.x + xOffset, position.y + yOffset, 0) * spaceBetweenRooms;
    }

    void BuildRoom(RoomTemplate room)
    {
        RoomLayout newRoom = Instantiate(roomPrefab);

        newRoom.bonusValue = room.bonusValue;
        newRoom.bonusType = room.bonusType;
        newRoom.enemyesOnRoom = room.enemyesOnRoom;
        newRoom.lockType = room.lockType;
        newRoom.havePortal = room.havePortal;

        SetRoomPosition(newRoom, room.position);
    }

    void BuildStartRoom(RoomTemplate startRoom)
    {
        RoomLayout newStartRoom = Instantiate(startRoomPrefab);
        newStartRoom.havePortal = startRoom.havePortal;
        SetRoomPosition(newStartRoom, startRoom.position);
    }

    void BuildEndRoom(RoomTemplate endRoom)
    {
        RoomLayout newEndRoom = Instantiate(endRoomPrefab);
        newEndRoom.havePortal = endRoom.havePortal;
        SetRoomPosition(newEndRoom, endRoom.position);
    }

    void BuildTransition(TransitionTemplate transitionTemplate)
    {
        TransitionLayout newTransition = Instantiate(transitionPrefab);

        if (transitionTemplate.rightOrUpRoom != null && transitionTemplate.leftOrDownRoom != null)
        {
            Vector3 rightOrUpRoomTransformPosition = new Vector3(transitionTemplate.rightOrUpRoom.position.x + xOffset, transitionTemplate.rightOrUpRoom.position.y + yOffset, 0) * spaceBetweenRooms;
            Vector3 leftOrDownRoomTransformPosition = new Vector3(transitionTemplate.leftOrDownRoom.position.x + xOffset, transitionTemplate.leftOrDownRoom.position.y + yOffset, 0) * spaceBetweenRooms;

            Vector3 newPosition = (rightOrUpRoomTransformPosition + leftOrDownRoomTransformPosition) / 2;

            newTransition.transform.position = newPosition;

            if (transitionTemplate.transitionType == TransitionType.Vertical)
                newTransition.transform.rotation = Quaternion.identity;
            else if (transitionTemplate.transitionType == TransitionType.Horizontal)
                newTransition.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            Destroy(newTransition);
            Debug.LogError("Incomplete transition!");
        }
    }
}
