using System;
using UnityEngine;

namespace Asteroids.Modules.Gameplay
{
    public class World : MonoBehaviour
    {
        private static World _instance;
    
        public Camera gameCamera;

        private static Vector2 _gameWorldMinimum, _gameWorldMaximum;

        private static float Width => _gameWorldMaximum.x - _gameWorldMinimum.x;
        private static float Height => _gameWorldMaximum.y - _gameWorldMinimum.y;
        
        void Start()
        {
            if (_instance != null)
                Destroy(this); //More than one world can't exist at the same time.
        
            if (gameCamera == null)
                throw new Exception("World Camera is null. Please add the reference to world script!");
            
            _instance = this;

            _gameWorldMinimum = gameCamera.ScreenToWorldPoint(Vector2.zero);
            _gameWorldMaximum = gameCamera.ScreenToWorldPoint(gameCamera.pixelRect.size);
            
            Debug.Log(PlaceInPlayArea(_gameWorldMinimum - Vector2.one));
            Debug.Log(PlaceInPlayArea(_gameWorldMaximum));
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

