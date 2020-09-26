using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U8SK.ContentNodeIcons.Database;

namespace U8SK.ContentNodeIcons.Api
{
	public interface IContentNodeIconsService
	{
		List<Schema> GetIcons();
		Schema GetIcon(int id);
		Schema SaveIcon(Schema config);
		bool RemoveIcon(int id);
	}
}
