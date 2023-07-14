public enum SceneNames
{
    OUTDOOR_TEST,
    POKEMON_CENTER,
    CITY_TEST,
    GATE,
    DEFAULT
}

public class ScenesMapping
{
    public static string FromSceneNameEnumToSceneName(SceneNames sceneNames)
    {
        switch(sceneNames)
        {
            case SceneNames.CITY_TEST: return "CityTest";
            case SceneNames.GATE: return "Gate";
            case SceneNames.OUTDOOR_TEST: return "OutdoorTest";
            case SceneNames.POKEMON_CENTER: return "PokemonCenter";
            default: return null;
        }
    }

    public static SceneNames FromSceneNameStringToSceneNameEnum(string sceneName)
    {
        switch(sceneName)
        {
            case "PokemonCenter": return SceneNames.POKEMON_CENTER;
            case "OutdoorTest": return SceneNames.OUTDOOR_TEST;
            case "Gate": return SceneNames.GATE;
            case "CityTest": return SceneNames.CITY_TEST;
            default: return SceneNames.DEFAULT;
        }
    }
}