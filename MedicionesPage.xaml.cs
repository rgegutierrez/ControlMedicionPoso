using ClosedXML.Excel;
using ControlPoso;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ControlMedicionPoso
{
    public partial class MedicionesPage : ContentPage
    {
        ObservableCollection<DatosMedicion> mediciones = new ObservableCollection<DatosMedicion>();
        DatosMedicionDatabase database;

        public MedicionesPage()
        {
            InitializeComponent();
            medicionesListView.ItemsSource = mediciones;

            // Inicializar la base de datos
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mediciones.db3");
            database = new DatosMedicionDatabase(dbPath);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Cargar mediciones desde la base de datos
            var medicionesList = await database.GetItemsAsync();
            mediciones.Clear();
            foreach (var item in medicionesList)
            {
                mediciones.Add(item);
            }
        }
    }

}
