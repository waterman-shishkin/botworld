using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using botworld.bl;
using NUnit.Framework;

namespace botworld.ogmo.tests
{
	[TestFixture]
	public class LevelSettingsBuilderTests
	{
		[Test]
		public void Build_ForProjectAndLevelDocuments_ReturnsLevelSettings()
		{
			const string project =
				@"<project>
					<BackgroundColor A='255' R='192' G='192' B='192' />
					<LayerDefinitions>
						<LayerDefinition>
							<Name>Entities</Name>
							<Grid>
								<Width>16</Width>
								<Height>16</Height>
							</Grid>
						</LayerDefinition>
					</LayerDefinitions>
					<EntityDefinitions>
						<EntityDefinition Name='wall'>
							<ImageDefinition ImagePath='wall.png'/>
						</EntityDefinition>
						<EntityDefinition Name='gem'>
							<ImageDefinition ImagePath='gem.png'/>
						</EntityDefinition>
						<EntityDefinition Name='mine'>
							<ImageDefinition ImagePath='mine.png'/>
						</EntityDefinition>
						<EntityDefinition Name='bot'>
							<ImageDefinition ImagePath='bot.png'/>
						</EntityDefinition>
					</EntityDefinitions>
				</project>";
			var projectDoc = XDocument.Parse(project);

			const string level =
				@"<level width='512' height='640' ScenarioJSON='{&#xD;&#xA;	&quot;type&quot;: &quot;points&quot;,&#xD;&#xA;	&quot;wp&quot;: 20&#xD;&#xA;}'>
					<Entities>
						<wall x='208' y='176' AttackStrength='10' DefenseStrength='20' HP='100' />
						<gem x='224' y='240' WP='10' />
						<mine x='192' y='208' AttackStrength='50' />
						<bot x='480' y='416' Name='Bot' AttackStrength='20' AutoDamageStrength='0' DefenseStrength='10' HP='100' IntelligenceJSON='{&#xD;&#xA;	&quot;type&quot;: &quot;human&quot;&#xD;&#xA;}' Direction='North' />
					</Entities>
				</level>";
			var levelDoc = XDocument.Parse(level);

			var settings = new LevelSettingsBuilder().ForProject(projectDoc).ForLevel(levelDoc).Build();

			Assert.That(settings.BackgroundColor, Is.EqualTo(Color.FromArgb(255, 192, 192, 192)));
			Assert.That(settings.CellSize, Is.EqualTo(16));
			Assert.That(settings.EntitiesImageFilenames, Is.EqualTo(new Dictionary<string, string> { { "wall", "wall.png" }, { "gem", "gem.png" }, { "mine", "mine.png" }, { "bot", "bot.png" } }));
			Assert.That(settings.Width, Is.EqualTo(32));
			Assert.That(settings.Height, Is.EqualTo(40));
			Assert.That(settings.Scenario, Is.TypeOf<PointsScenario>());
			var scenario = settings.Scenario as PointsScenario;
			Assert.That(scenario.WP, Is.EqualTo(20));
			var entities = settings.Entities.ToArray();
			Assert.That(entities[0], Is.TypeOf<Wall>());
			var wall = entities[0] as Wall;
			Assert.That(wall.Location, Is.EqualTo(new Location(13, 11)));
			Assert.That(wall.HP, Is.EqualTo(100));
			Assert.That(wall.AttackStrength, Is.EqualTo(10));
			Assert.That(wall.DefenseStrength, Is.EqualTo(20));
			Assert.That(entities[1], Is.TypeOf<Gem>());
			var gem = entities[1] as Gem;
			Assert.That(gem.Location, Is.EqualTo(new Location(14, 15)));
			Assert.That(gem.WP, Is.EqualTo(10));
			Assert.That(entities[2], Is.TypeOf<Mine>());
			var mine = entities[2] as Mine;
			Assert.That(mine.Location, Is.EqualTo(new Location(12, 13)));
			Assert.That(mine.AttackStrength, Is.EqualTo(50));
			Assert.That(entities[3], Is.TypeOf<Bot>());
			var bot = entities[3] as Bot;
			Assert.That(bot.Location, Is.EqualTo(new Location(30, 26)));
			Assert.That(bot.Name, Is.EqualTo("Bot"));
			Assert.That(bot.AttackStrength, Is.EqualTo(20));
			Assert.That(bot.AutoDamageStrength, Is.EqualTo(0));
			Assert.That(bot.DefenseStrength, Is.EqualTo(10));
			Assert.That(bot.HP, Is.EqualTo(100));
			Assert.That(bot.Direction, Is.EqualTo(Direction.North));
			Assert.That(bot.Intelligence, Is.TypeOf<HumanControlBotIntelligence>());
		}
	}
}
