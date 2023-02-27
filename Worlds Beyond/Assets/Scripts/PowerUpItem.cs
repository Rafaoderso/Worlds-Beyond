using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    public float powerUpDuration = 10f; // Dauer der Stärkung
    public float powerUpMultiplier = 2f; // Faktor, um den der Schaden des Spielers erhöht wird

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            Weapon weapon = other.gameObject.GetComponent<Weapon>();
            if (weapon != null)
            {
                StartCoroutine(PowerUp(weapon));
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator PowerUp(Weapon weapon)
    {
        weapon.damageMultiplier *= powerUpMultiplier; // Erhöht den Schaden des Spielers
        yield return new WaitForSeconds(powerUpDuration);
        weapon.damageMultiplier /= powerUpMultiplier; // Reduziert den Schaden des Spielers auf den normalen Wert
    }
}
