using ClosedXML.Excel;
using ControlPoso;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ControlMedicionPoso
{
    public partial class IngresoMedicionPage : ContentPage
    {
        public IngresoMedicionPage()
        {
            InitializeComponent();
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            var medicion = new DatosMedicion
            {
                Fecha = fechaPicker.Date.ToString("dd-MM-yyyy"),
                pH = decimal.Parse(phEntry.Text, CultureInfo.InvariantCulture),
                CobreDisuelto = decimal.Parse(cobreDisueltoEntry.Text, CultureInfo.InvariantCulture),
                SolidosTotalesDisueltos = decimal.Parse(solidosTotalesDisueltosEntry.Text, CultureInfo.InvariantCulture),
                Sulfato = decimal.Parse(sulfatoEntry.Text, CultureInfo.InvariantCulture),
                Conductividad = decimal.Parse(conductividadEntry.Text, CultureInfo.InvariantCulture),
                NombreDoc = nombreDocEntry.Text
            };


            // Aquí agregarías el código para guardar la instancia de DatosMedicion en la base de datos
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mediciones.db3");
            var database = new DatosMedicionDatabase(dbPath);
            await database.SaveItemAsync(medicion);

            // Mostrar confirmación
            await DisplayAlert("Guardado", "La medición ha sido guardada correctamente.", "OK");
        }
    }
}
