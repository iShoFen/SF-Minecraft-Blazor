using Blazorise;

namespace SF_Minecraft_Blazor;

public partial class App
{
    private readonly Theme _theme = new()
    {
        ColorOptions = new ThemeColorOptions
        {
            Primary = "#2b2d42",
            Dark = "#000814",
            Light = "#FFFFFF"
        }
    };
}