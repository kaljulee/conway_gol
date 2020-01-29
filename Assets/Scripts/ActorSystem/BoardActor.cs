using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IsBoardActor
{

   T ExecuteBoardAction<T>(Action<T> action);

}
