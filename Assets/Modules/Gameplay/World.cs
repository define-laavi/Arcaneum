using System;
using UnityEngine;

namespace Asteroids.Modules.Gameplay
{
    public class World : MonoBehaviour
    {
        private static World _instance;

        public GameObject loopCameraPrefab;
        
        public Camera gameCamera;

        private static Vector2 _gameWorldMinimum, _gameWorldMaximum;

        public static float Width => _gameWorldMaximum.x - _gameWorldMinimum.x;
        public static float Height => _gameWorldMaximum.y - _gameWorldMinimum.y;
        
        void Start()
        {
            if (_instance != null)
                Destroy(this); //More than one world can't exist at the same time.
        
            if (gameCamera == null)
                throw new Exception("World Camera is null. Please add the reference to the world script!");

            if (loopCameraPrefab == null)
                throw new Exception("Loop Camera Prefab is null. Please add the reference to the world script!");
            
            _instance = this;

            _gameWorldMinimum = gameCamera.ScreenToWorldPoint(Vector2.zero);
            _gameWorldMaximum = gameCamera.ScreenToWorldPoint(gameCamera.pixelRect.size);
            
            CreateLoopCameras();
        }

        void CreateLoopCameras()
        {
            const float overlayPercentage = 0.05f;
            const float oneMinusOverlayPercentage = 1 - overlayPercentage;

            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if(i == j && i == 0) continue;

                    //Place the camera just right outside of the main camera so that the viewports are touching.
                    var position = new Vector3(i * Width / 2f * (1 + overlayPercentage),
                        j * Height / 2f * (1 + overlayPercentage), -10); 

                    //Orthographic size is normalized with viewports width so we have to handle the height ourselves.
                    var orthographicSize = gameCamera.orthographicSize * (j != 0 ? overlayPercentage : 1f); 

                    var viewport = new Rect(
                        i < 0 ? oneMinusOverlayPercentage : 0,
                        j < 0 ? oneMinusOverlayPercentage : 0, 
                        i == 0 ? 1 : overlayPercentage,
                        j == 0 ? 1 : overlayPercentage);
                    
                    var loopCam = GameObject.Instantiate(loopCameraPrefab, gameCamera.transform);
                    loopCam.transform.position = position;
                    loopCam.GetComponent<Camera>().rect = viewport;
                    loopCam.GetComponent<Camera>().orthographicSize = orthographicSize;
                }
            }
        }


        public static bool IsInPlayArea(Vector2 point)
        {
            if (_instance == null)
                throw new Exception("There is no world in the game scene!");

            return point.x >= _gameWorldMinimum.x && point.x <= _gameWorldMaximum.x &&
                   point.y >= _gameWorldMinimum.y && point.y <= _gameWorldMaximum.y;
        }

        public static Vector2 PlaceInPlayArea(Vector2 point)
        {
            if (IsInPlayArea(point)) return point;

            var shifted = point - _gameWorldMinimum;

            return new Vector2((shifted.x % Width + Width) % Width, (shifted.y % Height + Height) % Height) + _gameWorldMinimum;
        }
    }
}

