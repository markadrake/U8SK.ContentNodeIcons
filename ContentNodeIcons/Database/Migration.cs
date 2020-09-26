using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace U8SK.ContentNodeIcons.Database
{

	/*
	 * Umbraco Documentation: https://our.umbraco.com/documentation/extending/database/
	 */
	public class ContentNodeIconsComponent : IComponent
	{
		private IScopeProvider _scopeProvider;
		private IMigrationBuilder _migrationBuilder;
		private IKeyValueService _keyValueService;
		private ILogger _logger;

		public ContentNodeIconsComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
		{
			_scopeProvider = scopeProvider;
			_migrationBuilder = migrationBuilder;
			_keyValueService = keyValueService;
			_logger = logger;
		}

		public void Initialize()
		{

			var migrationPlan = new MigrationPlan("U8SKContentNodeIcons");

			migrationPlan.From(string.Empty)
				.To<AddContentNodeIconsTable>("contentNodeIcons-db");

			var upgrader = new Upgrader(migrationPlan);
			upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
		}

		public void Terminate()
		{
		}
	}

	public class AddContentNodeIconsTable : MigrationBase
	{
		public AddContentNodeIconsTable(IMigrationContext context) : base(context)
		{
		}

		public override void Migrate()
		{
			Logger.Debug<AddContentNodeIconsTable>("Running migration {MigrationStep}", "AddU8SKContentNodeIconsTable");

			// Lots of methods available in the MigrationBase class - discover with this.
			if (TableExists("U8SK_ContentNodeIcons") == false)
			{
				Create.Table<Schema>().Do();
			}
			else
			{
				Logger.Debug<AddContentNodeIconsTable>("The database table {DbTable} already exists, skipping", "U8SKContentNodeIcons");
			}
		}

	}
}