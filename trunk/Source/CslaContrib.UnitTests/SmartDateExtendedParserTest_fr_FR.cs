using System;
using System.Globalization;
using System.Threading;
using Csla;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CslaContrib.UnitTests
{
  /// <summary>
  /// Summary description for SmartDateExtendedParser
  /// </summary>
  [TestClass]
  public class SmartDateExtendedParserTest_fr_FR
  {
    public SmartDateExtendedParserTest_fr_FR()
    {
      SmartDate.CustomParser = SmartDateExtendedParser.ExtendedParser;
      Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
      Separator = DateTimeFormatInfo.CurrentInfo.DateSeparator;
      Pattern = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
      ParseDatePattern();
    }

    private static TestContext _testContextInstance;

    private static string Separator;
    private static string Pattern;

    private static bool _dayBeforeMonth;
    private static bool _yearIsLast;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return _testContextInstance; }
      set { _testContextInstance = value; }
    }

    #region Additional test attributes

    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //

    #endregion

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void Relative_Nearest_Day()
    {
      var day = 10;
      var smartDate = new SmartDate(day.ToString());
      if (DateTime.Now.Day - day < -16)
        Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).AddMonths(-1).ToString("d"), smartDate);
      else if (DateTime.Now.Day - day > 16)
        Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).AddMonths(1).ToString("d"), smartDate);
      else
        Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).ToString("d"), smartDate);

      day = 20;
      smartDate = new SmartDate(day.ToString());
      if (DateTime.Now.Day - day < -16)
        Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).AddMonths(-1).ToString("d"), smartDate);
      else if (DateTime.Now.Day - day > 16)
        Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).AddMonths(1).ToString("d"), smartDate);
      else
        Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).ToString("d"), smartDate);

      day = 28;
      smartDate = new SmartDate(day.ToString());
      if (DateTime.Now.Day - day < -16)
        Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).AddMonths(-1).ToString("d"), smartDate);
      else if (DateTime.Now.Day - day > 16)
        Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).AddMonths(1).ToString("d"), smartDate);
      else
        Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).ToString("d"), smartDate);
    }

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void Relative_Days()
    {
      var smartDate = new SmartDate("+365");
      Assert.AreEqual(DateTime.Now.AddDays(365).ToString("d"), smartDate);

      var fail = "+a1000";
      try
      {
        smartDate = new SmartDate(fail);
        Assert.Fail(fail + ": string value shouldn't be convertible to a date");
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(DateTime.Now.AddDays(365).ToString("d"), smartDate);
      }

      smartDate = new SmartDate("+ 30");
      Assert.AreEqual(DateTime.Now.AddDays(30).ToString("d"), smartDate);

      smartDate = new SmartDate("-365");
      Assert.AreEqual(DateTime.Now.AddDays(-365).ToString("d"), smartDate);

      try
      {
        smartDate = new SmartDate("-a1000");
        Assert.Fail(fail + ": string value shouldn't be convertible to a date");
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(DateTime.Now.AddDays(-365).ToString("d"), smartDate);
      }

      smartDate = new SmartDate("- 30");
      Assert.AreEqual(DateTime.Now.AddDays(-30).ToString("d"), smartDate);
    }

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void Relative_Months()
    {
      var smartDate = new SmartDate("<");
      Assert.AreEqual(DateTime.Now.AddMonths(-1).ToString("d"), smartDate);

      var fail = "<y27";
      try
      {
        smartDate = new SmartDate(fail);
        Assert.Fail(fail + ": string value shouldn't be convertible to a date");
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(DateTime.Now.AddMonths(-1).ToString("d"), smartDate);
      }

      smartDate = new SmartDate("< 24");
      Assert.AreEqual(DateTime.Now.AddMonths(-24).ToString("d"), smartDate);

      smartDate = new SmartDate(">");
      Assert.AreEqual(DateTime.Now.AddMonths(1).ToString("d"), smartDate);

      try
      {
        smartDate = new SmartDate(">y27");
        Assert.Fail(fail + ": string value shouldn't be convertible to a date");
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(DateTime.Now.AddMonths(1).ToString("d"), smartDate);
      }

      smartDate = new SmartDate("> 24");
      Assert.AreEqual(DateTime.Now.AddMonths(24).ToString("d"), smartDate);
    }

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void Relative_Years()
    {
      var smartDate = new SmartDate("<<");
      Assert.AreEqual(DateTime.Now.AddYears(-1).ToString("d"), smartDate);

      var fail = "<<y27";
      try
      {
        smartDate = new SmartDate(fail);
        Assert.Fail(fail + ": string value shouldn't be convertible to a date");
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(DateTime.Now.AddYears(-1).ToString("d"), smartDate);
      }

      smartDate = new SmartDate("<< 10");
      Assert.AreEqual(DateTime.Now.AddYears(-10).ToString("d"), smartDate);

      smartDate = new SmartDate(">>");
      Assert.AreEqual(DateTime.Now.AddYears(1).ToString("d"), smartDate);

      try
      {
        smartDate = new SmartDate(">>y27");
        Assert.Fail(fail + ": string value shouldn't be convertible to a date");
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(DateTime.Now.AddYears(1).ToString("d"), smartDate);
      }

      smartDate = new SmartDate(">> 10");
      Assert.AreEqual(DateTime.Now.AddYears(10).ToString("d"), smartDate);
    }

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void ExactDigits_Four()
    {
      //presume month before day
      var success = "1231";
      var fail = "3011";
      if (_dayBeforeMonth)
      {
        success = "3112";
        fail = "1130";
      }

      var smartDate = new SmartDate(success);
      Assert.AreEqual(new DateTime(DateTime.Now.Year, 12, 31).ToString("d"), smartDate);
      try
      {
        smartDate = new SmartDate(fail);
        Assert.Fail(fail + ": string value shouldn't be convertible to a date");
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(new DateTime(DateTime.Now.Year, 12, 31).ToString("d"), smartDate);
      }
    }

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void ExactDigits_Six()
    {
      //presume month before day
      var success = "1231";
      var fail = "3011";
      if (_dayBeforeMonth)
      {
        success = "3112";
        fail = "1130";
      }
      if (_yearIsLast)
      {
        success += "32";
        fail += "32";
      }
      else
      {
        success = "32" + success;
        fail = "32" + fail;
      }

      var smartDate = new SmartDate(success);
      Assert.AreEqual(new DateTime(1932, 12, 31).ToString("d"), smartDate);
      try
      {
        smartDate = new SmartDate(fail);
        Assert.Fail(fail + ": string value shouldn't be convertible to a date");
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(new DateTime(1932, 12, 31).ToString("d"), smartDate);
      }
    }

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void ExactDigits_Eight()
    {
      //presume month before day
      var success = "1231";
      var fail = "3011";
      if (_dayBeforeMonth)
      {
        success = "3112";
        fail = "1130";
      }
      if (_yearIsLast)
      {
        success += "2015";
        fail += "2015";
      }
      else
      {
        success = "2015" + success;
        fail = "2015" + fail;
      }

      var smartDate = new SmartDate(success);
      Assert.AreEqual(new DateTime(2015, 12, 31).ToString("d"), smartDate);
      try
      {
        smartDate = new SmartDate(fail);
        Assert.Fail(fail + ": string value shouldn't be convertible to a date");
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(new DateTime(2015, 12, 31).ToString("d"), smartDate);
      }
    }

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void ShortForm_DayMonth_Small()
    {
      //presume month before day
      var success = "1" + Separator + "3";
      var fail = "4" + Separator + "2";
      if (_dayBeforeMonth)
      {
        success = "3" + Separator + "1";
        fail = "2" + Separator + "4";
      }

      var smartDate = new SmartDate(success);
      Assert.AreEqual(new DateTime(DateTime.Now.Year, 1, 3).ToString("d"), smartDate);
      try
      {
        smartDate = new SmartDate(fail);
        Assert.AreEqual(new DateTime(DateTime.Now.Year, 4, 2).ToString("d"), smartDate);
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(new DateTime(DateTime.Now.Year, 1, 3).ToString("d"), smartDate);
      }
    }

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void ShortForm_DayMonth_Big()
    {
      //presume month before day
      var success = "12" + Separator + "31";
      var fail = "30" + Separator + "11";
      if (_dayBeforeMonth)
      {
        success = "31" + Separator + "12";
        fail = "11" + Separator + "30";
      }

      var smartDate = new SmartDate(success);
      Assert.AreEqual(new DateTime(DateTime.Now.Year, 12, 31).ToString("d"), smartDate);
      try
      {
        smartDate = new SmartDate(fail);
        if (_dayBeforeMonth)
          Assert.Fail(fail + ": string value shouldn't be convertible to a date");
        else
          Assert.AreEqual(new DateTime(DateTime.Now.Year, 11, 30).ToString("d"), smartDate);
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(new DateTime(DateTime.Now.Year, 12, 31).ToString("d"), smartDate);
      }
    }

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void ShortForm_DayMonthYear_Small()
    {
      //presume month before day
      var success = "1" + Separator + "3";
      var fail = "4" + Separator + "2";
      if (_dayBeforeMonth)
      {
        success = "3" + Separator + "1";
        fail = "2" + Separator + "4";
      }
      if (_yearIsLast)
      {
        success += Separator + "5";
        fail += Separator + "5";
      }
      else
      {
        success = "5" + Separator + success;
        fail = "5" + Separator + fail;
      }

      var smartDate = new SmartDate(success);
      Assert.AreEqual(new DateTime(2005, 1, 3).ToString("d"), smartDate);
      try
      {
        smartDate = new SmartDate(fail);
        Assert.AreEqual(new DateTime(2005, 4, 2).ToString("d"), smartDate);
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(new DateTime(2015, 12, 31).ToString("d"), smartDate);
      }
    }

    [TestMethod]
    [DeploymentItem("SmartDateExtendedParser.dll")]
    public void ShortForm_DayMonthYear_Big()
    {
      //presume month before day
      var success = "12" + Separator + "31";
      var fail = "30" + Separator + "11";
      if (_dayBeforeMonth)
      {
        success = "31" + Separator + "12";
        fail = "11" + Separator + "30";
      }
      if (_yearIsLast)
      {
        success += Separator + "2015";
        fail += Separator + "2015";
      }
      else
      {
        success = "2015" + Separator + success;
        fail = "2015" + Separator + fail;
      }

      var smartDate = new SmartDate(success);
      Assert.AreEqual(new DateTime(2015, 12, 31).ToString("d"), smartDate);
      try
      {
        smartDate = new SmartDate(fail);
        Assert.Fail(fail + ": string value shouldn't be convertible to a date");
      }
      catch (ArgumentException)
      {
        Assert.AreEqual(new DateTime(2015, 12, 31).ToString("d"), smartDate);
      }
    }

    private void ParseDatePattern()
    {
      var patternElements = Pattern.Split(Separator.ToCharArray());
      var dayPatternFound = false;
      var monthFoundAfterDay = false;
      var yearPatternFoundLast = false;
      var valuePointer = 0;

      foreach (var element in patternElements)
      {
        if (element.IndexOf("d", StringComparison.Ordinal) != -1)
        {
          dayPatternFound = true;
          valuePointer = valuePointer + element.Length;
        }
        else if (element.IndexOf("M", StringComparison.Ordinal) != -1)
        {
          if (dayPatternFound)
            monthFoundAfterDay = true;
          valuePointer = valuePointer + element.Length;
        }
        else if (element.IndexOf("y", StringComparison.Ordinal) != -1)
        {
          if (dayPatternFound)
            yearPatternFoundLast = true;
          valuePointer = valuePointer + element.Length;
        }
      }
      if (monthFoundAfterDay)
        _dayBeforeMonth = true;
      if (yearPatternFoundLast)
        _yearIsLast = true;
    }
  }
}
