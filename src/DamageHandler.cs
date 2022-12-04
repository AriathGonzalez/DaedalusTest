using UnityEditor;
using UnityEngine;

/// <summary>
/// Class that stores the difficulty multiplier and calculates the damage amount.
/// </summary>
public class DamageHandler : MonoBehaviour
{
    [Range(1f, 3f)]
    public float difficulty = 1f;
    public float difficultyMultiplier;

    public void Start()
    {
        difficultyMultiplier = difficulty;
    }

    /// <summary>
    /// Calcualtes the damage amount given the base damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <returns></returns>
    public float DamageAmount(float baseDamage)
    {
        return difficultyMultiplier * baseDamage;
    }
}