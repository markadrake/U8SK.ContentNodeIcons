using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using U8SK.ContentNodeIcons.Api;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

namespace U8SK.ContentNodeIcons.Trees
{

	public class TreeEvents : IComponent
	{

		private readonly IContentNodeIconsService _contentNodeIconsService;

		public TreeEvents(IContentNodeIconsService contentNodeIconsService)
		{
			_contentNodeIconsService = contentNodeIconsService;
		}

		public void Initialize()
		{
			TreeControllerBase.TreeNodesRendering += TreeControllerBase_TreeNodesRendering;
			TreeControllerBase.MenuRendering += TreeControllerBase_MenuRendering;
		}

		// Hijack the rendering of each content node to customize the icon shown.
		void TreeControllerBase_TreeNodesRendering(TreeControllerBase sender, TreeNodesRenderingEventArgs e)
		{

			if (sender.TreeAlias == Constants.Trees.Content)
			{

				var customIcons = _contentNodeIconsService.GetIcons();

				foreach (TreeNode treeNode in e.Nodes)
				{
					int nodeId = Convert.ToInt32(treeNode.Id);
					if(nodeId <= 0)
					{
						continue;
					}

					var node = customIcons.Where(x => x.ContentId.Equals(nodeId)).FirstOrDefault();
					if(node == null)
					{
						continue;
					}

					treeNode.Icon = $"{node.Icon} {node.IconColor}";
				}

			}
		}

		// Provide a new context menu option.
		void TreeControllerBase_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
		{

			if (sender.TreeAlias == Constants.Trees.Content && !e.NodeId.Equals("-1"))
			{
				// creates a menu action that will open /umbraco/currentSection/itemAlias.html
				var i = new MenuItem("setIcon", "Set Icon");

				// optional, if you don't want to follow the naming conventions, but do want to use a angular view
				// you can also use a direct path "../App_Plugins/my/long/url/to/view.html"
				i.AdditionalData.Add("actionView", "/app_plugins/u8sk.contentnodeicons/tree.action.seticontemplate.html");

				// sets the icon to icon-wine-glass
				i.Icon = "favorite";

				// adds "..."
				i.OpensDialog = true;

				// insert at index 5
				e.Menu.Items.Insert(5, i);
			}
		}
		public void Terminate()
		{
			TreeControllerBase.TreeNodesRendering -= TreeControllerBase_TreeNodesRendering;
			TreeControllerBase.MenuRendering -= TreeControllerBase_MenuRendering;
		}

	}
}
