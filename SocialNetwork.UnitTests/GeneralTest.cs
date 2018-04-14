using System;
using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Interface.Models.Entities;
using Utilities;
using Xunit;

namespace SocialNetwork.UnitTests
{
    public class GeneralTest
    {
        [Fact]
        public void Something()
        {
            "mate"
                .Capitalize()
                .Equals("Mate")
                .Also(Assert.True);

            IEnumerable<Post> posts = CollectionUtils.NewArray(() => new Post(), 10);
        }

        [Fact]
        public void Distinct()
        {
            new[] {"_Ratings", "_Ratings"}
                .Distinct()
                .Let(it => it.Single() == "_Ratings")
                .Also(Assert.True);
        }

        [Fact]
        public void EnumerableWrapper()
        {
            var ints = new[] {23, 34, 456, 12, 56, 67, 23, 45};

            var wrapper = new EnumerableWrapper<int, string, IEnumerable<int>>(ints, it => it.ToString(), filter: it => it > 30);

            wrapper
                .SequenceEqual(new[] { "34", "456", "56", "67", "45" })
                .Also(Assert.True);

            wrapper
                .SequenceEqual(new[] { "34", "456", "56", "67", "45" })
                .Also(Assert.True);
        }

        [Fact]
        public void CollectionWrapper()
        {
            var ints = new List<int> { 23, 34, 456, 12, 56, 67, 23, 45 };

            var wrapper = new CollectionWrapper<int, string>(ints, it => it.ToString(), int.Parse, filter: it => it > 30);

            wrapper.Add("800");
            wrapper.Remove("34");
            wrapper.Add("563");

            // just iterate
            var _ = wrapper.Select(it => it).ToArray();

            //wrapper
            //    .SequenceEqual(new[] { "456", "56", "67", "45", "800", "563" })
            //    .Also(Assert.True);

            //wrapper
            //    .SequenceEqual(new[] { "456", "56", "67", "45", "800", "563" })
            //    .Also(Assert.True);

            //ints
            //    .SequenceEqual(new[] {23, 34, 456, 12, 56, 67, 23, 45, 800, 563})
            //    .Also(Assert.True);
        }
    }
}
