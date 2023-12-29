using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EarTabBrowser.Wpf;
/// <summary>
/// Browser.xaml の相互作用ロジック
/// </summary>
public partial class Browser : UserControl
{
	public Browser()
	{
		InitializeComponent();
		_ = InitializeWebViewAsync();
	}

	private async Task InitializeWebViewAsync()
	{
		await webView.EnsureCoreWebView2Async(null);

		webView.CoreWebView2.WebMessageReceived +=
			(_, e) => addressBar.Text = e.TryGetWebMessageAsString();

		await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
			"window.chrome.webview.postMessage(window.document.URL);");
	}

	private void NavigateIfRetrun(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return &&
			webView != null && webView.CoreWebView2 != null &&
			(addressBar.Text.StartsWith("http://") || addressBar.Text.StartsWith("https://")))
			webView.CoreWebView2.Navigate(addressBar.Text);
	}

	private void GoBack(object sender, RoutedEventArgs e) => webView.CoreWebView2.GoBack();
	private void GoForward(object sender, RoutedEventArgs e) => webView.CoreWebView2.GoForward();
	private void Reload(object sender, RoutedEventArgs e) => webView.CoreWebView2.Reload();
	private void MinimizeWindow(object sender, RoutedEventArgs e) => Window.GetWindow(this).WindowState = WindowState.Minimized;

	private void MaximizeWindow(object sender, RoutedEventArgs e)
	{
		Window mainWindow = Window.GetWindow(this);
		switch (mainWindow.WindowState)
		{
			case WindowState.Normal:
				mainWindow.WindowState = WindowState.Maximized;
				break;
			case WindowState.Maximized:
				mainWindow.WindowState = WindowState.Normal;
				break;
		}
	}

	private void CloseWindow(object sender, RoutedEventArgs e) => Window.GetWindow(this).Close();
}
