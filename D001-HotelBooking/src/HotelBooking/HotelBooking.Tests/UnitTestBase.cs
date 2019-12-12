using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace HotelBooking.Tests
{
    internal abstract class UnitTestBase
    {
        protected MockRepository MockRepository { get; private set; }

        [SetUp]
        public void UnitTestBaseTestInitialize() => MockRepository = new MockRepository(MockBehavior.Strict);

        [TearDown]
        public void UnitTestBaseTearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
            {
                MockRepository.VerifyAll();
            }
        }
    }
}