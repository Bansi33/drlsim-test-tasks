using UnityEngine;
using UnityEngine.EventSystems;

namespace DRL
{
    /// <summary>
    /// Class responsible for controlling the camera position and orientation when the user
    /// drags the mouse over the screen or scrolls the mouse wheel. Requirement for this script to work
    /// properly is that the camera needs to be parented under the <see cref="_cameraHolderTransform"/>.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraOrbit : MonoBehaviour
    {
        [Header("References: ")]
        [Tooltip("Reference to the transform of the object holding the camera. This is a " +
            "requirement for this script to work properly.")]
        [SerializeField] private Transform _cameraHolderTransform = null;

        [Header("Scroll Options: ")]
        [SerializeField] private float _scrollSpeed = 50f;
        [SerializeField] private float _scrollLerpFactor = 0.9f;
        [SerializeField] private float _minimumDistanceFromOrigin = 2f;

        [Header("Rotation Options: ")]
        [SerializeField] private float _rotationMultiplier = 0.25f;

        /// <summary>
        /// Mouse position at the start of the drag event.
        /// </summary>
        private Vector2 _dragStartMousePosition = Vector2.zero;

        /// <summary>
        /// Rotation of the <see cref="_cameraHolderTransform"/> at the start of the drag event. 
        /// </summary>
        private Quaternion _dragStartHolderRotation = Quaternion.identity;

        /// <summary>
        /// Helper variable indicating if the mouse was over UI on the last frame. Used to
        /// reset the drag event when the mouse goes over UI.
        /// </summary>
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

        /// <summary>
        /// Function remembers the current conditions (mouse position and camera holder's rotation) 
        /// at the beginning of the drag event. These conditions are required for correctly calculating 
        /// camera's rotation and position during the drag event.
        /// </summary>
        private void CacheDragStartData()
        {
            _dragStartMousePosition = Input.mousePosition;
            _dragStartHolderRotation = _cameraHolderTransform.rotation;
        }

        /// <summary>
        /// Function moves the camera's transform towards/from origin based on the mouse scroll.
        /// </summary>
        /// <param name="scrollAmount">Scroll amount representing the camera movement amount.</param>
        private void MoveCameraOnScroll(float scrollAmount)
        {
            // Making use of the fact that the origin is at Vector3.zero;
            Vector3 originToCameraDirection = transform.position.normalized;

            // Clamping camera position if it gets too close to the origin.
            Vector3 desiredPosition = transform.position - originToCameraDirection * scrollAmount;
            if (desiredPosition.magnitude < _minimumDistanceFromOrigin)
            {
                desiredPosition = originToCameraDirection * _minimumDistanceFromOrigin;
            }

            // Moving camera based on the scroll amount, with additional smoothing.
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _scrollLerpFactor);
        }

        /// <summary>
        /// Function rotates the <see cref="_cameraHolderTransform"/> based on the mouse drag amount. 
        /// The final rotation is calculated by rotating the <see cref="_dragStartHolderRotation"/> for
        /// rotation calculated from the total horizontal and vertical offset from the start position.
        /// </summary>
        /// <param name="horizontalDelta">Total horizontal mouse delta from the drag start.</param>
        /// <param name="verticalDelta">Total vertical mouse delta from the drag start.</param>
        private void RotateHolderOnDrag(float horizontalDelta, float verticalDelta)
        {
            Quaternion rotation = _dragStartHolderRotation;
            rotation = Quaternion.AngleAxis(verticalDelta * Mathf.Rad2Deg, rotation * Vector3.right) * rotation;
            rotation = Quaternion.AngleAxis(horizontalDelta * Mathf.Rad2Deg, Vector3.up) * rotation;
            _cameraHolderTransform.rotation = rotation;
        }
    }
}