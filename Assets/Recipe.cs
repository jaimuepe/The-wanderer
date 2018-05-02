using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    public readonly List<string> components;
    public readonly string result;

    public Recipe(List<string> components, string result)
    {
        this.components = components;
        this.result = result;
    }

    public bool Matches(List<string> matchingComponents)
    {
        if (matchingComponents.Count != components.Count)
        {
            return false;
        }

        List<string> tempCompList = new List<string>(components);

        for (int i = 0; i < matchingComponents.Count; i++)
        {
            tempCompList.Remove(matchingComponents[i]);
        }

        return tempCompList.Count == 0;
    }
}
