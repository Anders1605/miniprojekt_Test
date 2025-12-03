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

	[TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
	public void TestAtKodenSmiderEnException()
	{
		var patienter = service.GetPatienter();
		var lægemidler = service.GetLaegemidler();
		var ordinationer = service.GetPNs();

		patienter[0].PatientId = 20;

		//Act

		var PNToTest = service.OpretPN(patienter[0].PatientId, lægemidler[0].LaegemiddelId, 10, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5));

		Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
	}

	[TestMethod]
	public void AvendOrdinationTestSuccess()
	{

		//Arrange
		var patienter = service.GetPatienter();
		var lægemidler = service.GetLaegemidler();
		var ordinationer = service.GetPNs();


		//Act

		var PNToTest = service.OpretPN(patienter[0].PatientId, lægemidler[0].LaegemiddelId, 10, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5));
		var result = service.AnvendOrdination(PNToTest.OrdinationId, new Dato() { dato = DateTime.Now });

		//Assert
		Assert.AreEqual("Ordination anvendt.", result);
	}

	[TestMethod]
	public void OpretPNTest()
	{

		//Arrange
		var patienter = service.GetPatienter();
		var lægemidler = service.GetLaegemidler();
		var ordinationer = service.GetPNs();

		int forventet = ordinationer.Count + 1;

		//Act

		var PNToTest = service.OpretPN(patienter[0].PatientId, lægemidler[0].LaegemiddelId, 10, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5));

		ordinationer = service.GetPNs();

		//Assert

		Assert.AreEqual(forventet, ordinationer.Count);
	}
}