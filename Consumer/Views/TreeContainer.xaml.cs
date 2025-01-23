using CSE.Maui.CustomControls.Consumer.Helpers;
using CSE.Maui.CustomControls.Consumer.Services;

namespace CSE.Maui.CustomControls.Consumer.Views;

public partial class TreeContainer : ContentView
{

    public required DataService service;
    public required CompanyTreeViewBuilder companyTreeViewBuilder;

    public TreeContainer()
    {
        InitializeComponent();
    }

    public TreeContainer(DataService service, CompanyTreeViewBuilder companyTreeViewBuilder)
    {
        //InitializeComponent();
        //this.service = service;
        //this.companyTreeViewBuilder = companyTreeViewBuilder;

        //ProcessTreeView();
    }

    //private void ProcessTreeView()
    //{
    //    var xamlItemGroups = companyTreeViewBuilder.GroupData(service);
    //    var rootNodes = TheTreeView.ProcessXamlItemGroups(xamlItemGroups);
    //    TheTreeView.RootNodes = rootNodes;
    //}


}