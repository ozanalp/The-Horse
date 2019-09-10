using UnityEngine;

public class ComboMovement : MonoBehaviour
{
    public static float x, y;
    private static bool x_down, y_down;

    private void Start()
    {
        x = 0; y = 0;
        x_down = false; y_down = false;
    }
    private void Update()
    {
        GetInput(ref x, ref x_down, "Horizontal");
        GetInput(ref y, ref y_down, "Vertical");
    }

    private void GetInput(ref float val, ref bool down, string axis)
    {
        float input = Input.GetAxisRaw(axis);
        input = (input > 0) ? 1 : ((input < 0) ? -1 : 0); // forces controls to be -1, 0, 1 not anything between
        if (input != val)
        {
            val = input;
            down = (val != 0) ? true : false;
        }
        else
        {
            down = false;
        }
    }

    public static Vector2 GetMovement()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public static bool InputDownX()
    {
        return x_down;
    }

    public static bool InputDownY()
    {
        return y_down;
    }
}
