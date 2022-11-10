namespace Selenium.Tests
{
    public class ApiTests
    {
        private IAPI _api;

        [SetUp]
        public void SetUp()
        {
            _api = new API();
        }

        [Test]
        public async Task GetRandomMail_notnullreturned()
        {
            var responce = await _api.GetRandomMail(new RandomEmailParseRequest());
            Assert.IsNotNull(responce.Email);
        }

        [Test]
        public async Task GetRandomName_notnullreturned()
        {
            var responce = await _api.GetRandomName(new RandomNameParseRequest());
            Assert.IsNotNull(responce.Name);
        }
    }
}