using SQLite;

namespace ControlPoso;

public class DatosMedicion
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [Column("Fecha")]
    public string Fecha { get; set; }

    [Column("pH")]
    public decimal pH { get; set; }

    [Column("CobreDisuelto")]
    public decimal CobreDisuelto { get; set; }

    [Column("SolidosTotalesDisueltos")]
    public decimal SolidosTotalesDisueltos { get; set; }

    [Column("Sulfato")]
    public decimal Sulfato { get; set; }

    [Column("Conductividad")]
    public decimal Conductividad { get; set; }

    [Column("NombreDoc")]
    public string NombreDoc { get; set; }
}

