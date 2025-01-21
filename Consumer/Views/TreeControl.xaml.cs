using CSE.Maui.CustomControls.Consumer.Helpers;
using CSE.Maui.CustomControls.Consumer.Services;

namespace CSE.Maui.CustomControls.Consumer.Views;


public partial class TreeControl : ContentView
{
    DataService service;
    CompanyTreeViewBuilder companyTreeViewBuilder;

    public TreeControl()
    { 
    
    }

    public TreeControl(DataService service, CompanyTreeViewBuilder companyTreeViewBuilder)
    {
        InitializeComponent();
        this.service = service;
        this.companyTreeViewBuilder = companyTreeViewBuilder;

        ProcessTreeView();
    }

    private void ProcessTreeView()
    {
        //var xamlItemGroups = companyTreeViewBuilder.GroupData(service);
        //var rootNodes = TheTreeView.ProcessXamlItemGroups(xamlItemGroups);
        //TheTreeView.RootNodes = rootNodes;
    }
}
