using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IsBoardDirector
{
    Action IssueAddressBoardDirection(int actionType, float payload, Vector2 address);
    Action IssueDirectBoardDirection(int actionType, float payload, GameObject target);
}
