namespace ControlMedicionPoso
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MedicionesPage), typeof(MedicionesPage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
            Routing.RegisterRoute(nameof(IngresoMedicionPage), typeof(IngresoMedicionPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}
