using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class CurrencyManagerTests
{
    [SetUp]
    public void SetUp()
    {
        // Скидаємо баланс перед кожним тестом
        PlayerPrefs.DeleteKey("CurrencyBalance");
    }

    [Test]
    public void Balance_DefaultsToZero()
    {
        Assert.AreEqual(0, CurrencyManager.Balance);
    }

    [Test]
    public void Add_IncrementsBalance()
    {
        CurrencyManager.Add(50);
        Assert.AreEqual(50, CurrencyManager.Balance);
    }

    [Test]
    public void TrySpend_ReturnsTrueAndDecrements_WhenEnoughBalance()
    {
        CurrencyManager.Add(100);
        bool success = CurrencyManager.TrySpend(30);
        Assert.IsTrue(success);
        Assert.AreEqual(70, CurrencyManager.Balance);
    }

    [Test]
    public void TrySpend_ReturnsFalse_WhenInsufficientBalance()
    {
        CurrencyManager.ResetBalance();
        bool success = CurrencyManager.TrySpend(10);
        Assert.IsFalse(success);
        Assert.AreEqual(0, CurrencyManager.Balance);
    }
}