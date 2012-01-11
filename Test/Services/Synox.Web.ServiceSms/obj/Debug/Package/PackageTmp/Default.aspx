<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="Default.aspx.cs" 
    Inherits="Synox.Web.ServiceSms.Default"
    validateRequest="false"
 %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Page Sms</title>
    <link rel="stylesheet" href="./Style.css" type="text/css"/>    
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center;">
    <table>
    <tr>
        <td>    
            <asp:Button ID="ButtonTest" runat="server" Text="Test d'Envoi" 
                onclick="ButtonTest_Click" /><br />
        </td><td>
            <asp:Button ID="Button1" runat="server" Text="En Attente" 
                onclick="ButtonEnAttente_Click"  /><br />
        </td><td>
            <asp:Button ID="ButtonEnErreur" runat="server" Text="En Erreur" 
                onclick="ButtonEnErreur_Click" /><br />
        </td><td>
            <asp:Button ID="ButtonEnvoyes" runat="server" Text="Envoyés" 
                onclick="ButtonEnvoyes_Click" /><br />
        </td><td>
            <asp:Button ID="ButtonSmsRecus" runat="server" Text="Reçus" 
                onclick="ButtonRecus_Click" /><br />
        </td>
    </tr>
    <tr>
        <td colspan="5">
            <asp:Panel runat="server" ID="PanelTest">
            <h3><a href="http://192.168.7.123/M2MSms">pour envoyer les SMS M2M Birdy  << Cliquez ICI >></a></h3>
            <br />
            <hr />
            <br />
            <div> Numéro GSM<br />
                <asp:TextBox ID="TextBoxGsm" runat="server">+33663429874</asp:TextBox><br />
                Message<br />
                <asp:TextBox ID="TextBoxMessage" runat="server" Height="55px" TextMode="MultiLine" Width="216px"></asp:TextBox><br />
                <asp:Button ID="ButtonSend" runat="server" Text="Envoyer" onclick="ButtonSend_Click" />
            </div>
            </asp:Panel>
            <br />
        </td>
    </tr>
    <tr>
        <td>
            <hr />
        </td>
    </tr>
    <tr><td colspan="5">
    <asp:Panel runat="server" ID="PanelSmsAttente">
    <h2>Sms En Attente</h2>
    <div align="center" style="padding:10px; text-align:left;">        
       <asp:ListView ID="ListViewSmsEnAttente" runat="server" 
            ItemPlaceholderID="ItemPlaceHolder" 
            OnPagePropertiesChanging="ListViewSmsEnAttente_PagePropertiesChanging" >
         <LayoutTemplate>
            <table class="tableau" cellpadding="2" cellspacing="0">
                <tbody>
                    <asp:PlaceHolder runat="server" ID="ItemPlaceholder"></asp:PlaceHolder>
                </tbody>
                <tfoot>
                    <tr>
                      <td colspan="8" align="right">
                        <asp:DataPager runat="server" ID="Pager"
                                PagedControlID="ListViewSmsEnAttente"
                                PageSize="50">
                            <Fields>
                            
                              <asp:NextPreviousPagerField ButtonType="Image" 
                                ShowFirstPageButton="true"
                                ShowPreviousPageButton="true"
                                ShowLastPageButton="false"
                                ShowNextPageButton="false"
                                FirstPageImageUrl="~/Images/Icones/Premiere.png"
                                PreviousPageImageUrl="~/Images/Icones/Precedent.png"/>
                              <asp:NumericPagerField ButtonCount="10"/>
                              <asp:NextPreviousPagerField ButtonType="Image"
                                ShowLastPageButton="true"
                                ShowNextPageButton="true"
                                ShowFirstPageButton="false"
                                ShowPreviousPageButton="false"
                                LastPageImageUrl="~/Images/Icones/Derniere.png"
                                NextPageImageUrl="~/Images/Icones/Suivant.png" />                      
                                <asp:TemplatePagerField>
                                    <PagerTemplate>
                                    <table align="center">
                                        <tr>
                                        <td style="width:400px;text-align:center;">
                                            SMS(s) [ <asp:Label ID="Label7" runat="server" Text="<%# Convert.ToInt32(Container.StartRowIndex) +1 %>" />
                                            - <asp:Label ID="Label8" runat="server" Text="<%# (Convert.ToInt32(Container.PageSize)) + (Convert.ToInt32(Container.StartRowIndex))%>" />
                                            ] / <asp:Label ID="Label9" runat="server" Text="<%# Container.TotalRowCount %>" />
                                            
                                            </td>
                                        </tr>
                                    </table>
                                    </PagerTemplate>
                                </asp:TemplatePagerField>
                            </Fields>
                        </asp:DataPager>
                      </td>
                    </tr>           
                </tfoot>
            </table>
         </LayoutTemplate>
         <ItemTemplate>
            <tr>
                <td><asp:Label ID="LabelId" ForeColor="Green" runat="server" Text='<%#Eval("Id") %>'></asp:Label></td>
                <td><asp:Label ID="LabelNumeroGsm" runat="server" Text='<%#Eval("NumeroGSM") %>'></asp:Label></td>
                <td><asp:Label ID="LabelMessage" ForeColor="Blue" runat="server" Text='<%#Eval("Message") %>'></asp:Label></td>
                <td><asp:Label ID="LabelDateDemande" ForeColor="Red" runat="server" Text='<%#Eval("DateDemande") %>'></asp:Label></td>
                <td><asp:Label ID="LabelProjet" ForeColor="Green" runat="server" Text='<%#Eval("Projet.Nom") %>'></asp:Label></td>
            </tr>
         </ItemTemplate>
        <EmptyDataTemplate>
            <div align="center">            
                <asp:Label runat="server" ID="LabelEmptyData" CssClass="title1" Text="Aucun SMS en attente" />
            </div>
        </EmptyDataTemplate>
       </asp:ListView>
    </div>  
    </asp:Panel>
    </td></tr>
    <tr><td colspan="5">
    <asp:Panel runat="server" ID="PanelSmsEnErreur">
    <h2>Sms En Erreur</h2>
    <div align="center" style="padding:10px; text-align:left;">        
       <asp:ListView ID="ListViewSmsEnErreur" runat="server" 
            ItemPlaceholderID="ItemPlaceHolder" 
            OnPagePropertiesChanging="ListViewSmsEnErreur_PagePropertiesChanging" >
         <LayoutTemplate>
            <table class="tableau" cellpadding="2" cellspacing="0">
                <tbody>
                    <asp:PlaceHolder runat="server" ID="ItemPlaceholder"></asp:PlaceHolder>
                </tbody>
                <tfoot>
                    <tr>
                      <td colspan="8" align="right">
                        <asp:DataPager runat="server" ID="Pager"
                                PagedControlID="ListViewSmsEnErreur"
                                PageSize="50">
                            <Fields>
                            
                              <asp:NextPreviousPagerField ButtonType="Image" 
                                ShowFirstPageButton="true"
                                ShowPreviousPageButton="true"
                                ShowLastPageButton="false"
                                ShowNextPageButton="false"
                                FirstPageImageUrl="~/Images/Icones/Premiere.png"
                                PreviousPageImageUrl="~/Images/Icones/Precedent.png"/>
                              <asp:NumericPagerField ButtonCount="10"/>
                              <asp:NextPreviousPagerField ButtonType="Image"
                                ShowLastPageButton="true"
                                ShowNextPageButton="true"
                                ShowFirstPageButton="false"
                                ShowPreviousPageButton="false"
                                LastPageImageUrl="~/Images/Icones/Derniere.png"
                                NextPageImageUrl="~/Images/Icones/Suivant.png" />                      
                                <asp:TemplatePagerField>
                                    <PagerTemplate>
                                    <table align="center">
                                        <tr>
                                        <td style="width:400px;text-align:center;">
                                            SMS(s) [ <asp:Label ID="Label7" runat="server" Text="<%# Convert.ToInt32(Container.StartRowIndex) +1 %>" />
                                            - <asp:Label ID="Label8" runat="server" Text="<%# (Convert.ToInt32(Container.PageSize)) + (Convert.ToInt32(Container.StartRowIndex))%>" />
                                            ] / <asp:Label ID="Label9" runat="server" Text="<%# Container.TotalRowCount %>" />
                                            
                                            </td>
                                        </tr>
                                    </table>
                                    </PagerTemplate>
                                </asp:TemplatePagerField>
                            </Fields>
                        </asp:DataPager>
                      </td>
                    </tr>           
                </tfoot>
            </table>
         </LayoutTemplate>
         <ItemTemplate>
            <tr>
                <td><asp:Label ID="LabelId" ForeColor="Green" runat="server" Text='<%#Eval("Id") %>'></asp:Label></td>
                <td><asp:Label ID="LabelNumeroGsm" runat="server" Text='<%#Eval("NumeroGSM") %>'></asp:Label></td>
                <td><asp:Label ID="LabelMessage" ForeColor="Blue" runat="server" Text='<%#Eval("Message") %>'></asp:Label></td>
                <td><asp:Label ID="LabelDateDemande" runat="server" Text='<%#Eval("DateDemande") %>'></asp:Label></td>
                <td><asp:Label ID="LabelDateEnvoi" ForeColor="Red" runat="server" Text='<%#Eval("DateEnvoi") %>'></asp:Label></td>
                <td><asp:Label ID="LabelStatutId" runat="server" Text='<%#Eval("SmsStatut.Id") %>' ToolTip='<%#Eval("SmsStatut.Nom") %>'></asp:Label></td>
                <td><asp:Label ID="LabelProjet" ForeColor="Green" runat="server" Text='<%#Eval("Projet.Nom") %>'></asp:Label></td>
            </tr>
         </ItemTemplate>
        <EmptyDataTemplate>
            <div align="center">            
                <asp:Label runat="server" ID="LabelEmptyData" CssClass="title1" Text="Aucun SMS en erreur" />
            </div>
        </EmptyDataTemplate>
       </asp:ListView>
    </div>
    </asp:Panel>
    </td></tr>
    <tr><td colspan="5">
    <asp:Panel runat="server" ID="PanelSmsRecu">
    <h2>Sms Recus</h2>
    <div align="center" style="padding:10px; text-align:left;">        
       <asp:ListView ID="ListViewSmsRecus" runat="server" 
            ItemPlaceholderID="ItemPlaceHolder" 
            OnPagePropertiesChanging="ListViewSmsRecus_PagePropertiesChanging" >
         <LayoutTemplate>
            <table class="tableau" cellpadding="2" cellspacing="0">
                <tbody>
                    <asp:PlaceHolder runat="server" ID="ItemPlaceholder"></asp:PlaceHolder>
                </tbody>
                <tfoot>
                    <tr>
                      <td colspan="8" align="right">
                        <asp:DataPager runat="server" ID="Pager"
                                PagedControlID="ListViewSmsRecus"
                                PageSize="50">
                            <Fields>
                            
                              <asp:NextPreviousPagerField ButtonType="Image" 
                                ShowFirstPageButton="true"
                                ShowPreviousPageButton="true"
                                ShowLastPageButton="false"
                                ShowNextPageButton="false"
                                FirstPageImageUrl="~/Images/Icones/Premiere.png"
                                PreviousPageImageUrl="~/Images/Icones/Precedent.png"/>
                              <asp:NumericPagerField ButtonCount="10"/>
                              <asp:NextPreviousPagerField ButtonType="Image"
                                ShowLastPageButton="true"
                                ShowNextPageButton="true"
                                ShowFirstPageButton="false"
                                ShowPreviousPageButton="false"
                                LastPageImageUrl="~/Images/Icones/Derniere.png"
                                NextPageImageUrl="~/Images/Icones/Suivant.png" />                      
                                <asp:TemplatePagerField>
                                    <PagerTemplate>
                                    <table align="center">
                                        <tr>
                                        <td style="width:400px;text-align:center;">
                                            SMS(s) [ <asp:Label ID="Label7" runat="server" Text="<%# Convert.ToInt32(Container.StartRowIndex) +1 %>" />
                                            - <asp:Label ID="Label8" runat="server" Text="<%# (Convert.ToInt32(Container.PageSize)) + (Convert.ToInt32(Container.StartRowIndex))%>" />
                                            ] / <asp:Label ID="Label9" runat="server" Text="<%# Container.TotalRowCount %>" />
                                            
                                            </td>
                                        </tr>
                                    </table>
                                    </PagerTemplate>
                                </asp:TemplatePagerField>
                            </Fields>
                        </asp:DataPager>
                      </td>
                    </tr>           
                </tfoot>
            </table>
         </LayoutTemplate>
         <ItemTemplate>
            <tr>
                <td><asp:Label ID="LabelId" ForeColor="Green" runat="server" Text='<%#Eval("Id") %>'></asp:Label></td>
                <td><asp:Label ID="LabelNumeroGsm" runat="server" Text='<%#Eval("NumeroGSM") %>'></asp:Label></td>
                <td><asp:Label ID="LabelMessage" ForeColor="Blue" runat="server" Text='<%#Eval("Message") %>'></asp:Label></td>
                <td><asp:Label ID="LabelDateReception" ForeColor="Red" runat="server" Text='<%#Eval("DateReception") %>'></asp:Label></td>
                <td><asp:Label ID="LabelProjet" ForeColor="Green" runat="server" Text='<%#Eval("Projet.Nom") %>'></asp:Label></td>
            </tr>
         </ItemTemplate>
        <EmptyDataTemplate>
            <div align="center">            
                <asp:Label runat="server" ID="LabelEmptyData" CssClass="title1" Text="Aucun SMS reçu" />
            </div>
        </EmptyDataTemplate>
       </asp:ListView>
    </div>  
    </asp:Panel>
    </td></tr>
    <tr><td colspan="5">
    <asp:Panel runat="server" ID="PanelSmsEnvoyes">
    <h2>Sms Envoyés</h2>
    <div align="center" style="padding:10px; text-align:left;">        
       <asp:ListView ID="ListViewSmsEnvoyes" runat="server" 
            ItemPlaceholderID="ItemPlaceHolder" 
            OnPagePropertiesChanging="ListViewSmsEnvoyes_PagePropertiesChanging" >
         <LayoutTemplate>
            <table class="tableau" cellpadding="2" cellspacing="0">
                <tbody>
                    <asp:PlaceHolder runat="server" ID="ItemPlaceholder"></asp:PlaceHolder>
                </tbody>
                <tfoot>
                    <tr>
                      <td colspan="8" align="right">
                        <asp:DataPager runat="server" ID="Pager"
                                PagedControlID="ListViewSmsEnvoyes"
                                PageSize="50">
                            <Fields>
                            
                              <asp:NextPreviousPagerField ButtonType="Image" 
                                ShowFirstPageButton="true"
                                ShowPreviousPageButton="true"
                                ShowLastPageButton="false"
                                ShowNextPageButton="false"
                                FirstPageImageUrl="~/Images/Icones/Premiere.png"
                                PreviousPageImageUrl="~/Images/Icones/Precedent.png"/>
                              <asp:NumericPagerField ButtonCount="10"/>
                              <asp:NextPreviousPagerField ButtonType="Image"
                                ShowLastPageButton="true"
                                ShowNextPageButton="true"
                                ShowFirstPageButton="false"
                                ShowPreviousPageButton="false"
                                LastPageImageUrl="~/Images/Icones/Derniere.png"
                                NextPageImageUrl="~/Images/Icones/Suivant.png" />                      
                                <asp:TemplatePagerField>
                                    <PagerTemplate>
                                    <table align="center">
                                        <tr>
                                        <td style="width:400px;text-align:center;">
                                            SMS(s) [ <asp:Label ID="Label7" runat="server" Text="<%# Convert.ToInt32(Container.StartRowIndex) +1 %>" />
                                            - <asp:Label ID="Label8" runat="server" Text="<%# (Convert.ToInt32(Container.PageSize)) + (Convert.ToInt32(Container.StartRowIndex))%>" />
                                            ] / <asp:Label ID="Label9" runat="server" Text="<%# Container.TotalRowCount %>" />
                                            
                                            </td>
                                        </tr>
                                    </table>
                                    </PagerTemplate>
                                </asp:TemplatePagerField>
                            </Fields>
                        </asp:DataPager>
                      </td>
                    </tr>           
                </tfoot>
            </table>
         </LayoutTemplate>
         <ItemTemplate>
            <tr>
                <td><asp:Label ID="LabelId" ForeColor="Green" runat="server" Text='<%#Eval("Id") %>'></asp:Label></td>
                <td><asp:Label ID="LabelNumeroGsm" runat="server" Text='<%#Eval("NumeroGSM") %>'></asp:Label></td>
                <td><asp:Label ID="LabelMessage" ForeColor="Blue" runat="server" Text='<%#Eval("Message") %>'></asp:Label></td>
                <td><asp:Label ID="LabelDateDemande" runat="server" Text='<%#Eval("DateDemande") %>'></asp:Label></td>
                <td><asp:Label ID="LabelDateEnvoi" ForeColor="Red" runat="server" Text='<%#Eval("DateEnvoi") %>'></asp:Label></td>
                <td><asp:Label ID="LabelProjet" ForeColor="Green" runat="server" Text='<%#Eval("Projet.Nom") %>'></asp:Label></td>
            </tr>
         </ItemTemplate>
        <EmptyDataTemplate>
            <div align="center">            
                <asp:Label runat="server" ID="LabelEmptyData" CssClass="title1" Text="Aucun SMS reçu" />
            </div>
        </EmptyDataTemplate>
       </asp:ListView>
    </div>  
    </asp:Panel>
    </td></tr>
    </table></div>
    </form>
</body>
</html>
