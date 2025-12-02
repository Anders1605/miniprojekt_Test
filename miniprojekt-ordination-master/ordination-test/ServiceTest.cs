namespace ordination_test;

using Microsoft.EntityFrameworkCore;

using Service;
using Data;
using shared.Model;

[TestClass]
public class ServiceTest
{
    private DataService service;

    [TestInitialize]
    public void SetupBeforeEachTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
        service.SeedData();
    }

    [TestMethod]
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }

    [TestMethod]
    public void OpretDagligFast()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());
    }

    [TestMethod] // CSB
    public void GetAnbefaletDosisPerDoegn_NormalVaegt_ReturnererNormalDose()
    {
        // ARRANGE
        // Vi henter en patient med vægt mellem 25 og 120 
        List<Patient> patienter = service.GetPatienter();
        List<Laegemiddel> laegemidler = service.GetLaegemidler();

        Patient patient = patienter[0];
        Laegemiddel lm = laegemidler[1];

        double forventet = lm.enhedPrKgPrDoegnNormal;

        // ACT
        double faktisk = service.GetAnbefaletDosisPerDøgn(patient.PatientId, lm.LaegemiddelId);

        // ASSERT
        Assert.AreEqual(forventet, faktisk);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestAtKodenSmiderEnException()
    {
        // Herunder skal man så kalde noget kode,
        // der smider en exception.

        // Hvis koden _ikke_ smider en exception,
        // så fejler testen.

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }

}