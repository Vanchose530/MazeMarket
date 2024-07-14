using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class TransitionTemplate
{
    public LevelTemplate level { get; private set; }

    private RoomTemplate _rightOrUpRoom;
    public RoomTemplate rightOrUpRoom
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

    private RoomTemplate _leftOrDownRoom;
    public RoomTemplate leftOrDownRoom
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

    public TransitionType transitionType;

    public TransitionTemplate(LevelTemplate level)
    {
        this.level = level;
    }
}
