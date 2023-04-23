using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "LevelsDatabase", menuName = "Data/Levels Database")]
public class LevelsDatabase : ScriptableObject
{
    [SerializeField] private DataLevel[] _levels;

    private Dictionary<LevelType, DataLevel> _levelsCached = new Dictionary<LevelType, DataLevel>();

    private void Init()
    {
        _levelsCached.Clear();

        foreach (var level in _levels)
        {
            _levelsCached.Add(level.Type, level);
        }
    }

    public DataLevel GetInfo(LevelType type)
    {
        if (_levelsCached.Count == 0)
            Init();

        if (_levelsCached.TryGetValue(type, out DataLevel level))
        {
            return level;
        }

        return null;
    }
}
