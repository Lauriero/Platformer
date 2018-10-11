using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float GameFieldX = 0;
    public float GameFieldY = 0;
    public float GameFieldWidth = 10.24f;
    public float GameFieldHeigh = 10.24f;

    public Dictionary<Vector2, Sprite> collideObjects = new Dictionary<Vector2, Sprite>();

    private float _speed = 2f;
    private float cellOffset = 0.64f;
    private Vector2 _movementDirection;

    void Start () {
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");

        CamMovement camMove = cam.GetComponent<CamMovement>();
        camMove.player = gameObject;
    }
	
	void Update () {

        //Движения плеера
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.x != 0) {
            _movementDirection.Set(input.x, 0f);
        }
        else if (input.y != 0) {
            _movementDirection.Set(0f, input.y);
        }
        else {
            _movementDirection = Vector2.zero;
        }

        Vector3 destination = transform.position + (Vector3)_movementDirection * _speed * Time.deltaTime;
        
        //Проверка столкновения с краями карты
        if (destination.x < GameFieldX) { destination.x = GameFieldX; }
        else if (destination.y < GameFieldY) { destination.y = GameFieldY; }
        else if (destination.x > GameFieldWidth - GameFieldX) { destination.x = GameFieldWidth - GameFieldX; }
        else if (destination.y > GameFieldHeigh - GameFieldY) { destination.y = GameFieldHeigh - GameFieldY; }

        //Проверка столкновения с объектами
        if (_movementDirection != Vector2.zero) {
            foreach (var collideObject in collideObjects)
            {
                Vector2 valueCoords = collideObject.Key;

                if (destination.y > valueCoords.y - cellOffset - cellOffset / 4 
                    && destination.y < valueCoords.y + cellOffset + cellOffset / 4
                    && destination.x > valueCoords.x - cellOffset - cellOffset / 4
                    && destination.x < valueCoords.x + cellOffset + cellOffset / 4) {

                    destination = transform.position;
                    break;

                }
            }
        }
        
        transform.position = destination;

    }

}
