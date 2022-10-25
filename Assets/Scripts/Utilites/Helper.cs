using UnityEngine;

public class Helper
{
    public static string GetStateString(E_State state)
    {
        switch (state)
        {
            case E_State.Player_Idle: return GlobalString.Player_Idle;
            case E_State.Player_Float: return GlobalString.Player_Float;
            case E_State.Player_Jump: return GlobalString.Player_Jump;
            case E_State.Player_Run: return GlobalString.Player_Run;
            case E_State.Player_Land: return GlobalString.Player_Land;
            case E_State.Player_Fall: return GlobalString.Player_Fall;
            case E_State.Player_Win: return GlobalString.Player_Win;
            case E_State.Player_Die: return GlobalString.Player_Die;
            case E_State.Player_AirJump: return GlobalString.Player_AirJump;
            case E_State.Player_CoyoteTime: return GlobalString.Player_CoyoteTime;
            default:
                Debug.LogError("不存在该state：" + state.ToString());
                break;
        }

        return "";
    }
}