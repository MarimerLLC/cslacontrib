using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure;

namespace OutlookStyleApp.Tests
{
    [TestClass]
    public class NotifyCollectionChangedExtensionsFixture
    {
        [TestMethod]
        public void CanAddDelegatesThatWillFireWhenAddingOrRemovingItemsFromCollection()
        {
            var target = new ObservableCollection<object>();
            object itemAdded = null;
            object itemRemoved = null;
            target.RegisterAddAndRemoveDelegates<object>(
                (itemToAdd) => itemAdded = itemToAdd
                , (itemToRemove) => itemRemoved = itemToRemove);

            object expected = new object();
            target.Add(expected);

            Assert.AreSame(expected, itemAdded);
            Assert.IsNull(itemRemoved);

            itemAdded = null;
            itemRemoved = null;

            target.Remove(expected);
            Assert.AreSame(expected, itemRemoved);
            Assert.IsNull(itemAdded);
        }
    }

}
