using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Modules.Gameplay
{
    public class World : MonoBehaviour
    {
        private static World _instance;
        
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

            _instance = this;

            _gameWorldMinimum = gameCamera.ScreenToWorldPoint(Vector2.zero);
            _gameWorldMaximum = gameCamera.ScreenToWorldPoint(gameCamera.pixelRect.size);
        }


        public static bool IsInPlayArea(Vector2 point)
        {
            if (_instance == null)
                throw new Exception("There is no world in the game scene!");

            return point.x >= _gameWorldMinimum.x && point.x <= _gameWorldMaximum.x &&
                   point.y >= _gameWorldMinimum.y && point.y <= _gameWorldMaximum.y;
        }

        public static bool IsVisibleInPlayArea(Vector2 point, Vector2 size)
        {
            return IsInPlayArea(point + size / 2f) || IsInPlayArea(point - size / 2f) ||
                   IsInPlayArea(point + new Vector2(size.x, -size.y) / 2f) ||
                   IsInPlayArea((point + new Vector2(-size.x, size.y) / 2f));
        }
        
        public static bool IsFullyInPlayArea(Vector2 point, Vector2 size)
        {
            return IsInPlayArea(point + size / 2f) && IsInPlayArea(point - size / 2f);
        }
      
        public static Vector2 LoopInPlayArea(Vector2 point)
        {
            if (IsInPlayArea(point)) return point;

            var shifted = point - _gameWorldMinimum;

            return new Vector2((shifted.x % Width + Width) % Width, (shifted.y % Height + Height) % Height) + _gameWorldMinimum;
        }

        public static Vector2 GetRandomVectorInPlayArea()
        {
            const float accuracy = 0.8f; //makes it impossible to get a point on the edge
            
            return new Vector2(Random.Range(_gameWorldMinimum.x, _gameWorldMaximum.x),
                Random.Range(_gameWorldMinimum.y, _gameWorldMaximum.y))*accuracy;
        }

        public static void CreateSpaceObject(SpaceObject definition)
        {
            
        }
    }
}

