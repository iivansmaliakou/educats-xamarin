﻿using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Pages.Testing.Results.ViewModels;
using EduCATS.Pages.Testing.Results.Views.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Results.Views
{
	public class TestingResultsPageView : ContentPage
	{
		public TestingResultsPageView(int testId, bool fromComplexLearning)
		{
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Title = CrossLocalization.Translate("test_results_title");
			BindingContext = new TestingResultsPageViewModel(
				testId, fromComplexLearning, new AppDialogs(), new AppPages());
			createToolbar();
			createViews();
		}

		void createToolbar()
		{
			var closeItem = new ToolbarItem {
				IconImageSource = ImageSource.FromFile(Theme.Current.BaseCloseIcon)
			};

			closeItem.SetBinding(MenuItem.CommandProperty, "CloseCommand");
			ToolbarItems.Add(closeItem);
		}

		void createViews()
		{
			Content = createList();
		}

		ListView createList()
		{
			var markTitleLabel = createRatingTitleLabel();
			var markLabel = createRatingLabel();

			var listView = new ListView {
				Header = new StackLayout {
					Padding = new Thickness(10),
					Children = {
						markTitleLabel,
						markLabel
					}
				},
				HasUnevenRows = true,
				SelectionMode = ListViewSelectionMode.None,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = new DataTemplate(typeof(TestingResultsViewCell)),
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor)
			};

			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Results");
			return listView;
		}

		Label createRatingTitleLabel()
		{
			return new Label {
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = CrossLocalization.Translate("test_results_label")
			};
		}

		Label createRatingLabel()
		{
			var mark = new Label {
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
			};

			mark.SetBinding(Label.TextProperty, "Mark");
			return mark;
		}
	}
}