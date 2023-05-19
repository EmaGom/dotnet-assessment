//using TimeChimp.Backend.Assessment.Controllers.V1;
//using TimeChimp.Backend.Assessment.UnitTests.Abstract;
//using TimeChimp.Backend.Assessment.UnitTests.Extensions;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Stashbox.Mocking.Moq;
//using Xunit;

//namespace TimeChimp.Backend.Assessment.UnitTests.ControllerTests.V1
//{
//    public class ExampleControllerFixture : ServiceFixture
//    {
//    }

//    public class ExampleControllerTest : ServiceControllerTestBase<ExampleControllerFixture>
//    {
//        public ExampleControllerTest(ExampleControllerFixture serviceFixture)
//            : base(serviceFixture) { }

//        [Fact]
//        public void Get()
//        {
//            using var stash = StashMoq.Create();
//            var controller = stash.GetWithParamOverrides<FeedController>().BypassStub(stash);

//            //Call the tested method
//            var result = controller.Get();

//            //Check the result
//            var okResult = result as OkResult;
//            okResult.Should().NotBeNull();
//        }
//    }
//}
