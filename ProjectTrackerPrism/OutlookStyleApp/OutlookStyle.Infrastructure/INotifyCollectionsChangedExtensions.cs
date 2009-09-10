using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace OutlookStyle.Infrastructure
{
    public static class INotifyCollectionsChangedExtensions
    {
        public static void RegisterAddAndRemoveDelegates<T>(this INotifyCollectionChanged collection, Action<T> addMethod, Action<T> removeMethod)
            where T : class
        {
            collection.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
                                                {
                                                    DoForEachItemInList(e.NewItems, addMethod);
                                                    DoForEachItemInList(e.OldItems, removeMethod);
                                                };
        }

        private static void DoForEachItemInList<T>(IList list, Action<T> action)
            where T:class
        {
            if (list == null || action == null)
                return;

            foreach(var item in list)
            {
                T castItem = item as T;

                if (castItem != null)
                {
                    action(castItem);
                }
            }
        }
    }
}
