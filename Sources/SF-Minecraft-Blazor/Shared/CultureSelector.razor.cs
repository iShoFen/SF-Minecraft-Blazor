using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace SF_Minecraft_Blazor.Shared;

public partial class CultureSelector
{
    [Inject] private NavigationManager NavigationManager { get; set; }

    private IEnumerable<CultureInfo> supportedCultures = new List<CultureInfo>
    {
        new("en-US"),
        new("fr-FR")
    };

    private CultureInfo Culture
    {
        get => CultureInfo.CurrentCulture;
        set
        {
            if (CultureInfo.CurrentUICulture.Equals(value)) return;

            var culture = value.Name.ToLower(CultureInfo.InvariantCulture);

            var uri = new Uri(NavigationManager.Uri).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
            var query = $"?culture={Uri.EscapeDataString(culture)}&" + $"redirectUri={Uri.EscapeDataString(uri)}";

            // Redirect the user to the culture controller to set the cookie
            NavigationManager.NavigateTo("/Culture/SetCulture" + query, forceLoad: true);
        }
    }
}