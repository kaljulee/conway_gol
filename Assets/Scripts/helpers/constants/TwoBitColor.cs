using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TwoBitColor
{
    public const string LIGHTEST = "LIGHTEST";
    public const string LIGHT = "LIGHT";
    public const string DARK = "DARK";
    public const string DARKEST = "DARKEST";
    private static Dictionary<string, T> CreateFourColorDictionary<T>(params T[] colors)
    {
        Dictionary<string, T> dictionary = new Dictionary<string, T>();
        dictionary.Add(LIGHTEST, colors[0]);
        dictionary.Add(LIGHT, colors[1]);
        dictionary.Add(DARK, colors[2]);
        dictionary.Add(DARKEST, colors[3]);
        return dictionary;
    }

    public static Color GenerateTwoBitColor(string shade)
    {
        return new Color(
            gameboyColorsRGB[shade][0],
            gameboyColorsRGB[shade][1],
            gameboyColorsRGB[shade][2]
            );
    }
    public static Dictionary<string, string> CreateFourColorDictionaryHex (params string[] colors)
    {
        return CreateFourColorDictionary(colors);
    }


    public static Dictionary<string, float[]> CreateFourColorDictionaryRGB (params float[][] colors)
    {
        return CreateFourColorDictionary(colors);
    }

    public static Dictionary<string, string> gameboyColorsHex = CreateFourColorDictionary(
        "9bbc0f",
        "8bac0f",
        "306230",
        "0f380f"
        );

    public static Dictionary<string, float[]> gameboyColorsRGB = CreateFourColorDictionary<float[]>(
        new float[3] { 0.6078f, 0.7373f, 0.0588f },
        new float[3] { 0.5450f, 0.6745f, 0.0588f },
        new float[3] { 0.1882f, 0.3843f, 0.1882f },
        new float[3] { 0.0588f, 0.2196f, 0.0588f });
}
