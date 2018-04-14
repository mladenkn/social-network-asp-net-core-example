using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Utilities
{
    public static class ObservableCollectionExtensions
    {
        public static void SyncWith<T, U>(this ObservableCollection<T> c1, ObservableCollection<U> c2,
                                          Func<T, U> c1Map, Func<U, T> c2Map,
                                          Func<T, bool> c1Filter = null, Func<U, bool> c2Filter = null)
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
                    foreach (T newItem in args.NewItems)
                    {
                        if (c1Filter(newItem))
                            c1Map(newItem).Also(c2.Add);
                    }
                }

                if (args.OldItems != null)
                {
                    foreach (T oldItem in args.OldItems)
                    {
                        if (c1Filter(oldItem))
                            c1Map(oldItem).Also(c2.Remove);
                    }
                }

                c2.CollectionChanged += OnC2Changed;
            }

            void OnC2Changed(object o, NotifyCollectionChangedEventArgs args)
            {
                c1.CollectionChanged -= OnC1Changed;

                if (args.NewItems != null)
                {
                    foreach (U newItem in args.NewItems)
                    {
                        if (c2Filter(newItem))
                            c2Map(newItem).Also(c1.Add);
                    }
                }

                if (args.OldItems != null)
                {
                    foreach (U oldItem in args.OldItems)
                    {
                        if (c2Filter(oldItem))
                            c2Map(oldItem).Also(c1.Remove);
                    }
                }

                c1.CollectionChanged += OnC1Changed;
            }
        }
    }
}
