using Data;
using Microsoft.EntityFrameworkCore;
using Service;
using shared.Model;

namespace ordination_test;

[TestClass]
public class DagligSkaevTest
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
    public void OpretDagligSkaevSucessTest()
    {
        //Arrange 

        var dagligSkaev = service.GetDagligSkæve();
        var patienter = service.GetPatienter();
        var laegmidler = service.GetLaegemidler();

        int forventet = dagligSkaev.Count+1;

        //Act

        service.OpretDagligSkaev(patienter[0].PatientId, laegmidler[0].LaegemiddelId, new Dosis[] {},DateTime.Now.AddDays(-5),DateTime.Now.AddDays(5));

        dagligSkaev = service.GetDagligSkæve();

        //Assert
        
        Assert.AreEqual(forventet, dagligSkaev.Count);
    }

   


}
