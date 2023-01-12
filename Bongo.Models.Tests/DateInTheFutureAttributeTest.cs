using Bongo.Models.ModelValidations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.Models.Tests
{
    [TestFixture]
    public class DateInTheFutureAttributeTest
    {
        [Test]
        [TestCase(100, ExpectedResult = true)]
        [TestCase(-100, ExpectedResult = false)]
        public bool DateValidator_InputExpectedRange_DateValidity(int additionalTime)
        {
            DateInFutureAttribute dateInFuture = new DateInFutureAttribute(()=>DateTime.Now);
            var result = dateInFuture.IsValid(DateTime.Now.AddSeconds(additionalTime));
            //Assert.That(result, Is.True);
            return result;
        }
        [Test]
        public void DateValidator_NotValidDate_returnErrorMessage()
        {
            var result = new DateInFutureAttribute();
            Assert.AreEqual("Date must be in the future", result.ErrorMessage);
        }
    }
}
