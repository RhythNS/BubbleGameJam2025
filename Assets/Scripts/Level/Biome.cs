using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Biome")]
public class Biome : ScriptableObject
{
    [SerializeField]
    private List<Level> levels;
    public List<Level> Levels => levels;
}
