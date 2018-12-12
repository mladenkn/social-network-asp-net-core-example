using System;
using System.Linq.Expressions;
using FluentAssertions;
using Utilities;
using Xunit;

namespace Utilities.Tests
{
    public class PropertyPathExpressionParserTest
    {
        [Fact]
        public void Should_parse_all()
        {
            void Test(Expression<Func<TestModel, object>> exp, string shouldParse)
            {
                var propertyString = exp.ParseAsPropertyPath();
                propertyString.Should().Be(shouldParse);
            }

            Test(it => it.Item3, "Item3");
            Test(it => it.Item3.Item1, "Item3.Item1");
            Test(it => it.Item1, "Item1");
            Test(it => it.Item3.IntValue, "Item3.IntValue");
            Test(it => it.ValueTestModel.Item1, "ValueTestModel.Item1");
        }
    }

    public class TestModel
    {
        public object Item1 { get; set; }
        public object Item2 { get; set; }
        public TestModel Item3 { get; set; }
        public int IntValue { get; set; }
        public ValueTestModel ValueTestModel { get; set; }
    }

    public struct ValueTestModel
    {
        public object Item1 { get; set; }
    }
}
