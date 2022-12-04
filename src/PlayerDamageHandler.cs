using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Holds the information for Player health;
/// </summary>
public class PlayerDamageHandler : MonoBehaviour
{
    public float health;
    [Range(1f, 270f)]
    public float maxHealth = 90f;

    void Start()
    {
        health = maxHealth;
    }

    /// <summary>
    /// Calculates the health of the Player after taking damage from an outside force.
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        
        if (health <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// Handles when the Player reaches 0 health and dies. The Player starts from the beginning of the maze.
    /// </summary>
    public void Death()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }

}