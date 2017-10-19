using System.Collections;
using UnityEngine;

public class ParticaleRotation : MonoBehaviour
{
    // Floats
	public float x_Rotation = 0F;
	public float y_Rotation = 0F;
	public float z_Rotation = 0F;

	void OnEnable()
    {
		InvokeRepeating("rotate", 0f, 0.0167f);
	}
	void OnDisable()
    {
		CancelInvoke();
	}
	public void clickOn()
    {
		InvokeRepeating("rotate", 0f, 0.0167f);
	}
	public void clickOff()
    {
		CancelInvoke();
	}
	void rotate()
    {
		this.transform.localEulerAngles += new Vector3(x_Rotation, y_Rotation, z_Rotation);
	}
}
