using System.Collections.Generic;
using UnityEngine;

public class DefenderWall : MonoBehaviour
{
    private List<GameObject> _defenders = new List<GameObject>();

    public void SetDefender(GameObject defender)
    { 
        _defenders.Add(defender);
    }
    public GameObject GetDefender()
    {
        if (_defenders.Count != 0)
        { 
            var defender = _defenders[0];
            _defenders.Remove(defender);
            return defender;
        }
        else
        {
            DefenderManager.Instance.RemoveWall(this);
            return null;
        }
    }
    public GameObject GetDefender(int id) => _defenders[id];
    public int GetDefendersCount() => _defenders.Count;
}
