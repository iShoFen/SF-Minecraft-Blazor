using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace SF_Minecraft_Blazor.Shared;

public partial class CultureSelector
{
    [Inject] public NavigationManager NavigationManager { get; set; }

    private CultureInfo[] supportedCultures = {
        new("en-US"),
        new("fr-FR")
    };

    private CultureInfo Culture
    {
        get => CultureInfo.CurrentCulture;
        set
        {
            if (CultureInfo.CurrentUICulture == value)
            {
                return;
            }

            var culture = value.Name.ToLower(CultureInfo.InvariantCulture);

            var redirectUri = new Uri(NavigationManager.Uri).GetComponents(UriComponents.PathAndQuery,
                UriFormat.Unescaped);
            var query = $"?culture={Uri.EscapeDataString(culture)}&" + $"redirectUri={Uri.EscapeDataString(redirectUri)}";

            // Redirect the user to the culture controller to set the cookie
            NavigationManager.NavigateTo("/Culture/SetCulture" + query, forceLoad: true);
        }
    }
}