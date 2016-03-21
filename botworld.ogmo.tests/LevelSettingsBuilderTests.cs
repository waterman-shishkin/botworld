using System.Collections.Generic;
using System.Drawing;
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
						<wall x='208' y='176' AttackStrength='10' DefenseStrength='10' />
						<gem x='224' y='240' Points='10' />
						<mine x='192' y='208' AttackStrength='0' />
						<bot x='480' y='416' AttackStrength='20' DefenseStrength='10' HP='100' Intelligence='Human' />
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
		}
	}
}
