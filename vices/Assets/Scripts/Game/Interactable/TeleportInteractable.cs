using System.Collections;
using UnityEngine;
using Vices.Scripts.Core;
using Vices.Scripts.Game.UI;

namespace Vices.Scripts.Game
{
    public class TeleportInteractable : Interactable
    {
        [SerializeField] private Transform _teleportPosition;
        private Transform _player;

        protected override void OnEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _player = other.transform;
        }

        private IEnumerator TeleportDelay(CharacterController characterController)
        {
            characterController.enabled = false;

            characterController.transform.position = _teleportPosition.position;

            yield return new WaitForSeconds(0.65f);

            characterController.enabled = true;

            BlackScreen.Screen.Hide();
        }

        protected override void OnInteract()
        {
            BlackScreen.Screen.Show(() => StartCoroutine(TeleportDelay(_player.GetComponent<CharacterController>())));
        }
    }
}
