using Data;
using Microsoft.EntityFrameworkCore;
using Service;
using shared.Model;

namespace ordination_test;

[TestClass]
public class PNTest
{
    PN testPN = new PN();
    Dato inputVar = new Dato();
    [TestInitialize]
    public void CreateTestData()
    {
        testPN = new PN(
            new DateTime(2025, 12, 1),
            new DateTime(2025, 12, 5),
            //Antal enheder
            2,
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"));
    }

    [TestMethod]
    public void PN_givDosis_TC1()
    {
        Assert.AreEqual(false, testPN.givDosis(inputVar));
    }

    [TestMethod]
    public void PN_givDosis_TC2()
    {
        inputVar.dato = new DateTime(2025, 12, 3);
        Assert.AreEqual(true, testPN.givDosis(inputVar));
    }

    [TestMethod]
    public void PN_givDosis_TC3()
    {
        inputVar.dato = new DateTime(2025, 12, 1);
        Assert.AreEqual(true, testPN.givDosis(inputVar));
    }

    [TestMethod]
    public void PN_givDosis_TC4()
    {
        inputVar.dato = new DateTime(2025, 12, 5);
        Assert.AreEqual(true, testPN.givDosis(inputVar));
    }

    [TestMethod]
    public void PN_givDosis_TC5()
    {
        inputVar.dato = new DateTime(2025, 12, 6);
        Assert.AreEqual(false, testPN.givDosis(inputVar));
    }

    [TestMethod]
    public void PN_givDosis_TC6()
    {
        inputVar.dato = new DateTime(2025, 12, 28);
        Assert.AreEqual(false, testPN.givDosis(inputVar));
    }

    [TestMethod]
    public void PN_doegnDosis_TC1()
    {
        Assert.AreEqual(0, testPN.doegnDosis());
    }


    [TestMethod]
    public void PN_doegnDosis_TC2()
    {
        inputVar.dato = new DateTime(2025, 12, 1);

        testPN.givDosis(inputVar);

        inputVar.dato = new DateTime(2025, 12, 3);

        testPN.givDosis(inputVar);

        inputVar.dato = new DateTime(2025, 12, 4);

        testPN.givDosis(inputVar);

        //(Antal doser = 3* antal enheder(2) = 6) / (antal dage imellem første og sidste behandling = 4) = 1.5 døgndosis
        Assert.AreEqual(1.5, testPN.doegnDosis());
    }

}
