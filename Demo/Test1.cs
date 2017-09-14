using UnityEngine;
using UnityEngine.UI;
using KABBOUCHI;

public class Test1 : MonoBehaviour
{

	public Text time;
	public Text logs;

	public InputField input;

	void Awake()
	{
		Konsole.RegisterAll();


		input.onEndEdit.AddListener((value) =>
		{
			Konsole.Excute(value);
			input.text = string.Empty;
		});
	}
	void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}
	void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;

	}

	void HandleLog(string logString, string stackTrace, LogType type)
	{
		logs.text = logString;
	}

	[Kommand("quit", "usage: /quit")]
	void Quit()
	{
		print("Test1::Quit => quiting");
		Application.Quit();
	}

	[Kommand("scaletime", "usage: /scaletime 1")]
	void ScaleTime(float scale)
	{
		Time.timeScale = scale;
		print("Test1::ScaleTime => " + scale);
	}

	[Kommand("runme", "usage: /runme foobar 1")]
	public void RunMe(string var1, int var2)
	{
		print(string.Format("Test1::RunMe => var1='{0}' var2={1}", var1, var2));
	}

	[Kommand("background", "usage: /background red, blue or green")]
	void SetBackground(string color)
	{
		print("Test1::SetBackground " + color);

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
		}

	}

	void Update()
	{
		time.text = Time.time.ToString("##.##");
	}

}
