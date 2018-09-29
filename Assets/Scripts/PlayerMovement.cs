using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 0.64f;

    public Movement m;

    public float GameFieldX = 0;
    public float GameFieldY = 0;
    public float GameFieldWidth = 10.24f;
    public float GameFieldHeigh = 10.24f;

    public Dictionary<Vector2, Sprite> collideObjects = new Dictionary<Vector2, Sprite>();


    private float cellOffset = 0.64f;
    private Vector2 _movementDirection;

    void Start () {

        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");

        CamMovement camMove = cam.GetComponent<CamMovement>();
        camMove.player = gameObject;

        m = gameObject.GetComponent<Movement>();

        m.canMoveUp = true;
        m.canMoveDown = true;
        m.canMoveLeft = true;
        m.canMoveRight = true;
    }
	
	void Update () {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.x != 0)
        {
            if ((input.x < 0 && !m.canMoveRight) || (input.x > 0 && !m.canMoveLeft))
            {
                return;
            }
            m.Direction = (int)input.x;
            _movementDirection.Set(input.x, 0f);
        }
        else if (input.y != 0)
        {
            if ((input.y < 0 && !m.canMoveDown) || (input.y > 0 && !m.canMoveUp))
            {
                return;
            }
            m.Direction = (int)input.y * 2;
            _movementDirection.Set(0f, input.y);
        }
        else
        {
            m.Direction = 0;
            _movementDirection = Vector2.zero;
        }

        Vector3 destination = transform.position + (Vector3)_movementDirection * speed * Time.deltaTime;

        if (destination.x < GameFieldX) { destination.x = GameFieldX; }
        else if (destination.y < GameFieldY) { destination.y = GameFieldY; }
        else if (destination.x > GameFieldWidth - GameFieldX) { destination.x = GameFieldWidth - GameFieldX; }
        else if (destination.y > GameFieldHeigh - GameFieldY) { destination.y = GameFieldHeigh - GameFieldY; }

        if (_movementDirection != Vector2.zero) {
            foreach (var collideObject in collideObjects)
            {
                Vector2 valueCoords = collideObject.Key;

                if ((destination.y > valueCoords.y - cellOffset - 0.16f && destination.y < valueCoords.y + cellOffset + 0.16f) && (destination.x > valueCoords.x - cellOffset - 0.16f && destination.x < valueCoords.x + cellOffset + 0.16f)) {

                    print("g");
                    destination = transform.position;
                    break;

                }


                //if ((destination.x >= valueCoords.x - cellOffset - 0.16f && destination.x <= valueCoords.x + 0.16f) && (destination.y >= valueCoords.y - cellOffset - 0.16f && destination.y <= valueCoords.y + 0.16f) ||
                //    (destination.x <= valueCoords.x + cellOffset + 0.16f && destination.x >= valueCoords.x - 0.16f) && (destination.y <= valueCoords.y + cellOffset + 0.16f && destination.y >= valueCoords.y - 0.16f))
                //{
                //    print("g");
                //    destination = transform.position;
                //    break;
                //}
            }
        }
        

        transform.position = destination;

    }

}
