using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public bool canMoveUp { get; set; }

    public bool canMoveDown { get; set; }

    public bool canMoveLeft { get; set; }

    public bool canMoveRight { get; set; }

    public int Direction { get; set; }

    public Vector3 TpToward { get; set; }


}
