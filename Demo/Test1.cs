using UnityEngine;
using UnityEngine.UI;
using KABBOUCHI;

public class Test1 : MonoBehaviour {

	public InputField input;
    public Text time;


    void Awake()
    {
        input.onEndEdit.AddListener((value)=>{
            KonsoleManager.Instance.Excute(value);
            input.text = "";
        }); 
    }

    private void Update()
    {
        time.text = Time.time.ToString("##.##");
    }

    [Konsole("runme")]
    public void RunMe (string var1,int var2) {
        print("RunMe: var1='"+ var1+ "' var2=" + var2);
	}

    [Konsole("scaletime","usage: /scaletime 1")]
	void ScaleTime(float scale)
	{
        Time.timeScale = scale;
        print("TimeScale: " + scale);
	}

	[Konsole("background", "usage: /background red, blue or green")]
	void SetBackground(string color)
	{
        switch (color)
        {
            case "red":
                Camera.main.backgroundColor = Color.red;
                break;
			case "blue":
				Camera.main.backgroundColor = Color.blue;
                break;
			case "green":
				Camera.main.backgroundColor = Color.green;
                break;
            default:
                break;
        }

	}
	
}
