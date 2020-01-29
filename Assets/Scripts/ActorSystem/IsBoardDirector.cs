using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IsBoardDirector
{
    Action IssueAddressBoardDirection(string actionType, float payload, Vector2 address);
    Action IssueDirectBoardDirection(string actionType, float payload, GameObject target);
}
