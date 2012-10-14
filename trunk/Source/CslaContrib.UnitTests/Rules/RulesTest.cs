using System.ComponentModel;
using CslaContrib.Rules.CommonRules;
using CslaContrib.UnitTests.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Csla.Core;
using Csla.Rules;

namespace CslaContrib.UnitTests
{


  /// <summary>
  ///This is a test class for CalcSumTest and is intended
  ///to contain all CalcSumTest Unit Tests
  ///</summary>
  [TestClass()]
  public class RulesTest
  {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion



    [TestMethod()]
    [DeploymentItem("CslaContrib.dll")]
    public void Rules_LessThan_Test()
    {
      var root = RuleTestRoot.NewEditableRoot();
      // no rules are run for this new object

      Assert.IsTrue(root.IsValid);
      // Num1 must be less than Num5 (=0)
      root.Num1 = 1;
      Assert.IsFalse(root.IsValid);
      Assert.AreEqual("rule://cslacontrib.rules.commonrules.lessthan/Num1", root.BrokenRulesCollection[0].RuleName);
      root.Num1 = -1;
      Assert.IsTrue(root.IsValid);
      root.Num1 = 0;
      Assert.IsFalse(root.IsValid);
      Assert.AreEqual("rule://cslacontrib.rules.commonrules.lessthan/Num1", root.BrokenRulesCollection[0].RuleName);
      root.Num1 = -1;
      Assert.IsTrue(root.IsValid);
    }

    [TestMethod()]
    [DeploymentItem("CslaContrib.dll")]
    public void Rules_LessThanOrEqual_Test()
    {
      var root = RuleTestRoot.NewEditableRoot();
      // no rules are run for this new object

      Assert.IsTrue(root.IsValid);
      // Num2 must be less than Num5 (=0)
      root.Num2 = 1;
      Assert.IsFalse(root.IsValid);
      Assert.AreEqual("rule://cslacontrib.rules.commonrules.lessthanorequal/Num2", root.BrokenRulesCollection[0].RuleName);
      root.Num2 = 0;
      Assert.IsTrue(root.IsValid);
      root.Num2 = 1;
      Assert.IsFalse(root.IsValid);
      Assert.AreEqual("rule://cslacontrib.rules.commonrules.lessthanorequal/Num2", root.BrokenRulesCollection[0].RuleName);
      root.Num2 = -1;
      Assert.IsTrue(root.IsValid);
    }

    [TestMethod()]
    [DeploymentItem("CslaContrib.dll")]
    public void Rules_GreaterThan_Test()
    {
      var root = RuleTestRoot.NewEditableRoot();
      // no rules are run for this new object

      Assert.IsTrue(root.IsValid);
      // Num3 must be greater than Num6 (=0)

      root.Num6 = 5;
      root.Num4 = 6;
      root.Num3 = -1;
      Assert.IsFalse(root.IsValid);
      Assert.AreEqual("rule://cslacontrib.rules.commonrules.greaterthan/Num3", root.BrokenRulesCollection[0].RuleName);
      root.Num3 = 6;
      Assert.IsTrue(root.IsValid);
      root.Num3 = 5;
      Assert.IsFalse(root.IsValid);
      Assert.AreEqual("rule://cslacontrib.rules.commonrules.greaterthan/Num3", root.BrokenRulesCollection[0].RuleName);
      root.Num3 = 6;
      Assert.IsTrue(root.IsValid);
    }

    [TestMethod()]
    [DeploymentItem("CslaContrib.dll")]
    public void Rules_GreaterThanOrEqual_Test()
    {
      var root = RuleTestRoot.NewEditableRoot();
      // no rules are run for this new object

      Assert.IsTrue(root.IsValid);
      // Num4 must be greater than Num6 
      root.Num6 = 5;
      root.Num3 = 6;
      root.Num4 = -1;
      Assert.IsFalse(root.IsValid);
      Assert.AreEqual("rule://cslacontrib.rules.commonrules.greaterthanorequal/Num4", root.BrokenRulesCollection[0].RuleName);
      root.Num4 = 6;
      Assert.IsTrue(root.IsValid);
      root.Num4 = -1;
      Assert.IsFalse(root.IsValid);
      Assert.AreEqual("rule://cslacontrib.rules.commonrules.greaterthanorequal/Num4", root.BrokenRulesCollection[0].RuleName);
      root.Num4 = 5;
      Assert.IsTrue(root.IsValid);
    }

    [TestMethod()]
    [DeploymentItem("CslaContrib.dll")]
    public void Rules_ToLowerCase_Test()
    {
      var root = RuleTestRoot.NewEditableRoot();
      // no rules are run for this new object

      Assert.IsTrue(root.IsValid);
      // Num3 must be greater than Num6 (=0)
      root.Lower = string.Empty;
      Assert.AreEqual(string.Empty, root.Lower);
      root.Lower = "ABCDEFghiJ123";
      Assert.AreEqual("abcdefghij123", root.Lower);
      root.Lower = string.Empty;
      Assert.AreEqual(string.Empty, root.Lower);
    }

    [TestMethod()]
    [DeploymentItem("CslaContrib.dll")]
    public void Rules_ToUpperCase_Test()
    {
      var root = RuleTestRoot.NewEditableRoot();
      // no rules are run for this new object

      Assert.IsTrue(root.IsValid);
      // Num3 must be greater than Num6 (=0)
      root.Upper = string.Empty;
      Assert.AreEqual(string.Empty, root.Upper);
      root.Upper = "ABCDEFghiJ123";
      Assert.AreEqual("ABCDEFGHIJ123", root.Upper);
      root.Upper = string.Empty;
      Assert.AreEqual(string.Empty, root.Upper);
    }

    [TestMethod()]
    [DeploymentItem("CslaContrib.dll")]
    public void Rules_CalcSum_Test()
    {
      var root = RuleTestRoot.NewEditableRoot();
      // no rules are run for this new object

      Assert.IsTrue(root.IsValid);
      // Sum is calculated as the sum of Num1,Num2,Num3, Num4
      Assert.AreEqual(0, root.Sum);
      root.Num1 = 1;
      Assert.AreEqual(1, root.Sum);
      root.Num2 = 2;
      Assert.AreEqual(3, root.Sum);
      root.Num3 = 3;
      Assert.AreEqual(6, root.Sum);
      root.Num4 = 4;
      Assert.AreEqual(10, root.Sum);
      root.Num1 = -5;
      Assert.AreEqual(4, root.Sum);
      root.Num2 = 0;
      Assert.AreEqual(2, root.Sum);
      root.Num1 = 0;
      root.Num2 = 0;
      root.Num3 = 0;
      root.Num4 = 0;
      Assert.AreEqual(0, root.Sum);
    }

    [TestMethod]
    public void Rules_CustomErrorMessage()
    {
      var root = RuleTestRoot.NewEditableRoot();
      Assert.AreEqual("US", root.Str1);
      Assert.IsTrue(root.IsValid);
      root.Str1 = string.Empty;
      Assert.IsFalse(root.IsValid);
      var errorMessage = ((IDataErrorInfo)root)[RuleTestRoot.Str1Property.Name];
      Assert.AreEqual("My error message Str1", errorMessage);
      root.Str1 = "US";
      Assert.IsTrue(root.IsValid);
    }
  }
}
