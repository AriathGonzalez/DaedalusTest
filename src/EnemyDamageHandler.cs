using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the damage done when an Enemy collides with the Player.
/// </summary>
public class EnemyDamageHandler : MonoBehaviour
{
    public DamageHandler damageHandler;
    public PlayerDamageHandler playerDamageHandler;
    public float baseDamage = 1f;
    public float damageAmount;

    public void Start()
    {
        damageAmount = damageHandler.DamageAmount(baseDamage);
    }

    /// <summary>
    /// Handles when an Enemy collides with the Player.
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerDamageHandler.TakeDamage(damageAmount);
        }
    }

}