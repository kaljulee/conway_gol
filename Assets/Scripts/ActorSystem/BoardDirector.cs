using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISBoardDirector
{
    T IssueBoardDirection<T>(Action<T> action);
}
