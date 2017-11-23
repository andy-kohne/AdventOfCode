<Query Kind="Program" />

const int screen_width = 50;
const int screen_height = 6;

void Main()
{
	var instructions = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day8.txt"));
	var screen = new bool[screen_width, screen_height];

	foreach (var i in instructions)
	{
		var parts = i.Split(new[] { ' ' }, 2);
		if (parts[0].Equals("rect"))
		{
			var parms = parts[1].Split('x');
			Rect(screen, int.Parse(parms[0]), int.Parse(parms[1]));
		}
		else if (parts[0].Equals("rotate"))
		{
			var parms = parts[1].Split('=');
			var components = parms[1].Split(new[] { " by " }, StringSplitOptions.None);
			if (parts[1].StartsWith("row"))
			{
				RotateRow(screen, int.Parse(components[0]), int.Parse(components[1]));
			}
			else
			{
				RotateColumn(screen, int.Parse(components[0]), int.Parse(components[1]));
			}
		}
		else
		{
		}
	}

	//part 1
	Count(screen).Dump();

	// part 2
	Render(screen);
}

void Rect(bool[,] screen, int width, int height)
{
	for (int w = 0; w < width; w++)
	{
		for (int h = 0; h < height; h++)
		{
			screen[w, h] = true;
		}
	}
}
void RotateColumn(bool[,] screen, int column, int count)
{
	var t = new bool[screen_height];
	for (int h = 0; h < screen_height; h++)
	{
		t[h] = screen[column, (h + (screen_height - (count % screen_height))) % screen_height];
	}
	for (int h = 0; h < screen_height; h++)
	{
		screen[column, h] = t[h];
	}

}
void RotateRow(bool[,] screen, int row, int count)
{
	var t = new bool[screen_width];
	for (int h = 0; h < screen_width; h++)
	{
		t[h] = screen[(h + (screen_width - (count % screen_width))) % screen_width, row];
	}
	for (int h = 0; h < screen_width; h++)
	{
		screen[h, row] = t[h];
	}

}
void Render(bool[,] screen)
{
	var sb = new StringBuilder();
	for (int h = 0; h < screen_height; h++)
	{
		for (int w = 0; w < screen_width; w++)
		{
			sb.Append(screen[w, h] ? "0" : " ");
		}
		sb.AppendLine();
	}
	var tempFile = Path.GetTempFileName();
	File.WriteAllText(tempFile, sb.ToString());
	Process.Start("notepad", $"\"{tempFile}\"");
}
int Count(bool[,] screen)
{
	var ret = 0;
	for (int h = 0; h < screen_height; h++)
	{
		for (int w = 0; w < screen_width; w++)
		{
			if (screen[w, h])
				ret++;
		}
	}
	return ret;
}
