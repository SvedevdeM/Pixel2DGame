using UnityEngine;
using Vices.Scripts.Core;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 3f;
    [SerializeField] private Vector2 offset;
    [SerializeField] private bool FollowPlayer = true;
    [SerializeField] private bool FollowXAxis = false;
    [SerializeField] private bool FollowYAxis = false;
    [SerializeField] private bool stayStill = false;
    [SerializeField] private LayerMask barrierLayer;
    
    private PlayerMovement _movement;
    private Transform _ptransform;
    private GameObject ObjectToFollow;
    private Vector2 _newoffset;

    private void Start()
    {
        ObjectToFollow = FindObjectOfType<CameraTargetFollow>().gameObject;
        _ptransform = ObjectToFollow.GetComponent<Transform>();
        _movement = ObjectToFollow.GetComponentInObject<PlayerMovement>();
        transform.position = new Vector3(_ptransform.position.x, _ptransform.position.y, transform.position.z);
    }

    private void Update()
    {
        if (FollowPlayer)
            FollowPlayerMode();
        else if(FollowXAxis)
            FollowXAxisMode();
        else if(FollowYAxis)
            FollowYAxisMode();

        MoveCamera(_newoffset);
    }

    private void FollowPlayerMode()
    {
        if (_movement._movement.x != 0)
        {
            _newoffset.x = offset.x * _movement._movement.x;
        }
        else if (_movement._movement.y != 0)
        {
            _newoffset.y = offset.y * _movement._movement.y;
        }
    }

    private void MoveCamera(Vector2 newoffset)
    {
        Vector3 ptransform = new Vector3(_ptransform.position.x + newoffset.x, _ptransform.position.y + newoffset.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, ptransform, smoothSpeed * Time.deltaTime);
    }

    private void FollowXAxisMode()
    {

    }
    private void FollowYAxisMode()
    {

    }
}
