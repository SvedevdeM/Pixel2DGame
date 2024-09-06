using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ATTENTION  ŒŒŒ–””””“»»»Õ€€€€ IN NEED TO REWRITE
/// </summary>
public class HidingInBush : MonoBehaviour
{
    public float dragSpeed = 2f;
    private Transform _bushTransform;
    private PlayerMovement _movement;
    private MovingSide OutHideSIde;
    private CharacterController _characterController;
    private Transform _;
    private Transform _1;
    public Vector2 BushOffset = new Vector2(1.1f, 1.1f);

    public bool IsInHide { get; private set; } = false;

    public bool isInsideBush { get; private set; } = false;
    private Vector3 initialPosition;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _characterController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.CompareTag("Bush") && !IsInHide)
        {
            initialPosition = collision.transform.position;
            StartCoroutine(DragIntoBush());
        }
       // _characterController.center.
    }

    private void OnTriggerStay(Collider other)
    {
        if (isInsideBush)
        if((_movement._movement.x != 0 ) || (_movement._movement.y != 0))
        {
            Vector2 facingDirection = Vector2.zero;
            if (_movement._movement.x == 1)
                facingDirection = Vector2.right;
            else if(_movement._movement.y == 1) 
                facingDirection = Vector2.up;
            else if( _movement._movement.x == -1)
                facingDirection = Vector2.left;
            else if((_movement._movement.y == -1))
                facingDirection = Vector2.down;
            StartCoroutine(DragOutOfBush(facingDirection));
        }
    }


    /*private void MovePlayer()
    {
        transform.position = Vector3.MoveTowards();
    }*/

    private IEnumerator WaitASec()
    {
        yield return null;
    }

    private IEnumerator DragIntoBush()
    {
        GetComponent<PlayerMovement>().enabled = false;
        IsInHide = true;
        Vector3 targetPosition = initialPosition;

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.002f);
            yield return null;
        }
        isInsideBush = true;
        GetComponent<PlayerMovement>().enabled = true;
        
    }

    private IEnumerator DragOutOfBush(Vector2 facingDirection)
    {
        GetComponent<PlayerMovement>().enabled = false;
        Vector3 targetPosition = new Vector3(initialPosition.x + (facingDirection.x * 1.1f), initialPosition.y + (facingDirection.y * 1.1f), 0);

        //Vector3 facingDirection = (targetPosition - startPosition).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, 1f, LayerMask.GetMask("Wall"));

        if (hit.collider == null)
        {
            while (transform.position!=targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.002f);
                yield return null;
            }
            isInsideBush = false;
            IsInHide = false;
            GetComponent<PlayerMovement>().enabled = true;
        }
        else
        {
            Debug.Log("Wall detected. Cannot exit the bush.");
        }
        
    }
}

