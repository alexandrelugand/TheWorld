using TheWorld.Tests.Fakes;
using Xunit;
using TheWorld.Controllers.Web;

namespace TheWorld.Tests.Controllers.Web
{
    public class AppControllerTests
    {
        private readonly FakeMailService _fakeMailService;
        private readonly FakeWorldRepository _fakeWorldRepository;
        private readonly AppController _appController;

        public AppControllerTests()
        {
            _fakeMailService = new FakeMailService();
            _fakeWorldRepository = new FakeWorldRepository();
            _appController = new AppController(_fakeMailService, _fakeWorldRepository);
        }
      

        [Fact(DisplayName = "Module [AppController]")]
        [Trait( "Category", "CI")]
        public void IndexCanRender()
        {
            var result = _appController.Index();
            Assert.NotNull(result);
        }
    }
}
