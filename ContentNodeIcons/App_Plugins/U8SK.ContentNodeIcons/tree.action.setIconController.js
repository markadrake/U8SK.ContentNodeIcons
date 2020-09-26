/*
	Slightly modified version from Umbraco CMS:
		- Umbraco-CMS/src/Umbraco.Web.UI.Client/src/views/common/infiniteeditors/iconpicker/iconpicker.controller.js
		- https://github.com/umbraco/Umbraco-CMS/blob/add1a99cc407af1971e22cbecf36d2c616ad1b4e/src/Umbraco.Web.UI.Client/src/views/common/infiniteeditors/iconpicker/iconpicker.controller.js

	- On load, make API request to find color & icon property values.
	- On submit, make another API request to save color & icon property values.
	- On close, the navigation service is used to close the dialog.
	- New button to remove / unset the custom icon.
	- On remove, new API call, and close the dialog.
	- Notification service added.
	- Tree service added.
*/
(function () {
	"use strict";

	function IconPickerController($q, $scope, $http, iconHelper, localizationService, navigationService, notificationsService, appState, treeService) {
		var vm = this;
		vm.hasIconSet = false;
		vm.selectIcon = selectIcon;
		vm.selectColor = selectColor;
		vm.submit = submit;
		vm.close = close;
		vm.remove = remove;
		vm.colors = [
			{
				name: 'Black',
				value: 'color-black',
				default: true
			},
			{
				name: 'Blue Grey',
				value: 'color-blue-grey'
			},
			{
				name: 'Grey',
				value: 'color-grey'
			},
			{
				name: 'Brown',
				value: 'color-brown'
			},
			{
				name: 'Blue',
				value: 'color-blue'
			},
			{
				name: 'Light Blue',
				value: 'color-light-blue'
			},
			{
				name: 'Indigo',
				value: 'color-indigo'
			},
			{
				name: 'Purple',
				value: 'color-purple'
			},
			{
				name: 'Deep Purple',
				value: 'color-deep-purple'
			},
			{
				name: 'Cyan',
				value: 'color-cyan'
			},
			{
				name: 'Green',
				value: 'color-green'
			},
			{
				name: 'Light Green',
				value: 'color-light-green'
			},
			{
				name: 'Lime',
				value: 'color-lime'
			},
			{
				name: 'Yellow',
				value: 'color-yellow'
			},
			{
				name: 'Amber',
				value: 'color-amber'
			},
			{
				name: 'Orange',
				value: 'color-orange'
			},
			{
				name: 'Deep Orange',
				value: 'color-deep-orange'
			},
			{
				name: 'Red',
				value: 'color-red'
			},
			{
				name: 'Pink',
				value: 'color-pink'
			}
		];

		if (!$scope.model) {
			$scope.model = {};
		}

		function onBeforeInit() {
			getCurrentNodesIcon().then((data) => {
				// This content node has a custom icon set.
				vm.hasIconSet = true;
				$scope.model.icon = data.icon;
				$scope.model.color = data.color;
				onInit();
			}, () => {
				// This content node does not have a custom icon.
				onInit();
			});
		}
		function getCurrentNodesIcon() {
			// make api request with $scope.currentNode.id to get current icon configuration
			const contentId = $scope.currentNode.id;

			return $q(function (resolve, reject) {
				$http.get(`/umbraco/api/contentnodeicons/geticon/${contentId}`).then((response) => {
					if (!response.data) reject(null);

					let data = response.data;
					if (!data.icon || !data.iconColor) reject(null);

					resolve({
						icon: data.icon,
						color: data.iconColor
					});
				}, () => {
					reject(null);
				});
			});
		}
		function onInit() {
			vm.loading = true;
			setTitle();
			iconHelper.getIcons().then(function (icons) {
				vm.icons = icons;
				vm.loading = false;
			});
			// set a default color if nothing is passed in
			vm.color = $scope.model.color ? findColor($scope.model.color) : vm.colors.find(function (x) {
				return x.default;
			});
			// if an icon is passed in - preselect it
			vm.icon = $scope.model.icon ? $scope.model.icon : undefined;
		}
		function setTitle() {
			if (!$scope.model.title) {
				localizationService.localize('defaultdialogs_selectIcon').then(function (data) {
					$scope.model.title = data;
				});
			}
		}
		function selectIcon(icon, color) {
			$scope.model.icon = icon;
			$scope.model.color = color;
			submit();
		}
		function findColor(value) {
			return vm.colors.find(function (x) {
				return x.value === value;
			});
		}
		function selectColor(color) {
			var newColor = color || vm.colors.find(function (x) {
				return x.default;
			});
			$scope.model.color = newColor.value;
			vm.color = newColor;
		}
		function close() {
			navigationService.hideMenu();
		}
		function submit() {
			const contentId = $scope.currentNode.id,
				icon = $scope.model.icon,
				iconColor = $scope.model.color;

			$http.post("/umbraco/api/contentnodeicons/saveicon", {
				contentId,
				icon,
				iconColor
			}).then((response) => {
				close();
				refreshTree();
				notificationsService.success("Icon Set", "Your custom icon has been applied to this content node.");
			}, (reject) => {
				notificationsService.error("Icon Not Set", "Something went wrong.");
			});
		}
		function remove() {
			$http.get(`/umbraco/api/contentnodeicons/removeicon/${$scope.currentNode.id}`).then(() => {
				notificationsService.success("Icon Removed", "Your custom icon has been removed from this content node.");
				close();
				refreshTree();
			}, () => {
				notificationsService.error("Icon Not Removed", "Something went wrong.");
			});
		}
		function refreshTree() {
			let currentNode = appState.getTreeState("selectedNode"),
				currentPath = treeService.getPath(currentNode);

			treeService.loadNodeChildren({ node: currentNode.parent() }).then(() => {
				navigationService.syncTree({ tree: "content", path: currentPath, forceReload: true });
			});
		}
		onBeforeInit();
	}

	angular.module("umbraco").controller("U8SK.ContentNodeIcons.SetIconController", IconPickerController);

})();