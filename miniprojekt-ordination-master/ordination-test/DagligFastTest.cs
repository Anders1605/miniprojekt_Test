using Data;
using Microsoft.EntityFrameworkCore;
using Service;
using shared.Model;

namespace ordination_test;

[TestClass]
public class DagligFastTest
{
    DagligFast testDagligFast = new DagligFast();

    [TestInitialize]
    public void CreateTestData()
    {
        testDagligFast = new DagligFast(
            new DateTime(2025, 12, 1),
            new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"),
            2, 0, 1, 0);
    }

    /// <summary>
    /// Vi kigger på doegnDosis() tests her
    /// Tester at døgndosis beregnes korrekt som summen af alle 4 doser
    /// Formel: morgen + middag + aften + nat
    /// </summary>

    [TestMethod]
    public void DagligFast_doegnDosis_TC1()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 0, 0, 0, 1);
        Assert.AreEqual(1.0, ordination.doegnDosis());
    }

    [TestMethod]
    public void DagligFast_doegnDosis_TC2()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 1, 1, 1, 1);
        Assert.AreEqual(4.0, ordination.doegnDosis());
    }

    [TestMethod]
    public void DagligFast_doegnDosis_TC3()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 3, 4, 1, 2);
        Assert.AreEqual(10.0, ordination.doegnDosis());
    }

    [TestMethod]
    public void DagligFast_doegnDosis_TC4()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 0, 0, 0, 0);
        Assert.AreEqual(0.0, ordination.doegnDosis());
    }

    [TestMethod]
    public void DagligFast_doegnDosis_TC5()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 5, 0, 0, 0);
        Assert.AreEqual(5.0, ordination.doegnDosis());
    }

    /// <summary>
    /// Vi kigger på samletDosis() tests her
    /// Tester at samlet dosis beregnes korrekt over hele perioden
    /// Formel: antalDage() * doegnDosis()
    /// antalDage() inkluderer både start- og slutdato
    /// </summary>

    [TestMethod]
    public void DagligFast_samletDosis_TC1()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 1),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 0, 0, 0, 1);
        Assert.AreEqual(1.0, ordination.samletDosis());
    }

    [TestMethod]
    public void DagligFast_samletDosis_TC2()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 3),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 1, 1, 1, 1);
        Assert.AreEqual(12.0, ordination.samletDosis());
    }

    [TestMethod]
    public void DagligFast_samletDosis_TC3()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 3, 4, 1, 2);
        Assert.AreEqual(50.0, ordination.samletDosis());
    }

    [TestMethod]
    public void DagligFast_samletDosis_TC4()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 10),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 0, 0, 0, 0);
        Assert.AreEqual(0.0, ordination.samletDosis());
    }

    [TestMethod]
    public void DagligFast_samletDosis_TC5()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 2),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 5, 0, 0, 0);
        Assert.AreEqual(10.0, ordination.samletDosis());
    }

    /// <summary>
    /// Vi kigger på getDoser() tests her
    /// Tester at getDoser() returnerer et array med præcis 4 Dosis objekter
    /// Rækkefølgen er altid: [0]=Morgen, [1]=Middag, [2]=Aften, [3]=Nat
    /// </summary>

    [TestMethod]
    public void DagligFast_getDoser_TC1()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 0, 0, 0, 1);
        Dosis[] doser = ordination.getDoser();
        Assert.AreEqual(4, doser.Length);
        Assert.AreEqual(0, doser[0].antal);
        Assert.AreEqual(0, doser[1].antal);
        Assert.AreEqual(0, doser[2].antal);
        Assert.AreEqual(1, doser[3].antal);
    }

    [TestMethod]
    public void DagligFast_getDoser_TC2()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 1, 1, 1, 1);
        Dosis[] doser = ordination.getDoser();
        Assert.AreEqual(4, doser.Length);
        Assert.AreEqual(1, doser[0].antal);
        Assert.AreEqual(1, doser[1].antal);
        Assert.AreEqual(1, doser[2].antal);
        Assert.AreEqual(1, doser[3].antal);
    }

    [TestMethod]
    public void DagligFast_getDoser_TC3()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 3, 4, 1, 2);
        Dosis[] doser = ordination.getDoser();
        Assert.AreEqual(4, doser.Length);
        Assert.AreEqual(3, doser[0].antal);
        Assert.AreEqual(4, doser[1].antal);
        Assert.AreEqual(1, doser[2].antal);
        Assert.AreEqual(2, doser[3].antal);
    }

    [TestMethod]
    public void DagligFast_getDoser_TC4()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 2, 3, 1, 1);
        Dosis[] doser = ordination.getDoser();
        Assert.AreEqual(4, doser.Length);
    }

    [TestMethod]
    public void DagligFast_getDoser_TC5()
    {
        DagligFast ordination = new DagligFast(new DateTime(2025, 12, 1), new DateTime(2025, 12, 5),
            new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 1, 2, 3, 4);
        Dosis[] doser = ordination.getDoser();
        Assert.AreEqual(1, doser[0].antal);
        Assert.AreEqual(2, doser[1].antal);
        Assert.AreEqual(3, doser[2].antal);
        Assert.AreEqual(4, doser[3].antal);
    }
}