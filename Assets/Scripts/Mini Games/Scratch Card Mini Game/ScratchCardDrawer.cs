using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace __App.Scripts.Utilities.Scratch_Card
{
    public class ScratchCardDrawer : MonoBehaviour, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("References")]
        [SerializeField] private RectTransform _bounds;
        [SerializeField] private RawImage _outputImage;
        
        [Header("Drawing Settings")]
        [SerializeField] private int textureSize = 256;
        [SerializeField] private int drawRadius = 4;
        [SerializeField] private int interpolation = 3;

        [Header("Callbacks")] 
        public UnityEvent<ScratchCardDrawer> CardScratched;
        
        private Vector2 lastMousePosition;

        private bool _isBlocked;

        private Texture2D Texture2D;
        private int _overallPixels => textureSize * textureSize;
        private int _current;

        private void Start()
        {
            ResetCard();
        }

        public void ResetCard()
        {
            // Create Texture
            Texture2D = new Texture2D(textureSize, textureSize);
            
            ClearTexture();

            // Reset blocked flag
            _isBlocked = false;
        }

        private void ClearTexture()
        {
            for (int x = 0; x < textureSize; x++)
                for (int y = 0; y < textureSize; y++)
                    Texture2D.SetPixel(x, y, Color.clear);
            
            Texture2D.Apply();
            
            _outputImage.texture = Texture2D;
            _current = 0;
        }

        public void Block(bool value)
        {
            _isBlocked = value;
        }
        
        private Vector2 MousePosition
        {
            get
            {
                var xPst = (Input.mousePosition.x - Rect.xMin) / Rect.width;
                var yPst = (Input.mousePosition.y - Rect.yMin) / Rect.height;

                return new Vector2(xPst, yPst);
            }
        }

        private Rect Rect
        {
            get
            {
                var rect = new Rect()
                {
                    x = _bounds.position.x + _bounds.rect.xMin,
                    y = _bounds.position.y + _bounds.rect.yMin,
                    width = _bounds.rect.xMax - _bounds.rect.xMin,
                    height = _bounds.rect.yMax - _bounds.rect.yMin
                };

                return rect;
            }
        }

        public float GetFillPercentage => (float)_current / (float)_overallPixels;

        private void DrawInPosition(Vector2 position)
        {
            // Interpolate over pixels in radius of position
            for (int x = -drawRadius; x < drawRadius; x++)
            {
                for (int y = -drawRadius; y < drawRadius; y++)
                {
                    // Restrict from drawing over radius
                    if (Mathf.Sqrt(x * x + y * y) <= drawRadius)
                    {
                        // Clamp coordinates in texture
                        var rx = (int)(Mathf.Clamp(position.x * textureSize + x, 0, textureSize));
                        var ry = (int)(Mathf.Clamp(position.y * textureSize + y, 0, textureSize));

                        // Set pixels
                        if (Texture2D.GetPixel(rx, ry).Equals(Color.clear)) _current++;
                        Texture2D.SetPixel(rx, ry, Color.white);
                    }
                }
            }

            Texture2D.Apply();
        }

        #region Pointer Events

        private bool _isOver = false;
        
        #if UNITY_EDITOR
                private bool _isPressed => Input.GetMouseButton(0);
        #else 
                private bool _isPressed => Input.touchCount > 0 && Input.GetTouch(0).phase is TouchPhase.Began or TouchPhase.Moved;
        #endif

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_isBlocked) return;
            
            // Adjust mouse cache position, if it`s not being setted before
            if (lastMousePosition == default) lastMousePosition = MousePosition;
            
            // Check for mouse over, and pressed
            if (_isOver && _isPressed)
            {
                // Interpolate mouse position between old and new mouse positions
                for (int i = 0; i <= interpolation; i++)
                {
                    var lerp = Vector2.Lerp(lastMousePosition, MousePosition, i / (float)interpolation);
                    DrawInPosition(lerp);
                }
            }
            
            // Invoke callbacks
            CardScratched?.Invoke(this);
            
            // Replace mouse cache position with current
            lastMousePosition = MousePosition;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isOver == false)
                _isOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isOver == true)
                _isOver = false;
        }

        #endregion
    }
}