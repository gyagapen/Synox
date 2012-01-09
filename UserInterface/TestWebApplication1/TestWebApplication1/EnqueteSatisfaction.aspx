<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnqueteSatisfaction.aspx.cs" Inherits="TestWebApplication1.EnqueteSatisfaction" MasterPageFile="~/MasterPage.Master" %>

<asp:Content runat=server ContentPlaceHolderID=Main>
    <asp:Wizard ID="Wizard1" runat="server" DisplaySideBar="False">
        <WizardSteps>
            <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>

</asp:Content>
