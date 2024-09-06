using System.Collections;
using UnityEngine;
using Vices.Scripts.Game;

public class DerogStartRage : MonoBehaviour
{
    [SerializeField] private DerogEnemy _derog;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        _derog.Context.IsStand = true;
    }
}
