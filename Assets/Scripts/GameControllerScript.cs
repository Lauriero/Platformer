using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{

    private const double cellOffset = 0.64;

    private int[,] _map = {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0},
        {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
        {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0},
        {0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0},
        {0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0},
        {1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0},
        {0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0},
        {0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0},
        {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1}
    };


    public int playerSpawnPoint = 1;

    int rows, columns;

    public GameObject spPref;
    public GameObject plPref;


    public Sprite waterBorderUp;
    public Sprite waterBorderDown;
    public Sprite waterBorderRight;
    public Sprite waterBorderLeft;
    public Sprite waterBorderUpLeftCorner;
    public Sprite waterBorderRightDownCorner;
    public Sprite waterBorderRightUpCorner;
    public Sprite waterBorderLeftDownCorner;
    public Sprite waterBorderLeftUpCorner;

    public Sprite water;
    public Sprite ground;
    public Sprite player;
    public Sprite coin;
    public Sprite enemy1;
    public Sprite enemy2;
    public Sprite spike;
    public Sprite win;

    private Dictionary<int, int> _layers = new Dictionary<int, int>() {
        {-1, 0},
        {-2, 0},
        {-3, 0},
        {-4, 0},
        {-5, 0},
        {-6, 0},
        {-7, 0},
        {-8, 0},
        {-9, 0},
        {0, 0},
        {1, 0},
        {2, 1},
    };

    private Dictionary<int, Sprite> _sprites = new Dictionary<int, Sprite>();
    //private Dictionary<Vector2, GameObject> _colliderGateawayes = new Dictionary<Vector2, GameObject>();
    private Dictionary<Vector2, Sprite> _collideObjects = new Dictionary<Vector2, Sprite>();

    // Use this for initialization
    void Start()
    {
        rows = _map.GetUpperBound(0) + 1;
        columns = _map.Length / rows;

        initializeSprites();
        CreateCells();
    }

    void initializeSprites()
    {
        _sprites.Add(-9, waterBorderUpLeftCorner);
        _sprites.Add(-8, waterBorderRightDownCorner);
        _sprites.Add(-7, waterBorderLeftUpCorner);
        _sprites.Add(-6, waterBorderDown);
        _sprites.Add(-5, waterBorderUp);
        _sprites.Add(-4, waterBorderRightUpCorner);
        _sprites.Add(-3, waterBorderRight);
        _sprites.Add(-2, waterBorderLeftDownCorner);
        _sprites.Add(-1, waterBorderLeft);
        _sprites.Add(0, ground);
        _sprites.Add(1, water);
        _sprites.Add(2, player);
    }

    void CreateCells()
    {

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                if (_sprites[_map[i, j]] == water) {
                    if (j != 0 && _sprites[_map[i, j - 1]] == ground) {
                        if (i != 0 && _sprites[_map[i - 1, j - 1]] == water) {
                            _map[i, j - 1] = -2;
                        } else if (i != rows - 1 && _sprites[_map[i + 1, j - 1]] == water) {
                            _map[i, j - 1] = -7;
                        } else {
                            _map[i, j - 1] = -1;
                        }
                    }
                    if (j != columns - 1 && _sprites[_map[i, j + 1]] == ground) {
                        if (i != rows + 1 && _sprites[_map[i + 1, j + 1]] == water) {
                            _map[i, j + 1] = -4;
                        } else {
                            _map[i, j + 1] = -3;
                        }

                        if (i != 0 && (_sprites[_map[i - 1, j]] == ground || _sprites[_map[i - 1, j]] == waterBorderRightUpCorner)) {
                            _map[i - 1, j + 1] = -9;
                        }
                    }
                    if (i != 0 && _sprites[_map[i - 1, j]] == ground) {
                        if (j != 0 && (_sprites[_map[i, j - 1]] == ground || _sprites[_map[i, j - 1]] == waterBorderLeft)) {
                            _map[i - 1, j - 1] = -9;
                            _map[i - 1, j] = -5;
                        }
                        if (j != columns - 1 && (_sprites[_map[i, j + 1]] == ground || _sprites[_map[i, j + 1]] == waterBorderRight || _sprites[_map[i, j + 1]] == waterBorderRightUpCorner))
                        {
                            _map[i - 1, j + 1] = -9;
                            _map[i - 1, j] = -5;
                        }
                        _map[i - 1, j] = -5;
                    }
                    if (i != rows - 1 && _sprites[_map[i + 1, j]] == ground) {
                        if (j != 0 && (_sprites[_map[i, j - 1]] == ground || _sprites[_map[i, j - 1]] == waterBorderLeft)) {
                            _map[i + 1, j - 1] = -9;
                        }
                        if (j != columns - 1 && (_sprites[_map[i, j + 1]] == ground || _sprites[_map[i, j + 1]] == waterBorderRight || _sprites[_map[i, j + 1]] == waterBorderRightDownCorner))
                        {
                            _map[i + 1, j + 1] = -9;
                        }
                        if (j != 0 && _sprites[_map[i + 1, j - 1]] == water) {
                            _map[i + 1, j] = -8;
                        } else  if (_sprites[_map[i + 1, j + 1]] == water) {
                            _map[i + 1, j] = -2;
                        } else { 
                            _map[i + 1, j] = -6;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {

                Debug.Log(_map[i, j] + " ");

                GameObject tmpCell = Instantiate(spPref);
                tmpCell.GetComponent<Transform>().position = new Vector3((float)(j * cellOffset), (float)((rows - 1) * cellOffset - i * cellOffset), _layers[_map[i, j]]);
                tmpCell.GetComponent<SpriteRenderer>().sprite = _sprites[_map[i, j]];

                if (_sprites[_map[i, j]] == water) {
                    _collideObjects.Add(new Vector2((float)(j * cellOffset), (float)((rows - 1) * cellOffset - i * cellOffset)), water);
                }
            }
            Debug.Log('\n');
        }

        GameObject pl = Instantiate(plPref);

        PlayerMovement pM = pl.GetComponent<PlayerMovement>();
        pM.collideObjects = _collideObjects;
        pM.GameFieldX = 0;
        pM.GameFieldY = 0;
        pM.GameFieldWidth = (float) ((columns - 1) * cellOffset);
        pM.GameFieldHeigh = (float) ((rows - 1) * cellOffset);
    }
}

