using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public static class Files
{

    private static List<Recipe> recipes = new List<Recipe>();
    private static Dictionary<string, string> npc_item = new Dictionary<string, string>();

    static Files()
    {
        string[] recipesFromFile = recipes_txt.Split('\n');
        for (int i = 0; i < recipesFromFile.Length; i++)
        {
            string recipe = recipesFromFile[i];
            string[] tokens = recipe.Split('=');

            string result = tokens[1];
            string[] components = tokens[0].Split('+');

            Recipe r = new Recipe(new List<string>(components), result);
            recipes.Add(r);
        }

        string[] npcInteractions = npc_interactions.Split('\n');

        for (int i = 0; i < npcInteractions.Length; i++)
        {
            string[] tokens = npcInteractions[i].Split('=');
            npc_item.Add(tokens[0], tokens[1]);
        }
    }

    private static string recipes_txt =
        "goo+spooky_shroom+jar1=magnet_potion\n" +
        "spiky_flower+spooky_shroom+jar2=cloud_potion\n" +
        "spiky_flower+shroom+jar3=stomach_potion\n" +
        "apple+shroom2+jar4=enlarge_potion";

    private static string npc_interactions =
        "npc_toilet=stomach_potion\n" + 
        "npc_cornfield=enlarge_potion\n" +
        "npc_half_guy=magnet_potion\n" + 
        "npc_dead_guy=cloud_potion\n" + 
        "npc_frog_guy=stomach_potion";

    public static bool CheckNpcItem(string npcId, string itemId)
    {
        return npc_item.ContainsKey(npcId) && npc_item[npcId].Equals(itemId);
    }

    public static string GetRecipe(List<Item> items)
    {
        List<string> components = new List<string>();
        for (int i = 0; i < items.Count; i++)
        {
            components.Add(items[i].Id);
        }

        for (int i = 0; i < recipes.Count; i++)
        {
            if (recipes[i].Matches(components))
            {
                return recipes[i].result;
            }
        }

        return null;
    }
}
