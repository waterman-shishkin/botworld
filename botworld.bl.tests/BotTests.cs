using NUnit.Framework;

namespace botworld.bl.tests
{
    [TestFixture]
    public class BotTests
    {
        [Test]
        public void Name_Returns_NameSetByConstructor()
        {
            var bot = new Bot("Angry bot");

            Assert.That(bot.Name, Is.EqualTo("Angry bot"));
        }
    }
}
