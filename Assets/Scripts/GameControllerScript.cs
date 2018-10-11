using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{

    #region Инициализация констант

    private const double cellOffset = 0.64;

    #endregion

    #region Инициализация карт
    private int[ , , ] _map;

    #endregion

    #region Инициализация глобальных переменных нужных для генерации

    int rows, columns, layers;
    int emptySprite = -255;

    #endregion

    #region Инициализация префабов

    public GameObject spPref;
    public GameObject plPref;

    #endregion

    #region Спрайты
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

    #endregion

    #region Словари

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
    private Dictionary<Vector2, Sprite> _collideObjects = new Dictionary<Vector2, Sprite>();

    #endregion

    #region Основной исполняющий код

    void Start()
    {
        initializeSprites();
        DateTime starttime = DateTime.Now;
        initializeMap(@"Assets\Maps\map1.txt");
        calculateWaterSprites();
        CreateCells();
        print(DateTime.Now - starttime); //Выводит время считывания карты и её генерации
    }

    /// <summary>
    /// Инициализация обозначения спрайтов на карте
    /// </summary>
    void initializeSprites()
    {
        _sprites.Add(emptySprite, new Sprite());

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

    /// <summary>
    /// Считывает карту из файла
    /// </summary>
    /// <param name="path">Путь к карте</param>
    void initializeMap(String path) {

        using (StreamReader reader = new StreamReader(path)) {

            if (reader.EndOfStream) { return; }

            rows = 1;
            columns = Regex.Split(reader.ReadLine(), " ").Length;
            layers = 1;

            string line;
            while ((line = reader.ReadLine()) != null) {
                rows++;

                int i = Regex.Match(line, @",").Length;
                if (layers < i + 1) {
                    layers = i + 1;
                }
            }
        }
  
        _map = new int[rows, columns, layers];

        using (StreamReader reader = new StreamReader(path)) {
            int counter = -1;

            while (!reader.EndOfStream) {
                string str = reader.ReadLine();
                counter++;

                string[] blocks = Regex.Split(str, " ");
                for (int i = 0; i < blocks.Length; ++i) {
                    string[] ls = Regex.Split(blocks[i], ",");
                    for (int l = 0; l < layers; ++l) {
                        if (l > ls.Length - 1) { _map[counter, i, l] = emptySprite; }
                        else { _map[counter, i, l] = int.Parse(ls[l]); }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Вычисляет и заменяет обозначения земли возле воды на обозначения берегов
    /// </summary>
    void calculateWaterSprites() {
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                if (_sprites[_map[i, j, 0]] == water) {
                    if (j != 0 && _sprites[_map[i, j - 1, 0]] == ground) {
                        if (i != 0 && _sprites[_map[i - 1, j - 1, 0]] == water) {
                            _map[i, j - 1, 0] = -2;
                        } else if (i != rows - 1 && _sprites[_map[i + 1, j - 1, 0]] == water) {
                            _map[i, j - 1, 0] = -7;
                        } else {
                            _map[i, j - 1, 0] = -1;
                        }
                    }
                    if (j != columns - 1 && _sprites[_map[i, j + 1, 0]] == ground) {
                        if (i != rows + 1 && _sprites[_map[i + 1, j + 1, 0]] == water) {
                            _map[i, j + 1, 0] = -4;
                        } else {
                            _map[i, j + 1, 0] = -3;
                        }

                        

                        if (i != 0 && (_sprites[_map[i - 1, j, 0]] == ground || _sprites[_map[i - 1, j, 0]] == waterBorderRightUpCorner)) {
                            _map[i - 1, j + 1, 0] = -9;
                        }
                    }
                    if (i != 0 && _sprites[_map[i - 1, j, 0]] == ground) {
                        if (j != 0 && (_sprites[_map[i, j - 1, 0]] == ground || _sprites[_map[i, j - 1, 0]] == waterBorderLeft)) {
                            _map[i - 1, j - 1, 0] = -9;
                            _map[i - 1, j, 0] = -5;
                        }
                        if (j != columns - 1 && (_sprites[_map[i, j + 1, 0]] == ground || _sprites[_map[i, j + 1, 0]] == waterBorderRight || _sprites[_map[i, j + 1, 0]] == waterBorderRightUpCorner))
                        {
                            _map[i - 1, j + 1, 0] = -9;
                            _map[i - 1, j, 0] = -5;
                        }
                        _map[i - 1, j, 0] = -5;
                    }
                    if (i != rows - 1 && _sprites[_map[i + 1, j, 0]] == ground) {
                        if (j != 0 && (_sprites[_map[i, j - 1, 0]] == ground || _sprites[_map[i, j - 1, 0]] == waterBorderLeft)) {
                            _map[i + 1, j - 1, 0] = -9;
                        }
                        if (j != columns - 1 && (_sprites[_map[i, j + 1, 0]] == ground || _sprites[_map[i, j + 1, 0]] == waterBorderRight || _sprites[_map[i, j + 1, 0]] == waterBorderRightDownCorner))
                        {
                            _map[i + 1, j + 1, 0] = -9;
                        }
                        if (j != 0 && _sprites[_map[i + 1, j - 1, 0]] == water) {
                            _map[i + 1, j, 0] = -8;
                        } else  if (j != columns - 1 && _sprites[_map[i + 1, j + 1, 0]] == water) {
                            _map[i + 1, j, 0] = -2;
                        } else { 
                            _map[i + 1, j, 0] = -6;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Генерирует карту
    /// </summary>
    void CreateCells(){
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                for (int l = 0; l < layers; l++) {

                    if (_map[i, j, l] == emptySprite) { continue; }

                    GameObject tmpCell;
                    if (_map[i, j, l] == 2) {
                        tmpCell = Instantiate(plPref);
                        InitializePlayer(tmpCell);
                    }
                    else {
                        tmpCell = Instantiate(spPref);
                    }
                    tmpCell.GetComponent<Transform>().position = new Vector3((float)(j * cellOffset), (float)((rows - 1) * cellOffset - i * cellOffset), -1 * l);
                    tmpCell.GetComponent<SpriteRenderer>().sprite = _sprites[_map[i, j, l]];

                    if (_sprites[_map[i, j, 0]] == water) {
                        _collideObjects.Add(new Vector2((float)(j * cellOffset), (float)((rows - 1) * cellOffset - i * cellOffset)), water);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Инициализирует плеера на карте
    /// </summary>
    /// <param name="pl">Объект плеера на карте</param>
    void InitializePlayer(GameObject pl) {
        PlayerMovement pM = pl.GetComponent<PlayerMovement>();
        pM.collideObjects = _collideObjects;
        pM.GameFieldX = 0;
        pM.GameFieldY = 0;
        pM.GameFieldWidth = (float)((columns - 1) * cellOffset);
        pM.GameFieldHeigh = (float)((rows - 1) * cellOffset);
    }

    #endregion

}

