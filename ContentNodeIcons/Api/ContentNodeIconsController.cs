using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using U8SK.ContentNodeIcons.Database;
using Umbraco.Web.WebApi;

namespace U8SK.ContentNodeIcons.Api
{
	public class ContentNodeIconsController : UmbracoApiController
	{

		private readonly IContentNodeIconsService _contentNodeIconsService;

		public ContentNodeIconsController(IContentNodeIconsService contentNodeIconsService)
		{
			_contentNodeIconsService = contentNodeIconsService;
		}

		[Route("geticons")]
		public List<Schema> GetIcons()
		{
			return _contentNodeIconsService.GetIcons();
		}

		[HttpGet]
		[Route("geticon/{id}")]
		// umbraco/api/contentnodeicons/geticon
		public Schema GetIcon(int id = 0)
		{
			return _contentNodeIconsService.GetIcon(id);
		}

		[HttpPost]
		// umbraco/api/contentnodeicons/saveicon
		public Schema SaveIcon(Schema config)
		{
			return _contentNodeIconsService.SaveIcon(config);
		}

		[HttpGet]
		[Route("removeicon/{id}")]
		// umbraco/api/contentnodeicons/removeicon
		public bool RemoveIcon(int id = 0)
		{
			return _contentNodeIconsService.RemoveIcon(id);
		}

	}
}
