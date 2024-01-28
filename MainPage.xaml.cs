using ClosedXML.Excel;
using ControlPoso;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ControlMedicionPoso
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<DatosMedicion> mediciones = new ObservableCollection<DatosMedicion>();


        public MainPage()
        {
            InitializeComponent();
            dataCollection.ItemsSource = mediciones;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mediciones.db3");
            var database = new DatosMedicionDatabase(dbPath);

            foreach (var medicion in mediciones)
            {
                await database.SaveItemAsync(medicion);
            }

            await DisplayAlert("Éxito", "Datos guardados en la base de datos SQLite.", "OK");
        }


        private async void OnLoadClicked(object sender, EventArgs e)
        {
            try
            {
                // Limpiar las mediciones existentes antes de cargar nuevas
                mediciones.Clear();

                // Abrir el selector de archivos
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.content" } },
                { DevicePlatform.Android, new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" } },
                { DevicePlatform.WinUI, new[] { ".xlsx" } },
                { DevicePlatform.Tizen, new[] { "*/*" } },
                { DevicePlatform.macOS, new[] { "public.content" } }, // o "com.microsoft.excel.xlsx" si es específico para Excel
            });
                var options = new PickOptions
                {
                    PickerTitle = "Por favor elige un archivo Excel",
                    FileTypes = customFileType,
                };
                var result = await FilePicker.Default.PickAsync(options);

                if (result != null)
                {
                    string fileName = result.FileName;

                    using var stream = await result.OpenReadAsync();
                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet("Base de Datos ");

                    if (worksheet == null)
                    {
                        await DisplayAlert("Error", "La hoja 'Base de datos' no se encontró en el archivo Excel.", "OK");
                        return;
                    }

                    // Leer las dos primeras filas para nombres de variables y unidades
                    var variables = worksheet.Row(1).Cells().Select(c => c.Value.ToString()).ToList();
                    var unidades = worksheet.Row(2).Cells().Select(c => c.Value.ToString()).ToList();

                    // Definir las variables de interés
                    var variablesDeInteres = new List<string> { "Fecha", "pH ", "Cobre Disuelto", "Solidos Totales Disueltos", "Sulfato", "Conductividad" };
                    var indicesDeInteres = new List<int>();

                    // Identificar los índices de las columnas de interés
                    for (int i = 0; i < variables.Count; i++)
                    {
                        if (variablesDeInteres.Contains(variables[i]))
                        {
                            indicesDeInteres.Add(i);
                        }
                    }

                    // Preparar el StringBuilder para recoger los datos
                    var data = new StringBuilder();

                    // Agregar la fecha y el nombre del archivo al principio
                    string fechaActual = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Iterar por cada fila a partir de la tercera
                    for (int row = 3; row <= worksheet.RowsUsed().Count(); row++)
                    {
                        var medicion = new DatosMedicion { NombreDoc = fileName };

                        foreach (var colIndex in indicesDeInteres)
                        {
                            var valor = worksheet.Cell(row, colIndex + 1).Value.ToString();
                            var variable = variables[colIndex];

                            if (variable == "Fecha")
                            {
                                medicion.Fecha = valor.Substring(0, 10); ;
                            }
                            else
                            {
                                // Regex que coincide con espacios o los símbolos < o >
                                string pattern = "[\\s<>]";

                                string processedValue = Regex.Replace(valor, pattern, "");

                                if (decimal.TryParse(processedValue, NumberStyles.Any, CultureInfo.GetCultureInfo("es-ES"), out decimal numberDecimal))
                                {
                                    switch (variable)
                                    {
                                        case "pH ":
                                            medicion.pH = numberDecimal;
                                            break;
                                        case "Cobre Disuelto":
                                            medicion.CobreDisuelto = numberDecimal;
                                            break;
                                        case "Solidos Totales Disueltos":
                                            medicion.SolidosTotalesDisueltos = numberDecimal;
                                            break;
                                        case "Sulfato":
                                            medicion.Sulfato = numberDecimal;
                                            break;
                                        case "Conductividad":
                                            medicion.Conductividad = numberDecimal;
                                            break;
                                    }
                                }
                            }
                        }

                        // Agregar la medición a la colección
                        mediciones.Add(medicion);
                    }
                }

                await DisplayAlert("Éxito", "Datos leidos con éxito", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al leer el archivo: {ex.Message}", "OK");
            }
        }
    }

}
