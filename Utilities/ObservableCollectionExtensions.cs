using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Utilities
{
    public static class ObservableCollectionExtensions
    {
        public static void SyncWith<T1, T2>(this ObservableCollection<T1> c1, ObservableCollection<T2> c2,
                                          Func<T1, T2> c1Map, Func<T2, T1> c2Map, Func<T1, T2, bool> compareItems,
                                          Func<T1, bool> c1Filter = null, Func<T2, bool> c2Filter = null)
        {
            c1Filter = c1Filter ?? (it => true);
            c2Filter = c2Filter ?? (it => true);

            c1.CollectionChanged += OnC1Changed;
            c2.CollectionChanged += OnC2Changed;

            void OnC1Changed(object sender, NotifyCollectionChangedEventArgs args)
            {
                c2.CollectionChanged -= OnC2Changed;

                if (args.NewItems != null)
                {
                    foreach (T1 newItem in args.NewItems)
                    {
                        if (c1Filter(newItem))
                            c1Map(newItem).Also(c2.Add);
                    }
                }

                if (args.OldItems != null)
                {
                    foreach (T1 oldItem in args.OldItems)
                    {
                        if (c1Filter(oldItem))
                            c2.RemoveIf(it => compareItems(oldItem, it));
                    }
                }

                c2.CollectionChanged += OnC2Changed;
            }

            void OnC2Changed(object o, NotifyCollectionChangedEventArgs args)
            {
                c1.CollectionChanged -= OnC1Changed;

                if (args.NewItems != null)
                {
                    foreach (T2 newItem in args.NewItems)
                    {
                        if (c2Filter(newItem))
                            c2Map(newItem).Also(c1.Add);
                    }
                }

                if (args.OldItems != null)
                {
                    foreach (T2 oldItem in args.OldItems)
                    {
                        if (c2Filter(oldItem))
                            c1.RemoveIf(it => compareItems(it, oldItem));
                    }
                }

                c1.CollectionChanged += OnC1Changed;
            }
        }
    }
}
