using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class RecordManagerTests
{
    const string KEY = "HighScore";

    [SetUp]
    public void SetUp()
    {
        // Скидаємо PlayerPrefs перед кожним тестом
        PlayerPrefs.DeleteKey(KEY);
    }

    [Test]
    public void HighScore_DefaultsToZero()
    {
        // Новий проект / скинутий ключ — має бути 0
        Assert.AreEqual(0, RecordManager.HighScore);
    }

    [Test]
    public void TryUpdate_ReturnsTrueAndSets_WhenScoreIsHigher()
    {
        // Встановлюємо початковий рекорд
        PlayerPrefs.SetInt(KEY, 10);
        PlayerPrefs.Save();

        bool updated = RecordManager.TryUpdate(20);

        Assert.IsTrue(updated, "TryUpdate мав повернути true");
        Assert.AreEqual(20, RecordManager.HighScore, "Рекорд мав оновитися до 20");
    }

    [Test]
    public void TryUpdate_ReturnsFalse_WhenScoreIsLowerOrEqual()
    {
        // Встановлюємо початковий рекорд
        PlayerPrefs.SetInt(KEY, 15);
        PlayerPrefs.Save();

        bool updated1 = RecordManager.TryUpdate(10);
        bool updated2 = RecordManager.TryUpdate(15);

        Assert.IsFalse(updated1, "TryUpdate з нижчим значенням мав повернути false");
        Assert.IsFalse(updated2, "TryUpdate з рівним значенням мав повернути false");
        Assert.AreEqual(15, RecordManager.HighScore, "Рекорд не мав змінюватися");
    }

    [Test]
    public void Reset_SetsHighScoreToZero()
    {
        PlayerPrefs.SetInt(KEY, 50);
        PlayerPrefs.Save();

        RecordManager.Reset();

        Assert.AreEqual(0, RecordManager.HighScore, "Після Reset рекорд має бути 0");
    }
}