using System.Globalization;

namespace shared.Model;

public class PN : Ordination {
	public double antalEnheder { get; set; }
    public List<Dato> dates { get; set; } = new List<Dato>();

    public PN(DateTime startDen, DateTime slutDen, double antalEnheder, Laegemiddel laegemiddel) : base(laegemiddel, startDen, slutDen)
    {
        this.antalEnheder = antalEnheder;
    }

    public PN() : base(null!, new DateTime(), new DateTime()) {
    }

    /// <summary>
    /// Underviser: Registrerer at der er givet en dosis på dagen givesDen
    /// Underviser: Returnerer true hvis givesDen er inden for ordinationens gyldighedsperiode og datoen huskes
    /// Underviser: Returner false ellers og datoen givesDen ignoreres
    /// AO: .Date da tidspunkter ellers kan give problemer med sammenligningen. Sikrer at vi kun kigger på dato. 
    /// </summary>
    public bool givDosis(Dato givesDen) {
        DateTime startDen = this.startDen.Date;
        DateTime slutDen = this.slutDen.Date;

        if (givesDen == null)
        {
            return false;
        }

        int idCounter = 0;
        if (dates.Count > 0)
        {
            idCounter = dates[dates.Count - 1].DatoId;
        }

        if (startDen <= givesDen.dato.Date && slutDen >= givesDen.dato.Date)
        {
            Dato dateDosisGiven = new Dato
            {
                //AO: Hvis der er ikke er elementer i listen, bliver datoId 1. 
                DatoId = (idCounter > 0 ? idCounter + 1 : 1),
                dato = givesDen.dato,
            };
            dates.Add(dateDosisGiven);
            return true;
        }

        return false;
    }

    /// <summary>
    /// AO: Antallet af enheder der er givet totalt divideret med antal "behandlingsdøgn" 
    /// (de dage hvor der rent faktisk er taget medicin) i indeværende ordinationsperiode. 
    /// </summary>
    /// <returns></returns>
    public override double doegnDosis() {
        if (dates.Count <= 0)
            return 0;

        //AO: Snedig LINQ som finder antallet unique datoer, altså antal "behandlingsdage" i ordinationsperioden. 
        int noOfDaysWTreatment = dates
            .Select(d => d.dato.Date)
            .Distinct()
            .Count();
        
        if(noOfDaysWTreatment > 0)
        {
            return samletDosis() / noOfDaysWTreatment;
        }
        return -1;
    }


    public override double samletDosis() {
        return dates.Count() * antalEnheder;
    }

    public int getAntalGangeGivet() {
        return dates.Count();
    }

	public override String getType() {
		return "PN";
	}
}
