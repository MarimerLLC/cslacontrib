//-----------------------------------------------------------------------
// <copyright file="Page1.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary></summary>
//-----------------------------------------------------------------------
using System;
using Wisej.Web;

namespace ActionExtenderSample
{
  public partial class Page1 : Page
  {
    public Page1()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      var id = "aea60714-d38b-4c08-9c5c-22fe6e0e7e64";
      var orderId = new Guid(id);

      var frm = new OrderMaint(orderId);
      frm.ShowDialog();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      var id = "aea60714-d38b-4c08-9c5c-22fe6e0e7e64";
      var orderId = new Guid(id);

      var frm = new OrderMaint2(orderId);
      frm.ShowDialog();
    }

    private void button3_Click(object sender, EventArgs e)
    {
      var id = "aea60714-d38b-4c08-9c5c-22fe6e0e7e64";
      var orderId = new Guid(id);

      var frm = new OrderMaint3(orderId);
      frm.ShowDialog();
    }
  }
}
