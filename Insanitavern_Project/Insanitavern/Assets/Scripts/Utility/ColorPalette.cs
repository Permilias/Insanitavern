using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorPalette
{
	public ColorPaletteType type;
	public Color[] colors = new Color[1] { Color.white };
	public Gradient[] gradients = new Gradient[1] { new Gradient() };

	public Color RandomColor()
	{
		switch (type)
		{
			case ColorPaletteType.single:
				return colors[0];
			case ColorPaletteType.randomBetweenSingles:
				return colors[Random.Range(0, colors.Length)];
			case ColorPaletteType.gradient:
				return gradients[0].Evaluate(Random.Range(0f, 1f));
			case ColorPaletteType.randomBetweenGradients:
				return gradients[Random.Range(0, gradients.Length)].Evaluate(Random.Range(0f, 1f));
		}

		return Color.black;
	}

	public Color[] GetGradientColors(int stepCount)
	{
		if (type == ColorPaletteType.single | type == ColorPaletteType.randomBetweenSingles)
		{
			Debug.LogError("This palette is not gradient based : cannot return gradient colors");
			return new Color[1] { Color.black };
		}
		float stepIncrement = 1f / (stepCount - 1);
		Color[] returned = new Color[stepCount];
		for (int i = 0; i < stepCount; i++)
		{
			returned[i] = gradients[type == ColorPaletteType.gradient ? 0 : Random.Range(0, gradients.Length)].Evaluate(stepIncrement * i);
		}
		return returned;
	}

	public Color[] GetAllFistColors()
	{
		if (type != ColorPaletteType.randomBetweenGradients)
		{
			Debug.LogError("This palette is not a randomBetweenGradients : cannot return all first colors");
			return new Color[1] { Color.black };
		}
		Color[] returned = new Color[gradients.Length];
		for (int i = 0; i < gradients.Length; i++)
		{
			returned[i] = gradients[i].Evaluate(0);
		}
		return returned;
	}
}

public enum ColorPaletteType
{
	single,
	randomBetweenSingles,
	gradient,
	randomBetweenGradients
}

