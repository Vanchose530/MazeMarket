using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionLayout : MonoBehaviour
{
    private RoomLayout _rightOrUpRoom;
    public RoomLayout rightOrUpRoom
    {
        get { return _rightOrUpRoom; }
        set
        {
            switch (transitionType)
            {
                case TransitionType.Horizontal:
                    value.transitionLeft = this;
                    break;
                case TransitionType.Vertical:
                    value.transitionDown = this;
                    break;
            }
            _rightOrUpRoom = value;
        }
    }

    private RoomLayout _leftOrDownRoom;
    public RoomLayout leftOrDownRoom
    {
        get { return _leftOrDownRoom; }
        set
        {
            switch (transitionType)
            {
                case TransitionType.Horizontal:
                    value.transitionRight = this;
                    break;
                case TransitionType.Vertical:
                    value.transitionUp = this;
                    break;
            }
            _leftOrDownRoom = value;
        }
    }

    private TransitionType _transitionType;
    public TransitionType transitionType
    {
        get { return _transitionType; }
        set
        {
            switch (value)
            {
                case TransitionType.Vertical:
                    transform.rotation = Quaternion.identity;
                    break;
                case TransitionType.Horizontal:
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }
            _transitionType = value;
        }
    }
    public void SetPositionBetweenRooms()
    {
        if (rightOrUpRoom != null && leftOrDownRoom != null)
        {
            Vector3 newPosition = (rightOrUpRoom.transform.position + leftOrDownRoom.transform.position) / 2;
            transform.position = newPosition;
        }
        else
        {
            Debug.LogError("Incomplete transition!");
        }
    }

    public void DestroyThisTransition()
    {
        if (transitionType == TransitionType.Horizontal)
        {
            rightOrUpRoom.transitionLeft = null;
            leftOrDownRoom.transitionRight = null;
        }
        else if (transitionType == TransitionType.Vertical)
        {
            rightOrUpRoom.transitionDown = null;
            leftOrDownRoom.transitionUp = null;
        }
        Destroy(this.gameObject);
    }
}
