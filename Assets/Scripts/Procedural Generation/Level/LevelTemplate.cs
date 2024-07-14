using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LevelTemplate
{
    public RoomTemplate[,] levelRooms;
    public List<Vector2Int> levelRoomsPositions { get; private set; }

    public List<TransitionTemplate> levelTransitions { get; private set; }

    public RoomTemplate startRoom;
    public RoomTemplate endRoom;

    public int maxX { get { return levelRooms.GetLength(0) - 1; } }
    public int maxY { get { return levelRooms.GetLength(1) - 1; } }

    public double bonusValueSum
    {
        get
        {
            double sum = 0;
            foreach (var roomPos in levelRoomsPositions)
            {
                sum += levelRooms[roomPos.x, roomPos.y].bonusValue;
            }
            return sum;
        }
    }

    public LevelTemplate()
    {
        levelRoomsPositions = new List<Vector2Int>();
        levelTransitions = new List<TransitionTemplate>();
    }

    public RoomTemplate CreateRoomAtPosition(int x, int y)
    {
        RoomTemplate newRoom = new RoomTemplate(this);
        Vector2Int roomPosition = new Vector2Int(x, y);

        levelRooms[x, y] = newRoom;
        levelRoomsPositions.Add(roomPosition);

        newRoom.position = roomPosition;

        return newRoom;
    }

    public Dictionary<Vector2Int, float> GetBonusRoomsPositions(bool getOnlyChests = true)
    {
        Dictionary<Vector2Int, float> bonusRoomsPositions = new Dictionary<Vector2Int, float>();

        foreach (var roomPos in levelRoomsPositions)
        {
            RoomTemplate room = levelRooms[roomPos.x, roomPos.y];

            if (room.bonusValue > 0)
            {
                if (getOnlyChests && room.bonusType != BonusType.Chest)
                    continue;

                bonusRoomsPositions.Add(new Vector2Int(roomPos.x, roomPos.y), room.bonusValue);
            }
        }

        return bonusRoomsPositions;
    }

    public HashSet<Vector2Int> GetVacantPlaces()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();

        for (int x = 0; x < levelRooms.GetLength(0); x++)
        {
            for (int y = 0; y < levelRooms.GetLength(1); y++)
            {
                RoomTemplate room = levelRooms[x, y];

                if (room == null)
                    continue;

                // Debug.Log("Room neighbours count: " + room.neighboursCount);

                if (!room.canSpawnAnotherNeighbour)
                    continue;

                if (x > 0 && levelRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && levelRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && levelRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && levelRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));

            }
        }

        return vacantPlaces;
    }

    public void MakeTransition(Vector2Int roomPositionA, Vector2Int roomPositionB)
    {
        // Debug.Log("Try to Make Transition in level");

        RoomTemplate roomA = levelRooms[roomPositionA.x, roomPositionA.y];
        RoomTemplate roomB = levelRooms[roomPositionB.x, roomPositionB.y];

        if (!roomA.canMakeTransitions || !roomB.canMakeTransitions)
        {
            // Debug.Log("Cant make transition between rooms because reached limit of the transitions count of one room");
            // Debug.Log("Room A max transitions: " + roomA.maxTransitionsCount + " Room B max transitions: " + roomB.maxTransitionsCount);
            return;
        }

        TransitionTemplate transition = new TransitionTemplate(this);
        
        bool exception = false;

        if (roomPositionA.x - roomPositionB.x == 1)
        {
            transition.transitionType = TransitionType.Horizontal;

            transition.rightOrUpRoom = levelRooms[roomPositionA.x, roomPositionA.y];
            transition.leftOrDownRoom = levelRooms[roomPositionB.x, roomPositionB.y];
        }
        else if (roomPositionA.x - roomPositionB.x == -1)
        {
            transition.transitionType = TransitionType.Horizontal;

            transition.rightOrUpRoom = levelRooms[roomPositionB.x, roomPositionB.y];
            transition.leftOrDownRoom = levelRooms[roomPositionA.x, roomPositionA.y];
        }
        else if (roomPositionA.y - roomPositionB.y == 1)
        {
            transition.transitionType = TransitionType.Vertical;

            transition.rightOrUpRoom = levelRooms[roomPositionA.x, roomPositionA.y];
            transition.leftOrDownRoom = levelRooms[roomPositionB.x, roomPositionB.y];
        }
        else if (roomPositionA.y - roomPositionB.y == -1)
        {
            transition.transitionType = TransitionType.Vertical;

            transition.rightOrUpRoom = levelRooms[roomPositionB.x, roomPositionB.y];
            transition.leftOrDownRoom = levelRooms[roomPositionA.x, roomPositionA.y];
        }
        else
        {
            // Debug.LogError("Trying to make transition with two rooms that are not neighbours");
            exception = true;
        }

        if (!exception)
        {
            levelTransitions.Add(transition);
            // Debug.Log("Make Transition in level");
        }
            
    }

    public void DestroyTransition(Vector2Int roomPositionA, Vector2Int roomPositionB)
    {
        RoomTemplate roomA = levelRooms[roomPositionA.x, roomPositionA.y];
        RoomTemplate roomB = levelRooms[roomPositionB.x, roomPositionB.y];

        TransitionTemplate transition = GetTransitionBetweenRooms(roomA, roomB);

        if (transition.transitionType == TransitionType.Vertical)
        {
            if (roomA.transitionUp == transition)
                roomA.transitionUp = null;
            else if (roomA.transitionDown == transition)
                roomA.transitionDown = null;

            if (roomB.transitionUp == transition)
                roomB.transitionUp = null;
            else if (roomB.transitionDown == transition)
                roomB.transitionDown = null;
        }
        else if (transition.transitionType == TransitionType.Horizontal)
        {
            if (roomA.transitionLeft == transition)
                roomA.transitionLeft = null;
            else if (roomA.transitionRight == transition)
                roomA.transitionRight = null;

            if (roomB.transitionLeft == transition)
                roomB.transitionLeft = null;
            else if (roomB.transitionRight == transition)
                roomB.transitionRight = null;
        }

        levelTransitions.Remove(transition);
    }

    public void DestroyTransition(RoomTemplate roomA, RoomTemplate roomB)
    {
        TransitionTemplate transition = GetTransitionBetweenRooms(roomA, roomB);

        if (transition.transitionType == TransitionType.Vertical)
        {
            if (roomA.transitionUp == transition)
                roomA.transitionUp = null;
            else if (roomA.transitionDown == transition)
                roomA.transitionDown = null;

            if (roomB.transitionUp == transition)
                roomB.transitionUp = null;
            else if (roomB.transitionDown == transition)
                roomB.transitionDown = null;
        }
        else if (transition.transitionType == TransitionType.Horizontal)
        {
            if (roomA.transitionLeft == transition)
                roomA.transitionLeft = null;
            else if (roomA.transitionRight == transition)
                roomA.transitionRight = null;

            if (roomB.transitionLeft == transition)
                roomB.transitionLeft = null;
            else if (roomB.transitionRight == transition)
                roomB.transitionRight = null;
        }

        levelTransitions.Remove(transition);
    }

    public TransitionTemplate GetTransitionBetweenRooms(RoomTemplate roomA, RoomTemplate roomB)
    {
        if (roomA.transitionUp?.rightOrUpRoom == roomB)
            return roomA.transitionUp;
        else if (roomA.transitionRight?.rightOrUpRoom == roomB)
            return roomA.transitionRight;
        else if (roomA.transitionDown?.leftOrDownRoom == roomB)
            return roomA.transitionDown;
        else if (roomA.transitionLeft?.leftOrDownRoom == roomB)
            return roomA.transitionLeft;
        else
            return null;
    }
}
