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

        public static Vector2 LoopInPlayArea(Vector2 point, Vector2 size)
        {
            var shifted = point - (_gameWorldMinimum - size / 2f);

            var sizeWidth = Width + size.x;
            var sizeHeight = Height + size.y;

            return new Vector2((shifted.x % sizeWidth + sizeWidth) % sizeWidth, (shifted.y % sizeHeight + sizeHeight) % sizeHeight) + (_gameWorldMinimum - size /2f);
        }

        public static Vector2 GetRandomVectorInPlayArea()
        {
            const float insideScalar = 0.8f; //makes it impossible to get a point on the edge
            
            return new Vector2(Random.Range(_gameWorldMinimum.x, _gameWorldMaximum.x),
                Random.Range(_gameWorldMinimum.y, _gameWorldMaximum.y)) * insideScalar;
        }

        public static Vector2 GetRandomVectorOutsideOfPlayArea()
        {
            const float width = 1f; //width of the outside "donut"
            const float outsideScalar = 1.1f; //makes sure the object will spawn outside of the play area - not on the edge
            
            var area = Random.Range(0, 4); // select on which side the vector is
            
            switch (area)
            {
                case 0: //Left
                    return new Vector2(Random.Range(_gameWorldMinimum.x - width, _gameWorldMinimum.x),
                        Random.Range(_gameWorldMinimum.y - width, _gameWorldMaximum.y + width)) * outsideScalar;
                case 1: //Top
                    return new Vector2(Random.Range(_gameWorldMinimum.x, _gameWorldMaximum.x), 
                        Random.Range(_gameWorldMaximum.y, _gameWorldMaximum.y + width)) * outsideScalar;
                case 2: //Right
                    return new Vector2(Random.Range(_gameWorldMaximum.x, _gameWorldMaximum.x + width),
                        Random.Range(_gameWorldMinimum.y - width, _gameWorldMaximum.y + width)) * outsideScalar;
                case 3: //Bottom
                    return new Vector2(Random.Range(_gameWorldMinimum.x, _gameWorldMaximum.x),
                        Random.Range(_gameWorldMinimum.y - width, _gameWorldMinimum.y)) * outsideScalar;
            }

            return Vector2.zero;
        }

        public static void CreateSpaceObject(SpaceObjectBehaviour definition)
        {
            
        }
    }
}

