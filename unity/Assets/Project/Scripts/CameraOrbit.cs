using UnityEngine;
using UnityEngine.EventSystems;

namespace DRL
{
    /// <summary>
    /// Class responsible for controlling the camera position and orientation when the user
    /// drags the mouse over the screen or scrolls. Requirement for this script to work
    /// properly is that the camera needs to be parented under the <see cref="_cameraHolderTransform"/>.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraOrbit : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] private Transform _cameraHolderTransform = null;

        [Header("Scroll Options: ")]
        [SerializeField] private float _scrollSpeed = 50f;
        [SerializeField] private float _scrollLerpFactor = 0.9f;
        [SerializeField] private float _minimumDistanceFromOrigin = 2f;

        [Header("Rotation Options: ")]
        [SerializeField] private float _rotationMultiplier = 0.25f;

        private Vector2 _dragStartMousePosition = Vector2.zero;
        private Quaternion _dragStartHolderRotation = Quaternion.identity;
        private bool _wasMouseOverUI = false;

        private void Awake()
        {
            // Initialize camera looking at the origin.
            transform.LookAt(Vector3.zero);
        }

        private void Update()
        {
            // When user scrolls, move camera towards or from the origin.
            float scrollAmount = Input.mouseScrollDelta.y * _scrollSpeed * Time.deltaTime;
            if (Mathf.Abs(scrollAmount) > 0f)
            {
                MoveCameraOnScroll(scrollAmount);
            }

            // When the users clicks on the screen, remember that position as the start point.
            if (Input.GetMouseButtonDown(0))
            {
                CacheDragStartData();
            }

            // While the user is dragging the mouse on the screen, rotate the camera.
            bool isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
            if (Input.GetMouseButton(0) && !isMouseOverUI)
            {
                if (_wasMouseOverUI)
                {
                    // When user drags over UI, set this as the new drag start position to 
                    // prevent sudden changes.
                    CacheDragStartData();
                }

                Vector2 mouseScreenPosition = Input.mousePosition;
                Vector2 mouseDragDelta = (mouseScreenPosition - _dragStartMousePosition) * _rotationMultiplier;
                RotateHolderOnDrag(mouseDragDelta.x, -mouseDragDelta.y);
            }

            _wasMouseOverUI = isMouseOverUI;
        }

        private void CacheDragStartData()
        {
            _dragStartMousePosition = Input.mousePosition;
            _dragStartHolderRotation = _cameraHolderTransform.rotation;
        }

        private void MoveCameraOnScroll(float scrollAmount)
        {
            // Making use of the fact that the origin is at Vector3.zero;
            Vector3 originToCameraDirection = transform.position.normalized;

            Vector3 desiredPosition = transform.position - originToCameraDirection * scrollAmount;
            if (desiredPosition.magnitude < _minimumDistanceFromOrigin)
            {
                desiredPosition = originToCameraDirection * _minimumDistanceFromOrigin;
            }

            transform.position = Vector3.Lerp(transform.position, desiredPosition, _scrollLerpFactor);
        }

        private void RotateHolderOnDrag(float horizontalDelta, float verticalDelta)
        {
            Quaternion rotation = _dragStartHolderRotation;
            rotation = Quaternion.AngleAxis(verticalDelta * Mathf.Rad2Deg, rotation * Vector3.right) * rotation;
            rotation = Quaternion.AngleAxis(horizontalDelta * Mathf.Rad2Deg, Vector3.up) * rotation;
            _cameraHolderTransform.rotation = rotation;
        }
    }
}