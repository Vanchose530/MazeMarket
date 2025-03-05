using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplate
{
    public LevelTemplate level { get; private set; }
    public Vector2Int position;

    public RoomType roomType { get; private set; }

    public RoomLockType lockType;

    public bool havePortal;

    private TransitionTemplate _transitionUp;
    private TransitionTemplate _transitionRight;
    private TransitionTemplate _transitionDown;
    private TransitionTemplate _transitionLeft;

    public TransitionTemplate transitionUp { get { return _transitionUp; } set { _transitionUp = value; SetRoomType(); } }
    public TransitionTemplate transitionRight { get { return _transitionRight; } set { _transitionRight = value; SetRoomType(); } }
    public TransitionTemplate transitionDown { get { return _transitionDown; } set { _transitionDown = value; SetRoomType(); } }
    public TransitionTemplate transitionLeft { get { return _transitionLeft; } set { _transitionLeft = value; SetRoomType(); } }

    public RoomTemplate(LevelTemplate level)
    {
        this.level = level;
        canHaveBonus = true;
        maxTransitionsCount = 4;
        lockType = RoomLockType.None;
    }

    void SetRoomType()
    {
        switch (transitionsCount)
        {
            case 0:
                roomType = RoomType.None;
                break;
            case 1:
                roomType = RoomType.Deadlock;
                break;
            case 2:
                if ((transitionUp != null && transitionDown != null) || (transitionLeft != null && transitionRight != null))
                {
                    roomType = RoomType.I_room;
                }
                else
                {
                    roomType = RoomType.L_room;
                }
                break;
            case 3:
                roomType = RoomType.T_room;
                break;
            case 4:
                roomType = RoomType.X_room;
                break;
        }
    }

    public bool obligatory // обязательная комната - комната, через которую игрок не может пропустить, исследуя все тупики карты
    {
        get
        {
            switch (roomType)
            {
                case RoomType.None:
                    return false;
                case RoomType.L_room:
                    return dangerousRooms < 1;
                case RoomType.T_room:
                    return dangerousRooms < 2;
                case RoomType.X_room:
                    return dangerousRooms < 3;
                default: // deadlocks and I_rooms are always obligatory
                    return true;
            }
        }
    }

    private int dangerousRooms // комнаты, которые способны сделать данную комнату необязательной
    {
        get
        {
            int res = 0;

            Vector2Int[] adjacentСells = new Vector2Int[]
            {
                position + Vector2Int.up + Vector2Int.right,
                position + Vector2Int.right + Vector2Int.down,
                position + Vector2Int.down + Vector2Int.left,
                position + Vector2Int.left + Vector2Int.up
            };

            foreach (var cell in adjacentСells)
            {
                try
                {
                    bool condA = level.levelRooms[cell.x, cell.y].GetHaveTransitionWithRoom(level.levelRooms[cell.x, position.y]);
                    bool condB = level.levelRooms[cell.x, cell.y].GetHaveTransitionWithRoom(level.levelRooms[position.x, cell.y]);
                    bool condC = this.GetHaveTransitionWithRoom(level.levelRooms[cell.x, position.y]);
                    bool condD = this.GetHaveTransitionWithRoom(level.levelRooms[position.x, cell.y]);

                    if (condA && condB && condC && condD)
                        res++;
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }

            //switch (roomType)
            //{
            //    case RoomType.L_room:
            //        Debug.Log("L-Room dangerous rooms: " + res);
            //        break;
            //    case RoomType.T_room:
            //        Debug.Log("T-Room dangerous rooms: " + res);
            //        break;
            //    case RoomType.X_room:
            //        Debug.Log("X-Room dangerous rooms: " + res);
            //        break;
            //}

            return res;
        }
    }

    public int maxTransitionsCount = 4;

    public int neighboursCount
    {
        get
        {
            int res = 0;
            Vector2Int[] adjacentСells = new Vector2Int[]
            {
                position + Vector2Int.up,
                position + Vector2Int.right,
                position + Vector2Int.down,
                position + Vector2Int.left
            };
            foreach (var cell in adjacentСells)
            {
                bool validX = cell.x >= 0 && cell.x < level.maxX;
                bool validY = cell.y >= 0 && cell.y < level.maxY;

                if (validX && validY && level.levelRooms[cell.x, cell.y] != null)
                    res++;
            }
            return res;
        }
    }

    public int transitionsCount
    {
        get
        {
            int transitionsCount = 0;

            if (transitionUp != null)
                transitionsCount++;
            if (transitionRight != null)
                transitionsCount++;
            if (transitionLeft != null)
                transitionsCount++;
            if (transitionDown != null)
                transitionsCount++;

            return transitionsCount;
        }
    }

    public bool canSpawnAnotherNeighbour { get { return neighboursCount < maxTransitionsCount; } }
    public bool canMakeTransitions { get { return transitionsCount < maxTransitionsCount; } }

    private bool _canHaveBonus;
    public bool canHaveBonus
    {
        get { return _canHaveBonus; }
        set
        {
            if (!value)
                bonusValue = 0;
            _canHaveBonus = value;
        }
    }

    private float _bonusValue;
    public float bonusValue
    {
        get { return _bonusValue; }
        set
        {
            if (value <= 0)
            {
                bonusType = BonusType.None;
            }
            else if (value > 0 && bonusType == BonusType.None)
            {
                bonusType = BonusType.Chest;
            }
            _bonusValue = value;
        }
    }

    public BonusType bonusType = BonusType.None;

    public EnemyesOnRoom enemyesOnRoom = EnemyesOnRoom.None;

    public bool canHaveEnemyesOnRoom = true;

    public HashSet<Vector2Int> GetNeighboursPositions()
    {
        HashSet<Vector2Int> neighbours = new HashSet<Vector2Int>();

        Vector2Int[] adjacentСells = new Vector2Int[]
        {
            position + Vector2Int.up,
            position + Vector2Int.right,
            position + Vector2Int.down, 
            position + Vector2Int.left
        };

        foreach (var cell in adjacentСells)
        {
            bool validX = cell.x >= 0 && cell.x <= level.maxX;
            bool validY = cell.y >= 0 && cell.y <= level.maxY;

            if (validX && validY && level.levelRooms[cell.x, cell.y] != null)
                neighbours.Add(cell);
        }

        return neighbours;
    }

    public HashSet<Vector2Int> GetTransistedRoomsPositions()
    {
        HashSet<Vector2Int> transistedNeighbours = new HashSet<Vector2Int>();

        if (transitionUp?.rightOrUpRoom != null)
            transistedNeighbours.Add(transitionUp.rightOrUpRoom.position);
        if (transitionRight?.leftOrDownRoom != null)
            transistedNeighbours.Add(transitionRight.rightOrUpRoom.position);
        if (transitionDown?.leftOrDownRoom != null)
            transistedNeighbours.Add(transitionDown.leftOrDownRoom.position);
        if (transitionLeft?.leftOrDownRoom != null)
            transistedNeighbours.Add(transitionLeft.leftOrDownRoom.position);

        return transistedNeighbours;
    }

    public HashSet<Vector2Int> GetNearbyRoomsPositions()
    {
        HashSet<Vector2Int> nearbyRooms = new HashSet<Vector2Int>();

        Vector2Int[] adjacentСells = new Vector2Int[]
        {
            position + Vector2Int.up,
            position + Vector2Int.up + Vector2Int.right,
            position + Vector2Int.right,
            position + Vector2Int.right + Vector2Int.down,
            position + Vector2Int.down,
            position + Vector2Int.down + Vector2Int.left,
            position + Vector2Int.left,
            position + Vector2Int.left + Vector2Int.up
        };

        foreach (var cell in adjacentСells)
        {
            bool validX = cell.x >= 0 && cell.x <= level.maxX;
            bool validY = cell.y >= 0 && cell.y <= level.maxY;

            if (validX && validY && level.levelRooms[cell.x, cell.y] != null)
                nearbyRooms.Add(cell);
        }

        return nearbyRooms;
    }

    public bool GetHaveTransitionWithRoom(RoomTemplate room)
    {
        if (room == null)
            return false;

        return transitionUp?.rightOrUpRoom == room
            || transitionRight?.rightOrUpRoom == room
            || transitionDown?.leftOrDownRoom == room
            || transitionLeft?.leftOrDownRoom == room;
    }
}
