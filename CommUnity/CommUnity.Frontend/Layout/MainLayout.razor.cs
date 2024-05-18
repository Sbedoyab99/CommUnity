
using MudBlazor;

namespace CommUnity.FrontEnd.Layout
{
    public partial class MainLayout
    {
        private bool _drawerOpen = false;
        private bool _darkMode { get; set; } = false;
        private string _icon = Icons.Material.Filled.DarkMode;

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        private void DarkModeToggle()
        {
            _darkMode = !_darkMode;
            _icon = _darkMode ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode;
        }
    }
}